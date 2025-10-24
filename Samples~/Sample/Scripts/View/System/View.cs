using System;
using Lucecita.HappinessBlossom.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Lucecita.HappinessBlossom
{
#if UNITY_EDITOR
    [RequireComponent(typeof(CanvasGroup))]
#endif
    public abstract class View : MonoBehaviour
    {
        [SerializeField] protected Selectable firstSelectedObject;

        public bool IsOn => _canvasGroup.interactable;
        public Selectable LastSelected { get; protected set; }

        protected CanvasGroup _canvasGroup;
        protected Navigation _activeNavigation;
        protected Navigation _inActiveNavigation;

        protected Selectable _originFirstSelectedObject;

        protected virtual void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _originFirstSelectedObject = firstSelectedObject;

            if (this is INavigationGroup)
            {
                InitNavigation();
            }
        }

        public void InitNavigation()
        {
            _activeNavigation = new Navigation()
            {
                mode = Navigation.Mode.Automatic,
            };
            _inActiveNavigation = new Navigation()
            {
                mode = Navigation.Mode.None,
            };

            INavigationGroup navigationGroup = (INavigationGroup)this;
            navigationGroup.InActiveNavigation(_inActiveNavigation);
        }

        public void FadeCanvasGroup(float value, float duration, Action onCompleted = null)
        {
            if (_canvasGroup == null)
                return;

            _canvasGroup.DOKill();
            _canvasGroup.DOFade(value, duration)
                .SetUpdate(true) // Time.Scale 값을 무시하고 Update에서 트윈을 진행함.
                .onComplete += () => onCompleted?.Invoke();
        }

        public void SetCanvasGroupAlpha(float value)
        {
            if (_canvasGroup == null)
                return;

            _canvasGroup.DOKill();
            _canvasGroup.alpha = value;
        }

        public virtual async void SetInteractable(bool enable)
        {
            if (_canvasGroup == null)
                return;

            _canvasGroup.interactable = enable;

            if (this is INavigationGroup navigatable)
            {
                if (enable)
                {
                    if (firstSelectedObject != null)
                    {
                        if (firstSelectedObject.gameObject.activeSelf == false)
                        {
                            await UniTask.WaitUntil(() => firstSelectedObject.gameObject.activeSelf);
                        }

                        bool isSelected = EventSystem.current.currentSelectedGameObject ==
                                          firstSelectedObject.gameObject;
                        if (isSelected)
                            firstSelectedObject.OnSelect(null);

                        SelectFirstSelectedObject();
                    }

                    navigatable.ActiveNavigation(_activeNavigation);
                }
                else
                {
                    navigatable.InActiveNavigation(_inActiveNavigation);
                }
            }
        }

        public void SetBlocksRaycasts(bool enable)
        {
            if (_canvasGroup == null)
                return;

            _canvasGroup.blocksRaycasts = enable;
        }

        public virtual void ResetView()
        {
        }

        public virtual void SelectFirstSelectedObject()
        {
            if (Mouse.current != null && Mouse.current.leftButton.wasReleasedThisFrame)
                return;

            if (EventSystem.current.alreadySelecting == false)
                EventSystem.current.SetSelectedGameObject(firstSelectedObject?.gameObject);
        }
        
        public virtual void SetFirstSelectedGameObject(Selectable selectable)
        {
            firstSelectedObject = selectable;
        }
        
        public virtual bool HasSelected()
        {
            return false;
        }
    }
}