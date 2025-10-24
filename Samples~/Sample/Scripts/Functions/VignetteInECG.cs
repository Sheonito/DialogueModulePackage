using Aftertime.HappinessBlossom.Define;
using Aftertime.HappinessBlossom.Directing;
using Aftertime.HappinessBlossom.Presenter;
using Aftertime.StorylineEngine;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Aftertime.SecretSome
{
    public class VignetteInECG : TypedFunction<VignetteInECGValue>, IElementChangeFunc
    {
        [Presenter] private StagePresenter Presenter; 
        private const float DefaultDuration = 1.5f;
        
        public override async UniTask StartFunction()
        {
            FunctionValue functionValue = TextElement.FunctionValue;
            string spriteName = functionValue.Get<string>(VignetteInECGValue.SpriteName);
            Sprite sprite = Resources.Load<Sprite>(ResourcesPath.ECGPrefix + spriteName);
            float duration = DefaultDuration;
            bool hasDuration = functionValue.HasValue(VignetteInECGValue.Duration);
            if (hasDuration)
            {
                duration = functionValue.Get<float>(VignetteInECGValue.Duration);
            }
            
            DoVignette(sprite, duration);
        }

        private void DoVignette(Sprite sprite, float duration)
        {
            SpriteRenderer ecgSr = Presenter.view.Ecg.SpriteRenderer;
            ecgSr.sprite = sprite;
            DirectingManager.SpriteRendererDirector.VignetteIn(ecgSr, sprite, Color.white, duration,true);
        }

        public TextElement GetNextElement()
        {
            int curElementIndex = TextElement.Index;
            TextElement nextTextElement = EpisodeData.GetNextTextElement(curElementIndex);
            return nextTextElement;
        }
    }

    public enum VignetteInECGValue
    {
        SpriteName,
        Duration,
    }
}
