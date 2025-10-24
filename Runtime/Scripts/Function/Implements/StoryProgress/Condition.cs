using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Lucecita.StorylineEngine
{
    public class Condition : TypedFunction<ConditionFunctionValue>, IElementChangeFunc
    {
        public override async UniTask StartFunction()
        {
            base.StartFunction().Forget();
            
            FunctionValue functionValue = TextElement.FunctionValue;
            string condition = functionValue.Get<string>(ConditionFunctionValue.Title);
            object conditionValue = null;
            bool hasConditionValue = functionValue.HasValue(ConditionFunctionValue.Condition);
            if (hasConditionValue)
            {
                conditionValue = true;
            }
            else
            {
                conditionValue = functionValue.Get<object>(ConditionFunctionValue.Condition);
            }

            

            AddCondition(condition, conditionValue);
        }

        public void AddCondition(string condition, object value)
        {
            ConditionComparator.SetCondition(condition, value);
        }

        public TextElement GetNextElement()
        {
            int curElementIndex = TextElement.Index;
            TextElement nextTextElement = EpisodeData.GetNextTextElement(curElementIndex);
            return nextTextElement;
        }
    }
    
    public enum ConditionFunctionValue
    {
        Title,
        Condition
    }
}
