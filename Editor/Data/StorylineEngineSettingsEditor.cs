using UnityEditor;

namespace Lucecita.StorylineEngine
{
    public class StorylineEngineSettingsEditor
    {
        [MenuItem("Storyline Engine/Create/StorylineEngineSettings")]
        public static void CreateSettings()
        {
            StorylineEngineSettings.CreateStorylineEngineSetting();
        }
    }
   
}