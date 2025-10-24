using UnityEngine;

namespace Lucecita.StorylineEngine
{
    [System.Serializable]
    public class CharacterSettings
    {
        public string UserNameKey => _userNameKey;
        public string AnonymousNameKey => _anonymousNameKey;
        public string AnonymousName => _anonymousName;

        [SerializeField] private string _userNameKey = "Player";
        [SerializeField] private string _anonymousNameKey = "_";
        [SerializeField] private string _anonymousName = "???";
    }
   
}