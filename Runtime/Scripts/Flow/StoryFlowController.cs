using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace Lucecita.StorylineEngine
{
    public class StoryFlowController
    {
        public event Action onEpisodeChanged = delegate { };
        public event Action<Function> onExecute = delegate { };
        public TextElement CurTextElement { get; private set; }

        private EpisodeData[] _episodeDatas;
        private EpisodeData _curEpisodeData;

        private FunctionExecutor _functionExecutor;
        private bool _enableNext = true;

        public StoryFlowController(EpisodeData[] episodeDatas)
        {
            _episodeDatas = episodeDatas;
            _functionExecutor = new FunctionExecutor();
            _functionExecutor.onExecuteEnd += OnExecuteEnd;

            RegisterFunctionEvent();
        }

        private void RegisterFunctionEvent()
        {
            IEnumerable<IStopFunc> stopFuncs = _functionExecutor._funcDic.Values.OfType<IStopFunc>();
            foreach (IStopFunc stopFunc in stopFuncs)
            {
                stopFunc.onStop += () => _enableNext = false;
                stopFunc.onResume += () => _enableNext = true;
            }
        }

        public void StartEpisode(string episodeName)
        {
            _curEpisodeData = Array.Find(_episodeDatas, episode => episode.name == episodeName);
            CurTextElement = _curEpisodeData.TextElements[0];
            onEpisodeChanged?.Invoke();

            _functionExecutor.UpdateEpisodeData(CurTextElement.FunctionName, _curEpisodeData);
            _functionExecutor.ExecuteFunction(CurTextElement, true);
        }

        public void Stop()
        {
            _functionExecutor.StopExecutingFunction();
            _curEpisodeData = _episodeDatas[0];
            CurTextElement = _curEpisodeData.TextElements[0];
            _enableNext = true;
        }

        public void NextElement()
        {
            if (!_enableNext)
                return;

            int nextElementIndex = CurTextElement.Index + 1;
            TextElement nextElement = _curEpisodeData.TextElements[nextElementIndex];
            StartElement(nextElement);
        }

        public void StartElement(TextElement textElement)
        {
            string episodeName = textElement.EpisodeName;
            _curEpisodeData = Array.Find(_episodeDatas, episode => episode.name == episodeName);
            CurTextElement = textElement;
            _functionExecutor.UpdateEpisodeData(CurTextElement.FunctionName, _curEpisodeData);
            _functionExecutor.ForceExecuteFunction(CurTextElement, true);
        }

        public void ExecuteInlineFunction(Function function, InlineFunction inlineFunction)
        {
            _functionExecutor.ExecuteInlineFunction(function, inlineFunction);
        }

        private void OnExecuteEnd(Function function)
        {
            onExecute(function);
            switch (function)
            {
                case IEpisodeChangeFunc episodeChangeFunc:
                    string nextEpisodeName = episodeChangeFunc.GetNextEpisodeName();
                    StartEpisode(nextEpisodeName);
                    break;

                case IElementChangeFunc elementChangeFunc:
                    TextElement nextElement = elementChangeFunc.GetNextElement();
                    StartElement(nextElement);
                    break;
            }
        }
    }
}
