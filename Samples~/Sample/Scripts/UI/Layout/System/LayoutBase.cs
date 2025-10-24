using System;
using System.Collections.Generic;
using Lucecita;
using UnityEngine;

namespace Aftertime.HappinessBlossom.UI.Layout
{
    [RequireComponent(typeof(View))]
    public class LayoutBase : MonoBehaviour
    {
        public event Action onShow;
        public event Action onHide;
        public bool IsOn { get; private set; }

        [SerializeField] private List<PopupBase> popups;
        
        protected View _view;
        private bool _isInit;

        protected virtual void Awake()
        {
            Init();
        }

        private void Init()
        {
            if (_isInit)
                return;

            _isInit = true;

            _view = GetComponent<View>();
        }

        protected virtual void Start()
        {
            RegisterEvent();
        }

        private void RegisterEvent()
        {
            RegisterPopupEvent();
        }

        private void RegisterPopupEvent()
        {
            foreach (PopupBase popup in popups)
            {
                popup.onShow += OnPopupShow;
            }

            void OnPopupShow()
            {
                if (_view is INavigationGroup)
                {
                    _view.SetInteractable(false);
                }
            }
        }

        public virtual void Show(float duration = 0)
        {
            IsOn = true;
            
            _view.SetInteractable(true);
            _view.SetBlocksRaycasts(true);
            _view.FadeCanvasGroup(1, duration);
            
            onShow?.Invoke();
        }

        public virtual void Hide(float duration = 0)
        {
            IsOn = false;
            
            _view.SetInteractable(false);
            _view.SetBlocksRaycasts(false);
            _view.FadeCanvasGroup(0, duration);

            PopupManager.Instance.HideAllInLayout(this);
            
            onHide?.Invoke();
        }
    }
}