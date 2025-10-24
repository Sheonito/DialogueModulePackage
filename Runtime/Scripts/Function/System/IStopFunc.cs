using System;
using UnityEngine;

namespace Aftertime.StorylineEngine
{
    public interface IStopFunc
    {
        public event Action onStop;
        public event Action onResume;

    }
   
}