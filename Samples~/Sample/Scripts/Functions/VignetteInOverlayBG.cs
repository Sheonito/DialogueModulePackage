using Aftertime.HappinessBlossom.Define;
using Aftertime.HappinessBlossom.Directing;
using Aftertime.HappinessBlossom.Presenter;
using Aftertime.StorylineEngine;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Aftertime.SecretSome
{
    public class VignetteInOverlayBG : TypedFunction<VignetteInOverlayBGValue>,IElementChangeFunc
    {
        [Presenter] private StagePresenter Presenter; 
        
        public override async UniTask StartFunction()
        {
            string spriteName = TextElement.FunctionValue.Get<string>(VignetteInOverlayBGValue.SpriteName);
            
            Sprite sprite = Resources.Load<Sprite>(ResourcesPath.BGPrefix + spriteName);
            
            float duration = TextElement.FunctionValue.Get<float>(VignetteInOverlayBGValue.Duration);
            bool hasDuration = duration != -1;
            if (!hasDuration)
            {
                duration = 1.5f;
            }
            
            DoVignette(sprite,duration);
        }

        private void DoVignette(Sprite sprite,float duration)
        {
            SpriteRenderer originSr = Presenter.view.Bg.SpriteRenderer;
            SpriteRenderer overlaySr = Presenter.view.Bg.OverlaySpriteRenderer;

            DirectingManager.SpriteRendererDirector.VignetteInOverlay(originSr,overlaySr,sprite,Color.white, duration,true);
        }

        public TextElement GetNextElement()
        {
            int curElementIndex = TextElement.Index;
            TextElement nextTextElement = EpisodeData.GetNextTextElement(curElementIndex);
            return nextTextElement;
        }
    }

    public enum VignetteInOverlayBGValue
    {
        SpriteName,
        Duration
    }

}
