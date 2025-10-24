using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Aftertime.StorylineEngine
{
    public class Function
    {
        public string Name => GetType().Name;
        public event Action onStart = delegate { };
        public event Action onEnd = delegate { };
        public EpisodeData EpisodeData { get; protected set; }
        public TextElement TextElement { get; protected set; }

        protected CancellationTokenSource cts;
        private bool _presentersBound;

        public virtual void Init()
        {
            cts = new CancellationTokenSource();
            BindPresenters();
        }

        public void CancelUniTask()
        {
            cts.Cancel();
            cts.Dispose();
            cts = new CancellationTokenSource();
        }

        public void UpdateTextElement(TextElement textElement)
        {
            TextElement = textElement;
        }

        public void UpdateCurEpisodeData(EpisodeData episodeData)
        {
            EpisodeData = episodeData;
        }

        public virtual async UniTask StartFunction()
        {
            onStart.Invoke();
        }

        public virtual async UniTask EndFunction()
        {
            onEnd.Invoke();
        }
        
        protected void BindPresenters()
        {
            if (_presentersBound)
                return;

            PresenterInjector.Bind(this);
            _presentersBound = true;
        }
    }

    public enum FunctionType
    {
        Start,
        End
    }
}
