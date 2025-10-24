using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Lucecita
{
    public class ChoiceButton : Button
    {
        #region StaticVariables

        private static readonly Color LineFocusColor = new Color(1f, 0.3803922f, 0.6f);

        #endregion

        public TextMeshProUGUI TextMeshPro => _textMeshPro;
        public RectTransform RectTransform => _rectTransform;
        public string Text { get; private set; }

        [Header("[View]")] [SerializeField] private Image _bgImage;
        [SerializeField] private TextMeshProUGUI _textMeshPro;

        [SerializeField] private Color _defaultBGColor;

        [SerializeField] private Color _hoverBGColor;

        [SerializeField] private float _showScale;
        [SerializeField] private float _showDuration;

        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _rectTransform;

        protected override void Awake()
        {
            base.Awake();
            ResetView();
        }

        private void ResetView()
        {
            _textMeshPro.text = null;
            _canvasGroup.alpha = 0;
            UpdateRaycast(false);
            UpdateDefaultColor(0);
            
            _canvasGroup.DOKill();
            _rectTransform.DOKill();
            _bgImage.DOKill();
        }

        public void Show(string text)
        {
            ResetView();
            gameObject.SetActive(true);
            _canvasGroup.alpha = 1;
            _textMeshPro.text = text;

            UpdateRaycast(true);
            UpdateDefaultColor(_showDuration);
        }

        private void UpdateDefaultColor(float duration)
        {
            _bgImage.DOColor(_defaultBGColor, duration);
        }
        
        private void UpdateHoverColor(float duration)
        {
            _bgImage.DOColor(_hoverBGColor, duration);
        }

        private void UpdateScale(float scale,float duration)
        {
            _rectTransform.DOScale(scale, duration);
        }
        
        private void UpdateRaycast(bool enable)
        {
            _canvasGroup.interactable = enable;
            _canvasGroup.blocksRaycasts = enable;
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            UpdateHoverColor(_showDuration);
            UpdateScale(_showScale, _showDuration);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            UpdateDefaultColor(_showDuration);
            UpdateScale(1, _showDuration);
        }
    }
}