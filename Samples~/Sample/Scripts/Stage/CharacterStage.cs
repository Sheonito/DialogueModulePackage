using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lucecita.HappinessBlossom.Stage
{
    public class CharacterStage : MonoBehaviour
    {
        public List<StageCharacter> Characters => _characters;

        [SerializeField] private List<StageCharacter> _characters;


        public StageCharacter GetCharacterBySpriteName(string spriteName)
        {
            StageCharacter character = _characters.FirstOrDefault(character => character.GetSpriteName() == spriteName);

            return character;
        }

        public StageCharacter GetCharacterByCharacterName(string characterName)
        {
            if (string.IsNullOrEmpty(characterName))
                return null;

            StageCharacter targetCharacter = null;
            foreach (StageCharacter character in _characters)
            {
                if (character.GetSpriteName() == null)
                    continue;

                else if (character.GetSpriteName().Contains(characterName))
                {
                    targetCharacter = character;
                }
            }

            return targetCharacter;
        }

        public StageCharacter GetInActiveCharacter()
        {
            StageCharacter character = _characters.FirstOrDefault(character => character.isOn == false);
            return character;
        }

        public List<StageCharacter> GetActiveCharacterList()
        {
            List<StageCharacter> characterList =
                _characters.Where(character => character.isOn).ToList();
            return characterList;
        }

        public async void FocusCharacter(string characterName)
        {
            StageCharacter focusCharacter = GetCharacterByCharacterName(characterName);
            if (focusCharacter == null)
                return;

            focusCharacter.Focus();

            List<StageCharacter> activeCharacters = GetActiveCharacterList();
            foreach (StageCharacter activeCharacter in activeCharacters)
            {
                if (activeCharacter == focusCharacter)
                    continue;

                activeCharacter.UnFocus();
            }
        }
    }
}