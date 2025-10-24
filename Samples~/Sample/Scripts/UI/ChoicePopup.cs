using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PopupBase = Lucecita.PopupBase;

namespace Lucecita
{
    public class ChoicePopup : PopupBase
    {
        private ChoiceView _choiceView;

        protected override void Awake()
        {
            base.Awake();
            _choiceView = _view as ChoiceView;
        }

        public override void Show()
        {
            base.Show();
            _choiceView.SetInteractable(true);
        }

        public override async void Hide(bool isHideImmediately = false)
        {
            base.Hide(isHideImmediately);

            if (isHideImmediately == false)
            {
                int fadeDurationForUniTask = (int)(fadeDuration * 1000);
                await UniTask.Delay(fadeDurationForUniTask);   
            }
            
            Init(false);
        }

        public void Init(bool hasDim)
        {
            _choiceView.ResetView();
            _choiceView.SetDim(hasDim);
        }
        
        public void ShowSelect(string selectText)
        {
            ChoiceButton selectButton = _choiceView.GetInActiveButton();
            selectButton.Show(selectText);
        }

        public void RegisterSelectEvent(string selectText, Action onClick)
        {
            onClick += () => _choiceView.SetInteractable(false);
            
            ChoiceButton button = GetSelectButton(selectText);
            button.onClick.AddListener(() => onClick?.Invoke());
        }
        
        public List<ChoiceButton> GetActiveSelectButtons()
        {
            return _choiceView.GetActiveSelectButtons();
        }

        public ChoiceButton GetSelectButton(string selectText)
        {
            return _choiceView.GetSelectButton(selectText);
        }

        public void UpdateContentLayout()
        {
            _choiceView.UpdateContentLayout();
        }
    }
}