#if UNITY_EDITOR
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using UnityEditor;

using UnityEngine;

namespace Lucecita.StorylineEngine
{
    public static class EpisodeDataCreator
    {
        public static void Create(TextAsset[] csvFiles)
        {
            CreateEpisodes(csvFiles);
            
            string episodePath = GetEpisodeDirectoryPath();
            Object episodeDirectory = AssetDatabase.LoadAssetAtPath<Object>(episodePath);
            ProjectWindowUtil.ShowCreatedAsset(episodeDirectory);
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void CreateEpisodes(TextAsset[] csvFiles)
        {
            CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                IncludePrivateMembers = true,
            };

            foreach (TextAsset csvFile in csvFiles)
            {
                string csvText = csvFile.text;

                using StringReader reader = new StringReader(csvText);
                using CsvReader csv = new CsvReader(reader, config);
                csv.Context.RegisterClassMap<TextElementMap>();
                List<TextElement> textElements = new List<TextElement>(csv.GetRecords<TextElement>());

                string episodeName = csvFile.name;
                EpisodeData episodeData = Get(episodeName);
                bool hasSaveData = episodeData != null;
                if(!hasSaveData)
                    episodeData = Create(episodeName);
                
                episodeData.UpdateData(textElements);
            }
        }
        
        private static EpisodeData Get(string episodeName)
        {
            string directory = GetEpisodeDirectoryPath();
            string path = PathUtil.Combine(directory, episodeName + ".asset");
            EpisodeData episodeData = AssetDatabase.LoadAssetAtPath<EpisodeData>(path);
            
            return episodeData;
        }

        private static EpisodeData Create(string episodeName)
        {
            EpisodeData episodeData = ScriptableObject.CreateInstance<EpisodeData>();
            string directory = GetEpisodeDirectoryPath();
            string path = PathUtil.Combine(directory, episodeName + ".asset");
            AssetDatabase.CreateAsset(episodeData, path);

            return episodeData;
        }

        private static string GetEpisodeDirectoryPath()
        {
            string directory = $"{StoryUtil.Relative_EnginePath}/Data/EpisodeSO";

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            
            return directory;
        }
    }
}
#endif