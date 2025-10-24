using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Aftertime.StorylineEngine
{
    [Serializable]
    public struct CharacterDirectoryName
    {
        public string storyName;
        public string directoryName;
    }

    [Serializable]
    public class StorylineEngineSettings : ScriptableObject
    {
        public static StorylineEngineSettings Instance
        {
            get
            {
                if (_instance == null)
                    _instance = Resources.Load<StorylineEngineSettings>(FilePath);

                if (_instance == null)
                {
                    _instance = CreateStorylineEngineSetting();
                }

                return _instance;
            }
        }

        private static StorylineEngineSettings _instance;

        public CharacterSettings CharacterSettings => _characterSettings;
        [SerializeField] private CharacterSettings _characterSettings;

        [SerializeField] private List<string> _conversationFunctionTypes;
        [SerializeField] private List<CharacterDirectoryName> _characterDirectoryNames;

        private static readonly string DirectoryPath = $"{StoryUtil.Relative_EnginePath}/Resources/Setting";
        private static readonly string FilePath = "Setting/StorylineEngineSettings";

        public string GetCharacterDirectoryName(string storyName)
        {
            CharacterDirectoryName directoryName =
                _characterDirectoryNames.FirstOrDefault(directoryName => directoryName.storyName == storyName);

            return directoryName.directoryName;
        }

        public bool IsConversationFunction(string functionName)
        {
            if (_conversationFunctionTypes == null || _conversationFunctionTypes.Count == 0)
                return false;

            bool isConversationFunction = _conversationFunctionTypes.Exists(type => type == functionName);
            return isConversationFunction;
        }

        public static StorylineEngineSettings CreateStorylineEngineSetting()
        {
#if UNITY_EDITOR
            StorylineEngineSettings storylineEngineSetting = CreateInstance<StorylineEngineSettings>();
            AssetDatabase.CreateAsset(storylineEngineSetting, GetAssetFilePath());
            ProjectWindowUtil.ShowCreatedAsset(storylineEngineSetting);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return storylineEngineSetting;

#else
return null;
#endif
        }

        private static string GetAssetFilePath()
        {
            if (!Directory.Exists(DirectoryPath))
                Directory.CreateDirectory(DirectoryPath);

            return $"{DirectoryPath}/StorylineEngineSettings.asset";
        }
    }
}