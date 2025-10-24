#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Lucecita.StorylineEngine
{
    public static class EpisodeCSVCreator
    {
        private const string StartFunctionKeyword = "#";
        private const string EndFunctionKeyword = "/";
        private const string FunctionValueKeyword = "=";

        public static TextAsset[] Create()
        {
            string episodeCSVPath = StoryUtil.GetEpisodeCSVPath();
            string[] episodeGUIDs = AssetDatabase.FindAssets("t:TextAsset", new[] { episodeCSVPath });

            Debug.Log($"찾은 스토리 수 : {episodeGUIDs.Length}");

            VoiceDebugger.voiceList = new Dictionary<string, List<string>>();

            List<TextAsset> textAssets = new List<TextAsset>();
            List<string> csvPathList = new List<string>();

            // 가져온 스토리 수만큼 에피소드를 생성합니다.
            foreach (string episodeGUID in episodeGUIDs)
            {
                string episodePath = AssetDatabase.GUIDToAssetPath(episodeGUID);
                TextAsset textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(episodePath);
                string fileName = textAsset.name;
                string episodeName = null;

                if (fileName.Contains("_"))
                {
                    episodeName = fileName.Split("_")[1];
                }
                else
                {
                    Debug.LogError("에피소드 이름은 _ 옆에 붙여야 합니다.");
                }

                string csvPath = CreateCSV(episodePath, episodeName);
                csvPathList.Add(csvPath);

                Debug.Log($"{textAsset.name} 생성 => {episodeName}");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            List<TextAsset> csvTextAssets = GetCSV(csvPathList);
            textAssets.AddRange(csvTextAssets);

            return textAssets.ToArray();
        }

        private static string CreateCSV(string episodePath, string episodeName)
        {
            TextAsset textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(episodePath);
            string rawData = textAsset.text;

            // 라인 단위 분리
            List<string> row = new List<string>(rawData.Split("\r\n"));

            // 공백/주석 제거
            row.RemoveAll(s => string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s));
            row.RemoveAll(r => r.StartsWith("//"));

            // CSV 빌더 준비
            StringBuilder csvBuilder = new StringBuilder();
            csvBuilder.AppendLine(
                "Index,EpisodeName,Uid,Text,Characters,FunctionName,FunctionType,FunctionValues,SelectElements,Conversation,VoicePath"
            );

            for (int i = 0; i < row.Count; i++)
            {
                int index = i;
                string uid = Guid.NewGuid().ToString();
                string text = row[i].Trim().Replace("\"", "\"\"");;
                string characters = ParseCharacters(text);
                string functionName = ParseFunctionName(text);
                string functionType = GetFunctionType(text);
                string functionValues = ParseFunctionValues(text);
                string selectElements = GetSelectElementsIndex(row,index);
                string voicePath = GetVoicePath(characters, text);
                string conversation = ParseConversation(text);

                // ---- CSV 라인 생성 ----
                csvBuilder.AppendLine(
                    $"{index},{episodeName},\"{uid}\",\"{text}\"," +
                    $"\"{characters}\",{functionName},{functionType},\"{functionValues}\",\"{selectElements}\",\"{conversation}\",\"{voicePath}\""
                );
            }

            // 저장 경로 지정
            string directory = $"{StoryUtil.Relative_EnginePath}/Data/EpisodeCSV";
            string savePath = Path.Combine(directory, $"{episodeName}.csv");
            File.WriteAllText(savePath, csvBuilder.ToString(), Encoding.UTF8);


            Debug.Log($"CSV 저장 완료: {savePath}");
            return savePath;
        }

        private static string ParseFunctionName(string text)
        {
            bool isConversationFunction = text.StartsWith(StartFunctionKeyword) == false &&
                                          text.StartsWith(EndFunctionKeyword) == false;
            if (isConversationFunction)
                return "Conversation";

            string filteredText = text.Replace(StartFunctionKeyword, "");
            filteredText = filteredText.Replace(EndFunctionKeyword, "");
            string[] functionNameSplit = filteredText.Split(FunctionValueKeyword);

            return functionNameSplit[0];
        }

        private static string GetFunctionType(string text)
        {
            bool isStartFunction = text.StartsWith(StartFunctionKeyword);
            bool isEndFunction = text.StartsWith(EndFunctionKeyword);
            bool isFunction = isStartFunction || isEndFunction;
            if (!isFunction)
                return "Start";

            string functionType = isStartFunction ? "Start" : "End";
            return functionType;
        }

        private static string ParseFunctionValues(string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;


            string sharpCut = text.Remove(0, 1);
            List<string> functionNameCut = sharpCut.Split("=").ToList();
            List<string> functionValues = new List<string>();

            bool isFunction = text.StartsWith("#") || text.StartsWith("/");
            if (!isFunction)
                return null;

            if (functionNameCut.Count > 1)
            {
                if (functionNameCut.Count > 2)
                {
                    string firstIndexValue = functionNameCut[0];
                    string combinedRest = string.Join("=", functionNameCut.Skip(1));
                    functionNameCut[0] = firstIndexValue;
                    functionNameCut[1] = combinedRest;
                    functionNameCut = functionNameCut.Take(2).ToList();
                }

                string values = functionNameCut[1];

                if (values.Contains(","))
                {
                    foreach (string value in values.Split(','))
                    {
                        functionValues.Add(value);
                    }
                }
                else
                {
                    functionValues.Add(values);
                }
            }

            return string.Join(",", functionValues);
        }

        private static string ParseConversation(string text)
        {
            bool isFunction = text.StartsWith("#") || text.StartsWith("/");
            if (isFunction)
                return null;

            string[] characterSplit = text.Split(":");
            if (characterSplit.Length > 1)
            {
                string conversation = characterSplit[1];
                conversation = conversation.TrimStart();
                return conversation;
            }
            else
            {
                return text;
            }
        }

        private static string ParseCharacters(string text)
        {
            string[] characterSplit = text.Split(':');
            if (characterSplit.Length > 1)
            {
                string character = characterSplit[0].Trim();
                string[] charactersSplit = character.Split(',');
                string characters = string.Join(",", charactersSplit.Select(c => c.Trim()));
                return characters;
            }
            else
            {
                return string.Empty;
            }
        }

        private static string GetSelectElementsIndex(List<string> row,int choiceIndex)
        {
            string curText = row[choiceIndex].Trim().Replace("\"", "\"\"");
            string curFunctionName = ParseFunctionName(curText);
            string curFunctionType = GetFunctionType(curText);
            bool isCurEndFunction = curFunctionType == FunctionType.End.ToString();
            if (curFunctionName.Contains("Choice") == false || isCurEndFunction)
                return null;
            
            List<int> selectElementsIndexList = new List<int>();
            for (int i = choiceIndex + 1; i < row.Count; i++)
            {
                string text = row[i].Trim().Replace("\"", "\"\"");;
                string functionType = GetFunctionType(text);
                string functionName = ParseFunctionName(text);
                bool isEndFunction = functionType == FunctionType.End.ToString();
                if (functionName.Contains("Choice") && isEndFunction)
                    break;

                bool isStartFunction = functionType == FunctionType.Start.ToString();
                if (functionName.Contains("Select") && isStartFunction)
                {
                    int index = i;
                    selectElementsIndexList.Add(index);
                }
            }

            string selectElementsIndex = string.Join(",", selectElementsIndexList);
            return selectElementsIndex;
        }

        private static string GetVoicePath(string storyCharacters, string text)
        {
            if (string.IsNullOrEmpty(storyCharacters))
                return null;

            string conversation = ParseConversation(text);
            List<string> voicePathList = new List<string>();
            string[] characterList = storyCharacters.Split(",");

            for (int i = 0; i < characterList.Length; i++)
            {
                string name = characterList[i].Trim();
                string directoryName = StorylineEngineSettings.Instance.GetCharacterDirectoryName(name);
                if (string.IsNullOrEmpty(directoryName))
                    continue;

                //불명
                if (directoryName.Contains("_"))
                {
                    var stringArray = directoryName.Split("_");

                    directoryName = stringArray[0];
                }

                if (!VoiceDebugger.voiceList.ContainsKey(directoryName))
                {
                    TextAsset textAsset =
                        AssetDatabase.LoadAssetAtPath<TextAsset>(
                            $"Assets/StorylineEngine/Data/Voice/{directoryName}.txt");

                    if (textAsset == null)
                        return null;

                    VoiceDebugger.voiceList.Add(directoryName, new List<string>());

                    string rawData = textAsset.text;
                    List<string> row = new List<string>();
                    row.AddRange(rawData.Split("\r\n"));
                    row.RemoveAll(s => string.IsNullOrEmpty(s) || s == " ");

                    VoiceDebugger.voiceList[directoryName] = row;
                    VoiceDebugger.debugVoiceList =
                        VoiceDebugger.voiceList.ToDictionary(key => key.Key, value => value.Value.ToList());
                }

                string line = string.Empty;
                string fileName = string.Empty;

                for (int j = 0; j < VoiceDebugger.voiceList[directoryName].Count; j++)
                {
                    var rowArray = VoiceDebugger.voiceList[directoryName][j].Split("/");

                    line = rowArray[2];

                    if (line == conversation)
                    {
                        fileName = $"{directoryName}_{rowArray[0]}";

                        string voicePath = $"Audio/Voice/{directoryName}/{fileName}";

                        var characters = rowArray[1].Split(",");
                        if (characters.Length >= 2)
                        {
                            for (int k = 0; k < characters.Length; k++)
                            {
                                if (characters[k].Trim() == directoryName)
                                {
                                    voicePathList.Add(voicePath);
                                }
                            }
                        }
                        else
                        {
                            voicePathList.Add(voicePath);
                        }

                        if (VoiceDebugger.voiceList[directoryName].Count != 0)
                        {
                            const int ConversationIndex = 2;
                            const string SeperateKey = "/";
                            string voiceLine = VoiceDebugger.voiceList[directoryName]
                                .FirstOrDefault(voice => conversation == voice.Split(SeperateKey)[ConversationIndex]);

                            string fullVoicePath = StoryUtil.CombinePath
                                ("Assets", nameof(Lucecita.StorylineEngine), nameof(Resources), voicePath);
                            bool existVoiceFile = StoryUtil.ExistVoiceFile(fullVoicePath);
                            if (existVoiceFile)
                            {
                                VoiceDebugger.debugVoiceList[directoryName].Remove(voiceLine);
                            }

                            VoiceDebugger.voiceList[directoryName].Remove(voiceLine);
                        }

                        break;
                    }
                }
            }

            string voicePaths = string.Join(",", voicePathList);
            return voicePaths;
        }
        
        private static List<TextAsset> GetCSV(List<string> csvPath)
        {
            List<TextAsset> csvTextAssets = new List<TextAsset>();
            foreach (string path in csvPath)
            {
                TextAsset csvTextAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
                csvTextAssets.Add(csvTextAsset);
            }

            return csvTextAssets;
        }
    }
}
#endif