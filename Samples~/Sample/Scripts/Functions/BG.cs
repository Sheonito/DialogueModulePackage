using System;
using Aftertime.HappinessBlossom.Define;
using Aftertime.HappinessBlossom.Presenter;
using Aftertime.StorylineEngine;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Aftertime.HappinessBlossom
{
    public class BG : TypedFunction<BGFunctionValue>,IElementChangeFunc
    {
        [Presenter] private StagePresenter _presenter;
        public override async UniTask StartFunction()
        {
            string bgName = TextElement.FunctionValue.Get<string>(BGFunctionValue.BGName);
            ShowBG(bgName);
        }

        private void ShowBG(string bgName)
        {
            Sprite bgSprite = Resources.Load<Sprite>(ResourcesPath.BGPrefix + bgName);
            SpriteRenderer bgImage = _presenter.view.Bg.SpriteRenderer;
            bgImage.color = Color.white;
            bgImage.sprite = bgSprite;
            SpriteRendererUtil.CalculateScale(bgImage);
        }

        public TextElement GetNextElement()
        {
            int curElementIndex = TextElement.Index;
            TextElement nextTextElement = EpisodeData.GetNextTextElement(curElementIndex);
            return nextTextElement;
        }
    }
    
    public enum BGFunctionValue
    {
        BGName
    }
}
