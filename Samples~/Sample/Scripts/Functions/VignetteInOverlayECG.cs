using Aftertime.HappinessBlossom.Define;
using Aftertime.HappinessBlossom.Directing;
using Aftertime.HappinessBlossom.Presenter;
using Aftertime.StorylineEngine;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Aftertime.SecretSome
{
    public class VignetteInOverlayECG : TypedFunction<VignetteInOverlayECGValue>,IElementChangeFunc
    {
        [Presenter] private StagePresenter Presenter;
        private const float DefaultDuration = 1.5f;
        
        public override async UniTask StartFunction()
        {
            string spriteName = TextElement.FunctionValue.Get<string>(VignetteInOverlayECGValue.SpriteName);
            Sprite sprite = Resources.Load<Sprite>(ResourcesPath.ECGPrefix + spriteName);
            float duration = TextElement.FunctionValue.Get<float>(VignetteInOverlayECGValue.Duration);
            bool hasDuration = duration != -1;
            if (!hasDuration)
            {
                duration = DefaultDuration;
            }
            
            DoVignette(sprite,duration);
        }

        private void DoVignette(Sprite sprite,float duration)
        {
            SpriteRenderer originSr = Presenter.view.Ecg.SpriteRenderer;
            SpriteRenderer overlaySr = Presenter.view.Ecg.OverlaySpriteRenderer;
            overlaySr.sprite = sprite;
            
            DirectingManager.SpriteRendererDirector.VignetteInOverlay(originSr,overlaySr,sprite,Color.white, duration,true);
        }

        public TextElement GetNextElement()
        {
            int curElementIndex = TextElement.Index;
            TextElement nextTextElement = EpisodeData.GetNextTextElement(curElementIndex);
            return nextTextElement;
        }
    }

    public enum VignetteInOverlayECGValue
    {
        SpriteName,
        Duration,
    }

}
