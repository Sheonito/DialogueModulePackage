using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace Lucecita.StorylineEngine.Editor
{
    public class StorylineEnginePostProcessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetPaths)
        {
            ImportSession(importedAssets);
            
        }

        private static void ImportSession(string[] importedAssets)
        {
            if (importedAssets.Length == 0)
                return;
            
            string packageJsonPath = "Packages/com.lucecita.dialoguemodule/package.json";
            bool isPackageLoaded = IsPackageLoaded(importedAssets,packageJsonPath);
            if (isPackageLoaded)
            {
                SaveVersion(packageJsonPath);
                CreateDirectories();
            }
        }

        private static bool IsPackageLoaded(string[] importedAssets,string packageJsonPath)
        {
            bool isPackageLoaded = importedAssets
                .ToList()
                .Exists(asset => asset.Contains(packageJsonPath));

            return isPackageLoaded;
        }

        private static void SaveVersion(string packageJsonPath)
        {
            TextAsset packageJson = AssetDatabase.LoadAssetAtPath<TextAsset>(packageJsonPath);
            if (packageJson == null)
                return;

            JObject jObject = JObject.Parse(packageJson.ToString());

            if (jObject != null)
            {
                string version = jObject["version"].ToString();
                PlayerPrefs.SetString(EditorDefine.VERSION_KEY, version);
            }
        }

        private static void CreateDirectories()
        {
            string dataPath = $"{StoryUtil.Absolute_EnginePath}/Data";
            if (Directory.Exists(dataPath))
                return;

            Directory.CreateDirectory(dataPath);
            Directory.CreateDirectory($"{dataPath}/EpisodeCSV");
            Directory.CreateDirectory($"{dataPath}/EpisodeSO");
            Directory.CreateDirectory($"{dataPath}/Resources");
            Directory.CreateDirectory($"{dataPath}/Setting");
            StorylineEngineSettings.CreateStorylineEngineSetting();

            AssetDatabase.Refresh();
        }
    }
}