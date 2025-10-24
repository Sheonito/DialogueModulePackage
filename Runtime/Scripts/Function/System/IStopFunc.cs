using System;
using UnityEngine;

namespace Lucecita.StorylineEngine
{
    public interface IStopFunc
    {
        public event Action onStop;
        public event Action onResume;

    }
   
}