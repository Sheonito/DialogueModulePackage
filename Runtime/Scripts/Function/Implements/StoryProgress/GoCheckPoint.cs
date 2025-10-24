using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Lucecita.StorylineEngine
{
    public class GoCheckPoint : TypedFunction<GoCheckPointFunctionValue>,IElementChangeFunc
    {
        public TextElement GetNextElement()
        {
            int checkPointIndex = (int)GoCheckPointFunctionValue.CheckPoint;
            string checkPoint = TextElement.FunctionValue.Get<string>(GoCheckPointFunctionValue.CheckPoint);
            TextElement checkPointTextElement = EpisodeData.TextElements
                .FirstOrDefault(element => element.FunctionName == nameof(CheckPoint) &&
                                           element.FunctionValue.Get<string>(CheckPointValue.CheckPointName) ==
                                           checkPoint);
            
            string condition = $"{checkPoint}_{TextElement.EpisodeName}_{TextElement.Index}";
            ConditionComparator.SetCondition(condition,true);

            return checkPointTextElement;
        }
    }
    
    public enum GoCheckPointFunctionValue
    {
        CheckPoint,
    }
}
