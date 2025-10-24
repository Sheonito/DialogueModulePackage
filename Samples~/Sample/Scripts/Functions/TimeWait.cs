using System;
using Lucecita.StorylineEngine;
using Cysharp.Threading.Tasks;

namespace Lucecita.HappinessBlossom
{
    public class TimeWait : TypedFunction<TimeWaitValue>, IElementChangeFunc, IStopFunc
    {
        public event Action onStop;
        public event Action onResume;

        public override async UniTask StartFunction()
        {
            onStop?.Invoke();

            FunctionValue functionValue = TextElement.FunctionValue;
            float delayTime = functionValue.Get<float>(TimeWaitValue.Duration);

            await UniTask.WaitForSeconds(delayTime).AttachExternalCancellation(cts.Token);
            if (cts.IsCancellationRequested == false)
                onResume?.Invoke();
        }

        public TextElement GetNextElement()
        {
            int curElementIndex = TextElement.Index;
            TextElement nextTextElement = EpisodeData.GetNextTextElement(curElementIndex);
            return nextTextElement;
        }
    }

    public enum TimeWaitValue
    {
        Duration
    }
}