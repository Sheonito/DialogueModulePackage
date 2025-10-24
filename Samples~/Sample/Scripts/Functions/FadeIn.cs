using System;
using Lucecita.HappinessBlossom.Directing;
using Lucecita.StorylineEngine;
using Cysharp.Threading.Tasks;

namespace Lucecita.HappinessBlossom
{
    public enum FadeInValue { Duration }
    public class FadeIn : TypedFunction<FadeInValue>, IElementChangeFunc
    {
        public override async UniTask StartFunction()
        {
            float duration = TextElement.FunctionValue.Get<float>(FadeInValue.Duration);
            FadeInImage(duration);
            await UniTask.Delay(300);
        }

        private void FadeInImage(float duration)
        {
            DirectingManager.TransitionDirector.FadeIn(duration, TransitionRenderMode.FullScreen);
        }

        public TextElement GetNextElement()
        {
            int curElementIndex = TextElement.Index;
            TextElement nextTextElement = EpisodeData.GetNextTextElement(curElementIndex);
            return nextTextElement;
        }
    }
}
