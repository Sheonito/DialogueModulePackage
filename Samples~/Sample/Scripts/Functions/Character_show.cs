using System.Collections.Generic;
using Aftertime.HappinessBlossom.Define;
using Aftertime.HappinessBlossom.Presenter;
using Aftertime.HappinessBlossom.Stage;
using Aftertime.StorylineEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Aftertime.HappinessBlossom
{
    public class Character_show : TypedFunction<CharacterShowValue>, IElementChangeFunc
    {
        #region Static Readonly Variables

        public static readonly float CharacterWidth = 5;
        public static readonly float CharacterPosY = -5;
        public static readonly float Spacing = 1;

        #endregion
        public static Vector3 DefaultCharacterPos { get; private set; }

        [Presenter] private StagePresenter Presenter;
        
        public override void Init()
        {
            base.Init();
            CharacterStage characterStage = Presenter.view.CharacterStage;
            StageCharacter inActiveCharacter = characterStage.GetInActiveCharacter();
            DefaultCharacterPos = inActiveCharacter.transform.position;
        }

        public override async UniTask StartFunction()
        {
            FunctionValue functionValue = TextElement.FunctionValue;
            string characterName = functionValue.Get<string>(CharacterShowValue.CharacterName);

            float duration = StageCharacter.DefaultMoveDuration;
            bool hasDuration = functionValue.HasValue(CharacterShowValue.Duration);
            if (hasDuration)
                duration = TextElement.FunctionValue.Get<float>(CharacterShowValue.Duration);

            float x = DefaultCharacterPos.x;
            bool hasX = functionValue.HasValue(CharacterShowValue.X);
            if (hasX)
                x = TextElement.FunctionValue.Get<float>(CharacterShowValue.X);
            
            float y = DefaultCharacterPos.y;
            bool hasY = functionValue.HasValue(CharacterShowValue.Y);
            if (hasY)
                y = TextElement.FunctionValue.Get<float>(CharacterShowValue.Y);

            float z = DefaultCharacterPos.z;
            bool hasZ = functionValue.HasValue(CharacterShowValue.Z);
            if (hasZ)
                z = TextElement.FunctionValue.Get<float>(CharacterShowValue.Z);
            
            Vector3 customPos = new Vector3(x, y, z);

            ShowCharacter(characterName,customPos,duration).Forget();
        }

        public async UniTask<StageCharacter> ShowCharacter(string characterName,Vector3 customPos = default,float duration = StageCharacter.DefaultMoveDuration)
        {
            string spritePath = ResourcesPath.CharacterPrefix + characterName;
            Sprite sprite = Resources.Load<Sprite>(spritePath);

            CharacterStage characterStage = Presenter.view.CharacterStage;
            StageCharacter stageCharacter = characterStage.GetInActiveCharacter();
            if (stageCharacter == null)
                return null;

            stageCharacter.ResetCharacter();
            
            stageCharacter.SpriteRenderer.DOKill();
            stageCharacter.SpriteRenderer.enabled = true;
            stageCharacter.SpriteRenderer.sprite = sprite;
            stageCharacter.SpriteRenderer.color = new Color(1, 1, 1, 0);
            stageCharacter.isOn = true;
            await UpdateCharacterPos(customPos,duration).AttachExternalCancellation(cts.Token);

            return stageCharacter;
        }

        public async UniTask UpdateCharacterPos(Vector3 customPos = default,float duration = StageCharacter.DefaultMoveDuration)
        {
            CharacterStage characterStage = Presenter.view.CharacterStage;
            List<StageCharacter> activeCharacters = characterStage.GetActiveCharacterList();
            int activeCharCount = activeCharacters.Count;
            float startPosX;
            float totalWidth = activeCharCount * CharacterWidth;

            // Spacing 만큼 값을 더해준다
            float totalSpacing = Spacing * (activeCharCount - 1);
            startPosX = -((totalWidth + totalSpacing) / 2) + (CharacterWidth / 2);

            Vector3 pos = new Vector3(startPosX, CharacterPosY,0);
            for (int i = 0; i < activeCharCount; i++)
            {
                pos = customPos == default ? pos : customPos;
                activeCharacters[i].Move(pos,duration);

                pos.x += CharacterWidth + Spacing;
            }

            int moveDelay = (int)(duration * 1000);
            await UniTask.Delay(moveDelay).AttachExternalCancellation(cts.Token);
        }

        public TextElement GetNextElement()
        {
            int curElementIndex = TextElement.Index;
            TextElement nextTextElement = EpisodeData.GetNextTextElement(curElementIndex);
            return nextTextElement;
        }
    }

    public enum CharacterShowValue
    {
        CharacterName,
        Duration,
        X,
        Y,
        Z
    }
}
