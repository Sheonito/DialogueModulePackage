using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Aftertime.StorylineEngine
{
    public class TimelineDirector
    {
        private PlayableDirector _playableDirector;
        private List<TimelineAsset> _timelineAssets;

        public bool IsInit { get; private set; }

        public void Init(PlayableDirector director, List<TimelineAsset> timelineAssets)
        {
            _playableDirector = director;
            _timelineAssets = timelineAssets;
            IsInit = _playableDirector != null;
        }

        public void Play() => _playableDirector.Play();

        public void Play(TimelineAsset timelineAsset)
        {
            _playableDirector.Play(timelineAsset);
        }

        public void Play(string timelineName)
        {
            TimelineAsset timelineAsset = _timelineAssets.FirstOrDefault(asset => asset.name == timelineName);
            if (timelineAsset == null)
            {
                Debug.Log("timelineAsset is null");
                return;
            }

            _playableDirector.Play(timelineAsset);
        }

        public async UniTask PlayAsync(TimelineAsset timelineAsset)
        {
            Play(timelineAsset);
            double duration = _playableDirector.playableAsset.duration;

            int delayTime = (int)(duration * 1000);
            await UniTask.Delay(delayTime);
        }

        public async UniTask PlayAsync(string timelineName)
        {
            Play(timelineName);
            double duration = _playableDirector.playableAsset.duration;

            await UniTask.WaitUntil(() => _playableDirector.state == PlayState.Paused);
            
            // TODO: Raycast 비활성화
        }

        public void PlayComplete(TimelineAsset timelineAsset)
        {
            Play(timelineAsset);
            CompleteCurrentTimeline();
        }

        public void PlayComplete(string timelineName)
        {
            Play(timelineName);
            CompleteCurrentTimeline();
        }

        public void CompleteCurrentTimeline()
        {
            _playableDirector.time = _playableDirector.playableAsset.duration;
            _playableDirector.Evaluate();
        }

        public void StopCurrentTimeline()
        {
            _playableDirector.Stop();
            _playableDirector.playableAsset = null;
        }

        public void MoveCurrentTimelineTime(float time)
        {
            _playableDirector.time = time;
            _playableDirector.Evaluate();
        }

        public bool IsPlaying()
        {
            if (_playableDirector.playableAsset == null)
                return false;

            bool isPlaying = _playableDirector.state == PlayState.Playing;

            return isPlaying;
        }
    }
}