using System;
using Lucecita.HappinessBlossom.Presenter;
using Lucecita.HappinessBlossom.Stage;
using Lucecita.StorylineEngine;
using Cysharp.Threading.Tasks;

namespace Lucecita.HappinessBlossom
{
    public class Character_hide : TypedFunction<CharacterHideValue>,IElementChangeFunc
    {
        [Presenter] private StagePresenter Presenter;
        
        public override async UniTask StartFunction()
        {
            string characterName = TextElement.FunctionValue.Get<string>(CharacterHideValue.CharacterName);

            HideCharacter(characterName);
        }

        private void HideCharacter(string characterName)
        {
            CharacterStage characterStage = Presenter.view.CharacterStage;
            StageCharacter characterUI = characterStage.GetCharacterByCharacterName(characterName);

            float fadeDuration = TextElement.FunctionValue.Get<float>(CharacterHideValue.Duration);
            bool hasDuration = fadeDuration != -1;
            if (hasDuration)
            {
                characterUI.Hide(fadeDuration);
            }
            else
            {
                characterUI.Hide();
            }
        }

        public TextElement GetNextElement()
        {
            int curElementIndex = TextElement.Index;
            TextElement nextTextElement = EpisodeData.GetNextTextElement(curElementIndex);
            return nextTextElement;
        }
    }

    public enum CharacterHideValue
    {
        CharacterName,
        Duration
    }
}
