using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Aftertime.StorylineEngine
{
    public delegate void ElementExecuteAction(TextElement nextTextElement);

    public class FunctionExecutor
    {
        public event Action<Function> onExecuteStart;
        public event Action<Function> onExecuteEnd;

        public Dictionary<string, Function> _funcDic;
        public bool isExecuting { get; private set; }

        private CancellationTokenSource cts;

        public FunctionExecutor()
        {
            _funcDic = new Dictionary<string, Function>(StringComparer.OrdinalIgnoreCase);
            cts = new CancellationTokenSource();

            InitStoryDictionary();
        }

        public IReadOnlyDictionary<string, Function> Functions => _funcDic;
        

        private void InitStoryDictionary()
        {
            Dictionary<Type, Function> instances = StoryUtil.CreateAllDerivedInstances<Function>();
            _funcDic = instances.ToDictionary(pair => pair.Key.Name, pair => pair.Value,
                StringComparer.OrdinalIgnoreCase);

            foreach (Function function in _funcDic.Values)
            {
                function.Init();
            }

            FunctionEventRegistry.Init(_funcDic.Values);
        }


        public void StopExecutingFunction()
        {
            CancelFunctionUniTask();
            isExecuting = false;
        }

        private void CancelFunctionUniTask()
        {
            cts.Cancel();
            cts = new CancellationTokenSource();

            foreach (Function function in _funcDic.Values)
            {
                function.CancelUniTask();
            }
        }

        public void UpdateEpisodeData(string functionType, EpisodeData episodeData)
        {
            Function storyFunc = _funcDic[functionType];
            storyFunc.UpdateCurEpisodeData(episodeData);
        }

        public virtual void ForceExecuteFunction(TextElement curTextElement, bool isContinuable)
        {
            Execute(curTextElement, isContinuable).Forget();
        }

        public virtual void ExecuteFunction(TextElement curTextElement, bool isContinuable)
        {
            if (isExecuting)
                return;

            Execute(curTextElement, isContinuable).Forget();
        }

        public virtual async UniTask ForceExecuteFunctionAsync(TextElement curTextElement, bool isContinuable)
        {
            await Execute(curTextElement, isContinuable)
                .AttachExternalCancellation(cts.Token);
        }

        public virtual async UniTask ExecuteFunctionAsync(TextElement curTextElement, bool isContinuable)
        {
            await Execute(curTextElement, isContinuable)
                .AttachExternalCancellation(cts.Token);
        }

        private async UniTask Execute(TextElement curTextElement, bool isContinuable)
        {
            string funcName = curTextElement.FunctionName;
            Function function = _funcDic[funcName];
            function.UpdateTextElement(curTextElement);

            onExecuteStart?.Invoke(function);
            isExecuting = true;

            if (function.TextElement.FunctionType == FunctionType.Start)
            {
                await function.StartFunction().AttachExternalCancellation(cts.Token);
            }
            else
            {
                await function.EndFunction().AttachExternalCancellation(cts.Token);
            }

            isExecuting = false;

            if (cts.Token.IsCancellationRequested == false)
                onExecuteEnd?.Invoke(function);
        }

        public void ExecuteInlineFunction(Function function, InlineFunction inlineFunction)
        {
            if (function == null)
                return;

            EpisodeData curEpisodeData = function.EpisodeData;
            TextElement inlineTextElement = new TextElement(inlineFunction.RawText);

            function.UpdateCurEpisodeData(curEpisodeData);
            function.UpdateTextElement(inlineTextElement);
            function.StartFunction();
        }
    }
}
