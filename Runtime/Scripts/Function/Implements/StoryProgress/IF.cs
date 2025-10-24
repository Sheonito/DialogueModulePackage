using System;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace Aftertime.StorylineEngine
{
    public class IF : TypedFunction<IFFunctionValue>, IElementChangeFunc
    {
        public TextElement GetNextElement()
        {
            FunctionValue functionValue = TextElement.FunctionValue;
            string condition = functionValue.Get<string>(IFFunctionValue.Condition);
            bool isClearConditions = ConditionComparator.CompareCondition(condition);
            TextElement nextElement;
            if (isClearConditions)
            {
                int curElementIndex = TextElement.Index;
                nextElement = EpisodeData.GetNextTextElement(curElementIndex);
            }
            else
            {
                int curElementIndex = TextElement.Index;
                TextElement endElement = EpisodeData.TextElements
                    .Skip(curElementIndex + 1)
                    .FirstOrDefault(ele => ele.FunctionName == nameof(IF) && ele.FunctionType == FunctionType.End);
                
                int nextElementIndex = endElement.Index + 1;
                nextElement = EpisodeData.TextElements[nextElementIndex];
            }

            return nextElement;
        }
    }
    
    public enum IFFunctionValue
    {
        Condition
    }
}