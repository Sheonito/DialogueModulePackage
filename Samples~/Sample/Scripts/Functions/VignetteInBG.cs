using Lucecita.HappinessBlossom.Define;
using Lucecita.HappinessBlossom.Directing;
using Lucecita.HappinessBlossom.Presenter;
using Lucecita.StorylineEngine;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Lucecita.SecretSome
{
    public class VignetteInBG : TypedFunction<VignetteInBGValue>,IElementChangeFunc
    {
        [Presenter] private StagePresenter Presenter; 
        
        public override async UniTask StartFunction()
        {
            string spriteName = TextElement.FunctionValue.Get<string>(VignetteInBGValue.SpriteName);
            
            Sprite sprite = Resources.Load<Sprite>(ResourcesPath.BGPrefix + spriteName);
            
            float duration = TextElement.FunctionValue.Get<float>(VignetteInBGValue.Duration);
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
            DirectingManager.SpriteRendererDirector.VignetteIn(originSr,sprite,Color.white, duration,true);
        }

        public TextElement GetNextElement()
        {
            int curElementIndex = TextElement.Index;
            TextElement nextTextElement = EpisodeData.GetNextTextElement(curElementIndex);
            return nextTextElement;
        }
    }

    public enum VignetteInBGValue
    {
        SpriteName,
        Duration,
    }

}
