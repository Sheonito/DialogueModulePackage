using Lucecita;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Lucecita.HappinessBlossom.UI.Page
{
#if UNITY_EDITOR
    [RequireComponent(typeof(PageView))]
#endif
    public class PageBase : MonoBehaviour
    {
        public int PageIndex => _pageIndex;
        public bool IsOn => _view.IsOn;

        [SerializeField] protected PopupBase _parentPopup;
        [SerializeField] private int _pageIndex;
        [SerializeField] protected float _fadeInDuration = 0.5f;
        [SerializeField] protected float _fadeOutDuration = 0.5f;
        protected PageView _view;

        protected virtual void Awake()
        {
            _view = GetComponent<PageView>();
        }

        public virtual void Show()
        {
            if (_view.IsOn)
                return;

            _view.SetInteractable(true);
            _view.SetBlocksRaycasts(true);
            _view.FadeCanvasGroup(1, _fadeInDuration);
            PopupManager.Instance.PageStackMap.Push(_parentPopup, this);
        }

        public virtual void Hide()
        {
            if (_view.IsOn == false)
                return;

            _view.SetInteractable(false);
            _view.SetBlocksRaycasts(false);
            _view.FadeCanvasGroup(0, _fadeOutDuration);
            PopupManager.Instance.PageStackMap.Pop(_parentPopup);
        }

        public virtual void OnSelectMenuButton()
        {
            _view.SelectFirstSelectedObject();
        }

        public void ActivateLastSelectedObject()
        {
            if (_view.LastSelected != null)
                EventSystem.current.SetSelectedGameObject(_view.LastSelected.gameObject);
        }

        public bool HasSelected()
        {
            return _view.HasSelected();
        }

        public void ResetView()
        {
            _view.ResetView();
        }
    }
}