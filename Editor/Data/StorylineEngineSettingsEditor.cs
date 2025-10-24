using UnityEditor;

namespace Aftertime.StorylineEngine
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