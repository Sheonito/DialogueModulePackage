using System;
using Lucecita.HappinessBlossom;
using Lucecita.HappinessBlossom.UI.Layout;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Lucecita
{
    #if UNITY_EDITOR
    [RequireComponent(typeof(View))]
    #endif
    public class PopupBase : MonoBehaviour
    {
        public LayoutBase OwnerLayout => _ownerLayout;
        public bool IsActive { get; protected set; }
        public event Action onShow;
        public event Action onHide;
        public event Action onHideCompleted;
        public float FadeDuration => fadeDuration;

        [SerializeField] protected LayoutBase _ownerLayout;
        [SerializeField] protected float fadeDuration = 0.5f;

        protected View _view;

        protected virtual void Awake()
        {
            _view = GetComponent<View>();
        }

        public virtual void Show()
        {
            _view.SetInteractable(true);
            _view.SetBlocksRaycasts(true);
            _view.FadeCanvasGroup(1,fadeDuration);
            IsActive = true;
            
            onShow?.Invoke();
        }

        public virtual void Hide(bool isHideImmediately = false)
        {
            float duration = isHideImmediately ? 0 : fadeDuration;
            
            _view.SetInteractable(false);
            _view.SetBlocksRaycasts(false);
            _view.FadeCanvasGroup(0,duration,onHideCompleted);
            
            IsActive = false;
            
            onHide?.Invoke();
        }
        
        public void SetFirstSelectedObject(Selectable selectable)
        {
            _view.SetFirstSelectedGameObject(selectable);
        }

        public void SelectFirstSelectedObject()
        {
            _view.SelectFirstSelectedObject();
        }
        
        public void ActivateLastSelectedObject()
        {
            if (_view.LastSelected != null)
                EventSystem.current.SetSelectedGameObject(_view.LastSelected.gameObject);
        }
    }
}