using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Aftertime.StorylineEngine
{
    public class CheckPoint : TypedFunction<CheckPointValue>,IElementChangeFunc
    {
        public TextElement GetNextElement()
        {
            int curElementIndex = TextElement.Index;
            TextElement nextTextElement =  EpisodeData.GetNextTextElement(curElementIndex);
            return nextTextElement;
        }
    }

    public enum CheckPointValue
    {
        CheckPointName
    }
}

