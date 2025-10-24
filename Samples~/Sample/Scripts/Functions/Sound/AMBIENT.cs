using System;
using Aftertime.StorylineEngine;
using Cysharp.Threading.Tasks;

namespace Aftertime.HappinessBlossom
{
    public class AMBIENT : TypedFunction<AMBIENTValue>, IElementChangeFunc
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
            string clipName = TextElement.FunctionValue.Get<string>(AMBIENTValue.ClipName);
            float duration = 0f;
            if (TextElement.FunctionValue.Length > 1)
                duration = TextElement.FunctionValue.Get<float>(AMBIENTValue.Duration);
            
            SoundManager.Instance.PlayAmbient(clipName, duration);
        }

        private void Stop()
        {
            float duration = 0f;
            if (TextElement.FunctionValue.Length > 0)
                duration = TextElement.FunctionValue.Get<float>(AMBIENTValue.Duration);
            
            SoundManager.Instance.StopAmbient(duration);
        }

        public TextElement GetNextElement()
        {
            int curElementIndex = TextElement.Index;
            TextElement nextTextElement = EpisodeData.GetNextTextElement(curElementIndex);
            return nextTextElement;
        }
    }
    
    public enum AMBIENTValue { ClipName, Duration }
}
