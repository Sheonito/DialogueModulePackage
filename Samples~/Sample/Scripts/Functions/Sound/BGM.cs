using System;
using Lucecita.StorylineEngine;
using Cysharp.Threading.Tasks;

namespace Lucecita.HappinessBlossom
{
    public class BGM : TypedFunction<BGMValue>, IElementChangeFunc
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
            string clipName = TextElement.FunctionValue.Get<string>(BGMValue.ClipName);
            float duration = 0f;
            if (TextElement.FunctionValue.Length > 1)
                duration = TextElement.FunctionValue.Get<float>(BGMValue.Duration);

            SoundManager.Instance.PlayBGM(clipName, duration);
        }

        private void Stop()
        {
            float duration = 0f;
            if (TextElement.FunctionValue.Length > 0)
                duration = TextElement.FunctionValue.Get<float>(BGMValue.Duration);

            SoundManager.Instance.StopAllBGM(duration);
        }

        public TextElement GetNextElement()
        {
            int curElementIndex = TextElement.Index;
            TextElement nextTextElement = EpisodeData.GetNextTextElement(curElementIndex);
            return nextTextElement;
        }
    }
    
    public enum BGMValue { ClipName, Duration }
}
