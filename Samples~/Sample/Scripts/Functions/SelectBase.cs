using System;
using System.Collections.Generic;
using Aftertime.StorylineEngine;
using Cysharp.Threading.Tasks;

namespace Lucecita
{
    public class SelectBase<T> : TypedFunction<T>, IElementChangeFunc where T : Enum
    {
        public TextElement GetNextElement()
        {
            FunctionType functionType = TextElement.FunctionType;
            if (functionType == FunctionType.Start)
            {
                int curElementIndex = TextElement.Index;
                TextElement nextElement = EpisodeData.GetNextTextElement(curElementIndex);
                return nextElement;
            }
            else
            {
                int nextTextElementIndex = TextElement.Index;
                while (true)
                {
                    TextElement curTextElement = EpisodeData.TextElements[nextTextElementIndex];
                    ++nextTextElementIndex;

                    bool isChoiceEnd = curTextElement.FunctionName.Contains(nameof(Choice));
                    if (isChoiceEnd)
                        break;
                }

                return EpisodeData.TextElements[nextTextElementIndex];
            }
        }
    }

    public enum SelectBaseValue
    {
        SelectTitle
    }
}