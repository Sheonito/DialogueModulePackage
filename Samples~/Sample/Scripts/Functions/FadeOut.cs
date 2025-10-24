using Aftertime.HappinessBlossom.Directing;
using Aftertime.StorylineEngine;
using Cysharp.Threading.Tasks;

namespace Aftertime.HappinessBlossom
{
    public enum FadeOutValue { Duration }
    public class FadeOut : TypedFunction<FadeOutValue>, IElementChangeFunc
    {
        public override async UniTask StartFunction()
        {
            float duration = TextElement.FunctionValue.Get<float>(FadeOutValue.Duration);
            FadeOutImage(duration);
        }

        private void FadeOutImage(float duration)
        {
            DirectingManager.TransitionDirector.FadeOut(duration, TransitionRenderMode.FullScreen);
        }

        public TextElement GetNextElement()
        {
            int curElementIndex = TextElement.Index;
            TextElement nextTextElement = EpisodeData.GetNextTextElement(curElementIndex);
            return nextTextElement;
        }
    }
}
