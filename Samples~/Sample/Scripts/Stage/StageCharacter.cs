using Lucecita.HappinessBlossom.Directing;
using DG.Tweening;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Lucecita.HappinessBlossom.Stage
{
    public class StageCharacter : StageSpriteRenderer
    {
        public const float DefaultMoveDuration = 0.3f;
        public const float DefaultFocusDuration = 0.3f;
        private static readonly Color FocusColor = new Color(1, 1, 1, 1);
        private static readonly Color UnFocusColor = new Color(0.7f, 0.7f, 0.7f, 1);
        private Vector2 _focusScale;
        private Vector2 _unFocusScale;

        public SpriteRenderer ObjectSpriteRenderer => _objectSpriteRenderer;
        public SpriteRenderer OverlayObjectSpriteRenderer => _overlayObjectSpriteRenderer;
        [SerializeField] private SpriteRenderer _objectSpriteRenderer;
        [SerializeField] private SpriteRenderer _overlayObjectSpriteRenderer;

        public bool isOn;
        private bool _isSpeaker;

        private Vector3 _originScale;
        private Vector3 _originPos;

        protected override void Awake()
        {
            base.Awake();
            _focusScale = transform.localScale;
            float unFocusValue = _focusScale.x - (transform.localScale.x / 20);
            _unFocusScale = new Vector2(unFocusValue, unFocusValue);
            _originScale = transform.localScale;
            _originPos = transform.localPosition;
        }

        public virtual string GetSpriteName()
        {
            if (_overlaySpriteRenderer.sprite != null)
                return _overlaySpriteRenderer.sprite.name;

            else
            {
                if (_spriteRenderer.sprite != null)
                    return _spriteRenderer.sprite.name;
            }

            return null;
        }

        public virtual void Move(Vector3 pos,float duration = DefaultMoveDuration)
        {
            transform.DOKill();

            float alphaValue = _spriteRenderer.color.a;
            if (alphaValue == 0)
            {
                Show(duration);
                transform.position = pos;
            }
            else
            {
                ReFadeInSpriteRenderer(duration);
                transform.DOMove(pos, duration);
            }
        }

        public virtual void Focus(float duration = DefaultFocusDuration)
        {
            if (_spriteRenderer.color.a != 1)
                return;

            _isSpeaker = true;

            transform.DOKill();
            transform.transform.DOScale(_focusScale, duration);
            
            _spriteRenderer.DOColor(FocusColor, duration);
            _objectSpriteRenderer.DOColor(FocusColor, duration);
            
            if (_overlaySpriteRenderer != null)
                _overlaySpriteRenderer.DOColor(FocusColor, duration);
            if (_overlayObjectSpriteRenderer != null)
                _overlayObjectSpriteRenderer.DOColor(FocusColor, duration);
        }

        public virtual void UnFocus(float duration = DefaultFocusDuration)
        {
            _isSpeaker = false;

            transform.DOKill();
            transform.DOScale(_unFocusScale, duration);
            
            _spriteRenderer.DOColor(UnFocusColor, duration);
            _objectSpriteRenderer.DOColor(UnFocusColor, duration);
            
            if (_overlaySpriteRenderer.sprite != null)
                _overlaySpriteRenderer.DOColor(UnFocusColor, duration);
            if (_overlayObjectSpriteRenderer.sprite != null)
                _overlayObjectSpriteRenderer.DOColor(UnFocusColor, duration);
        }

        public virtual void Show(float duration = DefaultMoveDuration)
        {
            _isSpeaker = true;
            _spriteRenderer.gameObject.SetActive(true);
            _spriteRenderer.transform.localScale = Vector3.one;
            _spriteRenderer.DOFade(1, duration).onKill += () => ReFadeInSpriteRenderer(duration);
            _spriteRenderer.DOFade(1, duration).onComplete += ResetOverlaySpriteRenderer;
        }

        protected virtual void ReFadeInSpriteRenderer(float duration = DefaultMoveDuration)
        {
            if (_isSpeaker)
            {
                float alphaValue = _spriteRenderer.color.a;
                if (isOn && alphaValue != 1)
                {
                    float alphaDuration = duration - alphaValue;
                    _spriteRenderer.DOFade(1, alphaDuration);
                }
            }
            else
            {
                UnFocus();
            }
        }

        public virtual void ShowImmediately()
        {
            gameObject.SetActive(true);
            _spriteRenderer.enabled = true;
            _spriteRenderer.color = Color.white;
            isOn = true;
        }

        public virtual void Hide(float duration = DefaultMoveDuration)
        {
            isOn = false;

            _spriteRenderer.DOKill();
            _overlaySpriteRenderer.DOKill();
            _objectSpriteRenderer.DOKill();
            _overlayObjectSpriteRenderer.DOKill();

            // TODO: SpriteDirector 援ы쁽
            SpriteRendererDirector spriteRendererDirector = DirectingManager.SpriteRendererDirector;
            spriteRendererDirector.StopImageAllDirecting(_spriteRenderer);
            spriteRendererDirector.StopImageAllDirecting(_overlaySpriteRenderer);

            _spriteRenderer.DOFade(0, duration).onComplete += ResetCharacter;
            _spriteRenderer.DOFade(0, duration).onKill += ResetCharacter;
            _overlaySpriteRenderer.DOFade(0, duration);
            _objectSpriteRenderer.DOFade(0, duration * 0.9f);
            _overlayObjectSpriteRenderer.DOFade(0, duration * 0.9f);
        }

        public virtual void ResetCharacter()
        {
            transform.DOKill();
            transform.localPosition = _originPos;
            transform.localScale = _originScale;
            
            ResetOverlaySpriteRenderer();
            _spriteRenderer.gameObject.SetActive(false);
            _spriteRenderer.enabled = false;
            _spriteRenderer.sprite = null;
            _spriteRenderer.transform.localScale = Vector3.one;
            _spriteRenderer.color = new Color(1, 1, 1, 0);

            _objectSpriteRenderer.gameObject.SetActive(false);
            _objectSpriteRenderer.enabled = false;
            _objectSpriteRenderer.sprite = null;
            _objectSpriteRenderer.transform.localScale = Vector3.one;
            _objectSpriteRenderer.color = new Color(1, 1, 1, 0);

            isOn = false;
        }

        private void ResetOverlaySpriteRenderer()
        {
            _overlaySpriteRenderer.gameObject.SetActive(false);
            _overlaySpriteRenderer.enabled = false;
            _overlaySpriteRenderer.sprite = null;
            _overlaySpriteRenderer.transform.localScale = Vector3.one;
            _overlaySpriteRenderer.color = new Color(1, 1, 1, 0);

            _overlayObjectSpriteRenderer.gameObject.SetActive(false);
            _overlayObjectSpriteRenderer.enabled = false;
            _overlayObjectSpriteRenderer.sprite = null;
            _overlayObjectSpriteRenderer.transform.localScale = Vector3.one;
            _overlayObjectSpriteRenderer.color = new Color(1, 1, 1, 0);
        }
    }
}
