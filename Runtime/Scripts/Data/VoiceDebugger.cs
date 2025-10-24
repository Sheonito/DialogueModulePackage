using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aftertime.StorylineEngine
{
    public static class VoiceDebugger
    {
        public static Dictionary<string, List<string>> voiceList = new Dictionary<string, List<string>>();
        public static Dictionary<string, List<string>> debugVoiceList = new Dictionary<string, List<string>>();
    
        public static void ShowMissingDataLog()
        {
            foreach (var item in debugVoiceList)
            {
                foreach (var value in item.Value)
                {
                    Debug.Log($"[Voice] {item.Key} // {value}");
                }
            }
        }

    }   
}