using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Aftertime.HappinessBlossom.Directing
{
    public enum ImageDirectingType
    {
        None = -1,
        Zoom,
        Fade,
        Scale,
        Dim,
        Color,
        ShakeRandom,
        ShakeUpDown,
        ShakeLeftRight,
        Move,
        Blur,
        Pixel,
        FillColor,
        Glitch,
        Overlap,
        Overlay,
        Vignette,
    }

    public class ImageDirector
    {
        public RawImage Screen { get; private set; }
        public RawImage CustomScreen { get; private set; }

        private Material _vignetteMaterial;

        private List<SequenceInfo> _sequences;

        #region ShaderVariable

        static class ShaderParams
        {
            public static readonly int GlitchSpeed = Shader.PropertyToID("_GlitchSpeed");
            public static readonly int UseFlickering = Shader.PropertyToID("_UseFlickering");
            public static readonly int NoiseAmount = Shader.PropertyToID("_NoiseAmount");
        }

        private static string GlitchShaderPath = "CGDirecting/Glitch";

        #endregion

        public void Init(RawImage screen, RawImage customScreen, Material vignetteMaterial)
        {
            Screen = screen;
            CustomScreen = customScreen;
            _vignetteMaterial = vignetteMaterial;

            _sequences = new List<SequenceInfo>();
        }

        public void Glitch(Image targetImage, float noiseAmount = 100, float glitchSpeed = 5, bool useFlickering = true)
        {
            if (targetImage == null)
                return;

            Sequence seq = DOTween.Sequence();
            Shader shader = Shader.Find(GlitchShaderPath);
            Material mat = new Material(shader);
            mat.SetFloat(ShaderParams.NoiseAmount, noiseAmount);
            mat.SetFloat(ShaderParams.GlitchSpeed, glitchSpeed);
            mat.SetInt(ShaderParams.UseFlickering, useFlickering ? 1 : 0);

            targetImage.material = mat;

            AddSequence(targetImage, seq, ImageDirectingType.Glitch);
        }

        public Sequence Zoom(Image targetImage, float duration, float scale, Vector2 pos, Action onCompleted = null)
        {
            if (targetImage == null)
                return null;

            Sequence seq = DOTween.Sequence();
            Vector2 scaleVec = new Vector2(scale, scale);
            seq.Join(targetImage.rectTransform.DOScale(scaleVec, duration));
            seq.Join(targetImage.rectTransform.DOAnchorPos(pos, duration));
            seq.onComplete += () => onCompleted?.Invoke();

            AddSequence(targetImage, seq, ImageDirectingType.Zoom);

            return seq;
        }


        public Sequence Fade(Image targetImage, float duration, float fadeValue, Action onCompleted = null)
        {
            if (_sequences.Exists(seq => seq.DirectingType == ImageDirectingType.Fade
                                         && seq.ImageID == targetImage.GetInstanceID()))
            {
                StopImageDirecting(targetImage, ImageDirectingType.Fade);
            }

            Color originColor = targetImage.color;
            targetImage.color = fadeValue > 0
                ? new Color(originColor.r, originColor.g, originColor.b, 0)
                : new Color(originColor.r, originColor.g, originColor.b, 1);

            Sequence seq = DOTween.Sequence();
            seq.Join(targetImage.DOFade(fadeValue, duration));
            seq.onComplete += () => onCompleted?.Invoke();

            AddSequence(targetImage, seq, ImageDirectingType.Fade);

            return seq;
        }

        public Sequence Scale(Image targetImage, float duration, float scaleValue, Action onCompleted = null)
        {
            Sequence seq = DOTween.Sequence();
            seq.Join(targetImage.rectTransform.DOScale(scaleValue, duration));
            seq.onComplete += () => onCompleted?.Invoke();

            AddSequence(targetImage, seq, ImageDirectingType.Scale);

            return seq;
        }

        public Sequence ColorFade(Image targetImage, float duration, Color color, Action onCompleted = null)
        {
            Sequence seq = DOTween.Sequence();
            seq.Join(targetImage.DOColor(color, duration));
            seq.onComplete += () => onCompleted?.Invoke();

            AddSequence(targetImage, seq, ImageDirectingType.Color);

            return seq;
        }

        public Sequence Dim(Image targetImage, float duration, Action onCompleted = null)
        {
            Sequence seq = DOTween.Sequence();
            Color dimColor = new Color(0.85f, 0.85f, 0.85f);
            seq.Join(targetImage.DOColor(dimColor, duration));
            seq.onComplete += () => onCompleted?.Invoke();

            AddSequence(targetImage, seq, ImageDirectingType.Dim);

            return seq;
        }

        public Sequence ShakeRandom(Image targetImage, float strength = 2, int vibrato = 10,
            Action onCompleted = null)
        {
            Sequence seq = DOTween.Sequence();
            seq.Join(targetImage.rectTransform.DOShakeAnchorPos(float.MaxValue, strength, vibrato, 90f, false, false));
            AddSequence(targetImage, seq, ImageDirectingType.ShakeRandom);

            return seq;
        }

        public Sequence ShakeUpDown(Image targetImage, float duration = 0.6f, float strength = 10, int vibrato = 10)
        {
            if (targetImage == null)
                return null;

            Sequence seq = DOTween.Sequence();
            seq.Join(targetImage.rectTransform.DOShakeAnchorPos(duration, Vector2.up * strength, vibrato, 0, false,
                false));

            AddSequence(targetImage, seq, ImageDirectingType.ShakeUpDown);

            return seq;
        }

        public Sequence ShakeLeftRight(Image targetImage, float duration = 0.6f, float strength = 10, int vibrato = 10)
        {
            if (targetImage == null)
                return null;

            Sequence seq = DOTween.Sequence();
            seq.Join(targetImage.rectTransform.DOShakeAnchorPos(duration, Vector2.right * strength, vibrato, 0, false,
                false));

            AddSequence(targetImage, seq, ImageDirectingType.ShakeLeftRight);

            return seq;
        }

        public Sequence Move(Image targetImage, float duration, Vector2 pos, Ease ease = Ease.Unset)
        {
            if (targetImage == null)
                return null;

            Sequence seq = DOTween.Sequence();
            seq.Join(targetImage.rectTransform.DOAnchorPos(pos, duration)
                .SetEase(ease));

            AddSequence(targetImage, seq, ImageDirectingType.Move);

            return seq;
        }

        public void Overlap(Image originImage, Image overlapImage, Sprite sprite, float duration,
            Action onCompleted = null)
        {
            originImage.DOKill();
            overlapImage.DOKill();

            overlapImage.enabled = true;
            overlapImage.sprite = sprite;
            overlapImage.color = new Color(1, 1, 1, 0);
            overlapImage.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            overlapImage.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            overlapImage.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            overlapImage.rectTransform.anchoredPosition = Vector2.zero;

            Sequence seq = DOTween.Sequence();
            seq.Join(overlapImage.DOFade(1.0f, duration));
            seq.Join(originImage.DOFade(0f, duration));
            seq.OnComplete(() =>
            {
                originImage.sprite = sprite;
                originImage.color = Color.white;
                originImage.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                originImage.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                originImage.rectTransform.pivot = new Vector2(0.5f, 0.5f);
                originImage.rectTransform.anchoredPosition = Vector2.zero;

                overlapImage.enabled = false;
                overlapImage.sprite = null;
                onCompleted?.Invoke();
            });

            AddSequence(overlapImage, seq, ImageDirectingType.Overlap);
        }

        public void Overlay(Image originImage, Image overlayImage, Sprite sprite, float duration,
            Action onCompleted = null)
        {
            originImage.DOKill();
            overlayImage.DOKill();

            overlayImage.enabled = true;
            overlayImage.sprite = sprite;
            overlayImage.color = new Color(1, 1, 1, 0);
            overlayImage.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            overlayImage.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            overlayImage.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            overlayImage.rectTransform.anchoredPosition = Vector2.zero;

            Sequence seq = DOTween.Sequence();
            seq.Join(overlayImage.DOFade(1.0f, duration));
            seq.OnComplete(() =>
            {
                originImage.sprite = sprite;
                originImage.color = Color.white;
                originImage.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                originImage.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                originImage.rectTransform.pivot = new Vector2(0.5f, 0.5f);
                originImage.rectTransform.anchoredPosition = Vector2.zero;

                overlayImage.color = new Color(1, 1, 1, 0);
                overlayImage.sprite = null;
                onCompleted?.Invoke();
            });

            AddSequence(overlayImage, seq, ImageDirectingType.Overlay);
        }

        public void VignetteIn(Image targetImage, Sprite sprite, Color color, float duration, Action onCompleted = null)
        {
            const string IntensityPropertyName = "_Intensity";

            targetImage.DOKill();
            targetImage.gameObject.SetActive(true);
            targetImage.enabled = true;
            targetImage.color = color;
            targetImage.sprite = sprite;
            targetImage.material = _vignetteMaterial;
            targetImage.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            targetImage.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            targetImage.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            targetImage.rectTransform.anchoredPosition = Vector2.zero;

            _vignetteMaterial.DOKill();
            _vignetteMaterial.SetFloat(IntensityPropertyName, 0);

            Sequence seq = DOTween.Sequence();
            seq.Join(_vignetteMaterial.DOFloat(1, IntensityPropertyName, duration));
            seq.OnComplete(() =>
            {
                targetImage.material = null;
                onCompleted?.Invoke();
            });

            AddSequence(targetImage, seq, ImageDirectingType.Vignette);
        }

        public void VignetteInOverlay(Image originImage, Image overlayImage, Sprite sprite, Color color, float duration,
            Action onCompleted = null)
        {
            onCompleted += () =>
            {
                originImage.sprite = sprite;
                originImage.color = Color.white;
                originImage.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                originImage.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                originImage.rectTransform.pivot = new Vector2(0.5f, 0.5f);
                originImage.rectTransform.anchoredPosition = Vector2.zero;

                overlayImage.enabled = false;
                overlayImage.sprite = null;
                overlayImage.material = null;
            };
            VignetteIn(overlayImage, sprite, color, duration, onCompleted);
        }

        public void StopImageDirecting(Image targetImage, ImageDirectingType directingType)
        {
            if (_sequences.Count == 0)
                return;

            int characterID = targetImage.GetInstanceID();
            SequenceInfo sequenceInfo =
                _sequences.FirstOrDefault(seq => seq.ImageID == characterID && seq.DirectingType == directingType);

            if (sequenceInfo == null)
                return;

            // DOTween Sequence Kill
            Sequence sequence = sequenceInfo.Sequence;
            sequence.Kill();
            RemoveSequence(sequenceInfo);


            if (sequenceInfo.DirectingType == ImageDirectingType.Glitch)
            {
                // Glitch는 duration이 없고, shader를 사용하기 때문에 material만 null 처리
                targetImage.material = null;
            }
        }

        public void StopImageAllDirecting(Image targetImage)
        {
            int imageID = targetImage.GetInstanceID();
            List<SequenceInfo> sequences = _sequences.Where(seq => seq.ImageID == imageID).ToList();
            foreach (SequenceInfo sequence in sequences)
            {
                ImageDirectingType directingType = sequence.DirectingType;
                StopImageDirecting(targetImage, directingType);
            }
        }

        public void StopAllDirecting()
        {
            Screen.material = null;
            CustomScreen.material = null;
            Screen.color = new Color(1, 1, 1, 0);
            CustomScreen.color = new Color(1, 1, 1, 0);

            foreach (SequenceInfo sequenceInfo in _sequences)
            {
                Sequence sequence = sequenceInfo.Sequence;
                sequence.Kill();
            }

            _sequences.Clear();
        }

        private void AddSequence(Image image, Sequence seq, ImageDirectingType directingType)
        {
            int imageID = image.GetInstanceID();
            SequenceInfo sequenceInfo = new SequenceInfo(imageID, directingType, seq);

            seq.onComplete += () => RemoveSequence(sequenceInfo);
            _sequences.Add(sequenceInfo);
        }

        private void RemoveSequence(SequenceInfo sequenceInfo)
        {
            if (_sequences.Count == 0)
                return;

            bool isExist = _sequences.Contains(sequenceInfo);

            if (isExist)
            {
                _sequences.Remove(sequenceInfo);
            }
        }
    }
}