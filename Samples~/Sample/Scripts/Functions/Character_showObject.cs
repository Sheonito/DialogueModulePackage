using Lucecita.HappinessBlossom.Define;
using Lucecita.HappinessBlossom.Presenter;
using Lucecita.HappinessBlossom.Stage;
using Lucecita.StorylineEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Lucecita.HappinessBlossom
{
    public class Character_showObject : TypedFunction<CharacterShowObjectValue>,IElementChangeFunc
    {
        [Presenter] private StagePresenter Presenter;
        public const float DefaultShowDuration = 0.3f;
        
        public override async UniTask StartFunction()
        {
            string characterName = TextElement.FunctionValue.Get<string>(CharacterShowObjectValue.CharacterName);
            string objectName = TextElement.FunctionValue.Get<string>(CharacterShowObjectValue.ObjectName);
            
            float duration = TextElement.FunctionValue.Get<float>(CharacterShowObjectValue.Duration);
            bool hasDuration = duration != -1;
            if (!hasDuration)
                duration = DefaultShowDuration;
            
            ShowObject(characterName,objectName,duration);
        }

        private void ShowObject(string characterName,string objectName,float duration = DefaultShowDuration)
        {
            string spritePath = ResourcesPath.FacePrefix + objectName;
            Sprite sprite = Resources.Load<Sprite>(spritePath);

            CharacterStage characterStage = Presenter.view.CharacterStage;
            StageCharacter stageCharacter = characterStage.GetCharacterByCharacterName(characterName);
            if (stageCharacter == null)
                return;

            SpriteRenderer spriteRenderer = stageCharacter.ObjectSpriteRenderer;
            spriteRenderer.DOKill();
            spriteRenderer.gameObject.SetActive(true);
            spriteRenderer.enabled = true;
            spriteRenderer.sprite = sprite;
            spriteRenderer.color = new Color(1, 1, 1, 0);
            spriteRenderer.DOFade(1, duration);
        }

        public TextElement GetNextElement()
        {
            int curElementIndex = TextElement.Index;
            TextElement nextTextElement = EpisodeData.GetNextTextElement(curElementIndex);
            return nextTextElement;
        }
    }
    
    public enum CharacterShowObjectValue
    {
        CharacterName,
        ObjectName,
        Duration,
    }

}
