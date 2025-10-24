using Aftertime.HappinessBlossom.Define;
using Aftertime.HappinessBlossom.Directing;
using Aftertime.HappinessBlossom.Presenter;
using Aftertime.HappinessBlossom.Stage;
using Aftertime.StorylineEngine;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Aftertime.HappinessBlossom
{
    public class Character_overlap : TypedFunction<CharacterOverlapValues>, IElementChangeFunc
    {
        [Presenter] private StagePresenter Presenter;
        
        private const float DefaultChangeDuration = 1f;

        private Character_show _characterShow;
        
        public override void Init()
        {
            base.Init();
            _characterShow = new Character_show();
            _characterShow.Init();
        }

        public override async UniTask StartFunction()
        {
            ChangeCharacter();
        }

        private void ChangeCharacter()
        {
            string characterName = TextElement.FunctionValue.Get<string>(CharacterOverlapValues.CharacterName);
            string changeCharacterName = TextElement.FunctionValue.Get<string>(CharacterOverlapValues.ChangeCharacterName);
            float changeDuration = TextElement.FunctionValue.Get<float>(CharacterOverlapValues.ChangeDuration);
            if (changeDuration == -1)
            {
                changeDuration = DefaultChangeDuration;
            }

            CharacterStage characterStage = Presenter.view.CharacterStage;
            StageCharacter characterUI = characterStage.GetCharacterByCharacterName(characterName);

            string newSpritePath = ResourcesPath.CharacterPrefix + changeCharacterName;
            Sprite newSprite = Resources.Load<Sprite>(newSpritePath);
            if (characterUI != null)
            {
                SpriteRenderer characterSr = characterUI.SpriteRenderer;
                SpriteRenderer overlapSr = characterUI.OverlaySpriteRenderer;
                
                SpriteRendererDirector srDirector = DirectingManager.SpriteRendererDirector;
                srDirector.StopImageAllDirecting(characterSr);
                srDirector.StopImageAllDirecting(overlapSr);
                srDirector.Overlap(characterSr,overlapSr,newSprite,changeDuration,false);
            }
            else
            {
                ShowNewCharacter(newSprite);
            }
        }

        private void ShowNewCharacter(Sprite newSprite)
        {
            _characterShow.ShowCharacter(newSprite.name,Character_show.DefaultCharacterPos);
        }

        public TextElement GetNextElement()
        {
            int curElementIndex = TextElement.Index;
            TextElement nextTextElement = EpisodeData.GetNextTextElement(curElementIndex);
            return nextTextElement;
        }
    }

    public enum CharacterOverlapValues
    {
        CharacterName,
        ChangeCharacterName,
        ChangeDuration,
    }
}
