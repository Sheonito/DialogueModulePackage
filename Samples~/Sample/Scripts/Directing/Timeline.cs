using System;
using Aftertime.HappinessBlossom.Directing;
using Aftertime.StorylineEngine;
using Cysharp.Threading.Tasks;

namespace Aftertime.HappinessBlossom
{
    public class Timeline : TypedFunction<TimelineValue>, IElementChangeFunc
    {
        public override async UniTask StartFunction()
        {
            await Play();
        }

        private async UniTask Play()
        {
            TimelineDirector timelineDirector = DirectingManager.TimelineDirector;
            bool isPlaying = timelineDirector.IsPlaying();
            if (isPlaying)
            {
                timelineDirector.CompleteCurrentTimeline();
            }

            string playableAssetKey = TextElement.FunctionValue.Get<string>(TimelineValue.TimelineName);

            bool isWait = false;
            bool hasWaitVariable = TextElement.FunctionValue.HasValue(TimelineValue.IsWait);
            if (hasWaitVariable)
                isWait = TextElement.FunctionValue.Get<bool>(TimelineValue.IsWait);

            if (isWait)
            {
                await timelineDirector.PlayAsync(playableAssetKey)
                    .AttachExternalCancellation(cts.Token);
            }
            else
            {
                timelineDirector.Play(playableAssetKey);
            }
        }

        public TextElement GetNextElement()
        {
            int curElementIndex = TextElement.Index;
            TextElement nextTextElement = EpisodeData.GetNextTextElement(curElementIndex);
            return nextTextElement;
        }
    }

    public enum TimelineValue
    {
        TimelineName,
        IsWait
    }
}