using System;
using Aftertime.StorylineEngine;
using Cysharp.Threading.Tasks;

namespace Aftertime.HappinessBlossom
{
    public enum SFXFunction { ClipName, Duration }
    public class SFX : TypedFunction<SFXFunction>, IElementChangeFunc
    {

        public override async UniTask StartFunction()
        {
            Play();
        }

        public override async UniTask EndFunction()
        {
            Stop();
        }
        
        private void Play()
        {
            string clipName = TextElement.FunctionValue.Get<string>(SFXFunction.ClipName);
            float duration = 0f;
            if (TextElement.FunctionValue.Length > 1)
                duration = TextElement.FunctionValue.Get<float>(SFXFunction.Duration);
            
            SoundManager.Instance.PlaySFX(clipName, duration);
        }

        private void Stop()
        {
            float duration = 0f;
            if (TextElement.FunctionValue.Length > 0)
                duration = TextElement.FunctionValue.Get<float>(SFXFunction.Duration);
            
            SoundManager.Instance.StopSFX(duration);
        }

        public TextElement GetNextElement()
        {
            int curElementIndex = TextElement.Index;
            TextElement nextTextElement = EpisodeData.GetNextTextElement(curElementIndex);
            return nextTextElement;
        }
    }
}
