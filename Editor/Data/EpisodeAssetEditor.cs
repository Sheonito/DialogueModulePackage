#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Lucecita.StorylineEngine
{
    public class EpisodeAssetEditor
    {
        [MenuItem("Storyline Engine/Import All Episodes")]
        public static void ImportAllEpisodeData()
        {
            CreateDirectories();
            
            TextAsset[] csvFiles = EpisodeCSVCreator.Create();
            EpisodeDataCreator.Create(csvFiles);

            VoiceDebugger.ShowMissingDataLog();
            
            Debug.Log("에피소드 데이터가 성공적으로 가져왔습니다.");
        }
        
        private static void CreateDirectories()
        {
            string dataPath = $"{StoryUtil.Absolute_EnginePath}/Data";
            if (Directory.Exists(dataPath))
                return;

            Directory.CreateDirectory(dataPath);
            Directory.CreateDirectory($"{dataPath}/EpisodeCSV");
            Directory.CreateDirectory($"{dataPath}/EpisodeSO");

            AssetDatabase.Refresh();
        }
    }
}
#endif