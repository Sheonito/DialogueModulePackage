using System;
using Lucecita.HappinessBlossom.UI;
using Lucecita.StorylineEngine;
using Febucci.UI;
using Febucci.UI.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Lucecita.HappinessBlossom.Presenter
{
    public class ConversationPresenter : MonoBehaviour, IPresenter
    {
        public event Action onTextShowed;
        public event Action onTextCompletedShowed;
        public event UnityAction onClickConversationButton
        {
            add { _conversationView.ConversationButton.onClick.AddListener(value); }
            remove { _conversationView.ConversationButton.onClick.RemoveListener(value); }
        }

        [SerializeField] private ConversationView _conversationView;
        [SerializeField] private float _fadeDuration;

        public void Show()
        {
            Show(_fadeDuration);
        }

        public void Show(float duration)
        {
            _conversationView.FadeCanvasGroup(1,duration);
            _conversationView.SetInteractable(true);
            _conversationView.SetBlocksRaycasts(true);
        }

        public void Hide()
        {
            Hide(_fadeDuration);
        }

        public void Hide(float duration)
        {
            _conversationView.FadeCanvasGroup(0,duration);
            _conversationView.SetInteractable(false);
            _conversationView.SetBlocksRaycasts(false);
        }

        public void ShowText(string text)
        {
            TypewriterByCharacter typeWriter = _conversationView.Typewriter;
            typeWriter.ShowText(text);
            typeWriter.StartShowingText();
        }

        public void ShowTextCompleted()
        {
            TypewriterByCharacter typeWriter = _conversationView.Typewriter;

            if (typeWriter.isActiveAndEnabled && typeWriter.isShowingText)
            {
                typeWriter.SkipTypewriter();

                onTextCompletedShowed?.Invoke();
            }
        }

        public bool IsTextShowing()
        {
            bool isTextShowing = false;

            TypewriterByCharacter typeWriter = _conversationView.Typewriter;
            if (typeWriter.isActiveAndEnabled && typeWriter.isShowingText)
            {
                isTextShowing = true;
            }

            return isTextShowing;
        }

        public void UpdateName(string characterName)
        {
            _conversationView.NameText.SetText(characterName);
        }

        public int GetShowingIndex()
        {
            TypewriterByCharacter typewriter = _conversationView.Typewriter;
            TAnimCore textAnimator = typewriter.TextAnimator;
            int showingIndex = textAnimator.latestCharacterShown.index;

            return showingIndex;
        }

        public void ResetView()
        {
            _conversationView.ResetView();
        }
    }
}