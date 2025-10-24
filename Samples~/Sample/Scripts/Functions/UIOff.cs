using System;
using Lucecita.HappinessBlossom.Presenter;
using Lucecita.StorylineEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.InputSystem;

namespace Lucecita.HappinessBlossom
{
    public class UIOff : TypedFunction<UIOffFunctionValue>, IElementChangeFunc
    {
        [Presenter] private ConversationPresenter _presenter;
        
        private float _duration;
        private bool _isOn;
        private const float defaultHideDuration = 0.3f;

        public override async UniTask StartFunction()
        {
            float duration = TextElement.FunctionValue.Get<float>(UIOffFunctionValue.Duration);
            SetUI(false, duration);
        }

        public override async UniTask EndFunction()
        {
            float duration = TextElement.FunctionValue.Get<float>(UIOffFunctionValue.Duration);
            int delayTime = (int)(duration * 1000);
            SetUI(true, duration);

            await UniTask.Delay(delayTime);
        }

        private void SetUI(bool enable, float duration)
        {
            _isOn = enable;

            if (_isOn)
            {
                _presenter.Show(duration);
            }
            else
            {
                _presenter.Hide(duration);
            }
        }

        public TextElement GetNextElement()
        {
            int curElementIndex = TextElement.Index;
            TextElement nextTextElement = EpisodeData.GetNextTextElement(curElementIndex);
            return nextTextElement;
        }
    }

    public enum UIOffFunctionValue
    {
        Duration
    }
}
