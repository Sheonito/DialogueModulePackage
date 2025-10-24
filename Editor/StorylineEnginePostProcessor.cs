using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace Aftertime.StorylineEngine.Editor
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
            
            string packageJsonPath = "Packages/com.aftertime.storylineengine/package.json";
            bool isPackageLoaded = IsPackageLoaded(importedAssets,packageJsonPath);
            if (isPackageLoaded)
            {
                SaveVersion(packageJsonPath);
                
                // 현재는 샘플 다운로드만 있음.
                // 하지만 샘플은 내부로 변경해서 아무 기능이 없음.
                // 모듈 다운로더로 변경 후 주석 해제 필요
                // ModuleDownloader.Window();
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
    }
}