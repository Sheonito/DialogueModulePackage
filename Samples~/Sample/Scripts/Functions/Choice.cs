using System;
using System.Collections.Generic;
using Aftertime.HappinessBlossom.Presenter;
using Aftertime.SecretSome;
using Aftertime.StorylineEngine;
using Cysharp.Threading.Tasks;
using Enumerable = System.Linq.Enumerable;

namespace Lucecita
{
    public class Choice : ChoiceBase<ChoiceBaseValue>,IStopFunc
    {
        public event Action onStop;
        public event Action onResume;

        private ChoicePopup _choicePopup;
        [Presenter] private ConversationPresenter _presenter;

        public override async UniTask StartFunction()
        {
            onStop?.Invoke();
            ShowChoicePopupAndInit();
            ShowSelectButtons();
        }

        public override async UniTask EndFunction()
        {
            int curElementIndex = TextElement.Index;
            TextElement nextElement = EpisodeData.GetNextTextElement(curElementIndex);
            moveNextTextElement?.Invoke(nextElement);
        }

        private void ShowChoicePopupAndInit()
        {
            PopupManager.Instance.Push<ChoicePopup>();

            if (_choicePopup == null)
                _choicePopup = PopupManager.Instance.GetPopup<ChoicePopup>() as ChoicePopup;
        }

        private void ShowSelectButtons()
        {
            List<TextElement> selectElements = GetSelectElements();
            AddSelects(selectElements);
        }

        private List<TextElement> GetSelectElements()
        {
            List<TextElement> selectElements = new List<TextElement>();
            for (int i = 0; i < TextElement.SelectElements.Length; i++)
            {
                List<TextElement> textElements = Enumerable.ToList(EpisodeData.TextElements);
                TextElement selectElement = textElements.Find(x => x.Index.ToString() == TextElement.SelectElements[i]);

                selectElements.Add(selectElement);
            }

            return selectElements;
        }

        protected override void OnResume()
        {
            onResume?.Invoke();
        }
    }
}