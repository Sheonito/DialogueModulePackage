using System;
using System.Collections.Generic;
using Aftertime.StorylineEngine;
using Cysharp.Threading.Tasks;
using Lucecita;
using UnityEngine;

namespace Aftertime.SecretSome
{
    public abstract class ChoiceBase<T> : TypedFunction<T> where T : Enum
    {
        public static ElementExecuteAction moveNextTextElement;


        public override async UniTask EndFunction()
        {
            int curElementIndex = TextElement.Index;
            TextElement nextElement = EpisodeData.GetNextTextElement(curElementIndex);
            moveNextTextElement?.Invoke(nextElement);
        }

        protected void AddSelects(List<TextElement> selectElements)
        {
            ChoicePopup choicePopup = PopupManager.Instance.GetPopup<ChoicePopup>() as ChoicePopup;

            foreach (TextElement selectElement in selectElements)
            {
                string title = selectElement.FunctionValue.Get<string>(SelectBaseValue.SelectTitle);

                choicePopup.ShowSelect(title);
                choicePopup.RegisterSelectEvent(title, () => OnSelectDefaultButtonClick(selectElement));
            }
        }

        protected void OnSelectDefaultButtonClick(TextElement selectElement)
        {
            PopupManager.Instance.Hide<ChoicePopup>();

            int selectElementIndex = selectElement.Index;
            TextElement nextElement = EpisodeData.GetNextTextElement(selectElementIndex);
            moveNextTextElement?.Invoke(nextElement);

            OnResume();
        }
        

        protected abstract void OnResume();
    }

    public enum ChoiceBaseValue
    {
    }
}