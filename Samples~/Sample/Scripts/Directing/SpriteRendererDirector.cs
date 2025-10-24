using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Lucecita.HappinessBlossom.Directing
{
    public class SpriteRendererDirector
    {
        private List<SequenceInfo> _sequences;

        private Material _vignetteMat;
        private Material _zSpriteMat;

        public void Init(Material vignetteMat, Material zSpriteMat)
        {
            _sequences = new List<SequenceInfo>();
            _vignetteMat = vignetteMat;
            _zSpriteMat = zSpriteMat;
        }

        public Sequence Zoom(SpriteRenderer targetSr, float duration, float scale, Vector2 pos,
            Action onCompleted = null)
        {
            if (targetSr == null)
                return null;

            targetSr.gameObject.SetActive(true);
            Sequence seq = DOTween.Sequence();
            Vector2 scaleVec = new Vector2(scale, scale);
            seq.Join(targetSr.transform.DOScale(scaleVec, duration));
            seq.Join(targetSr.transform.DOMove(pos, duration));
            seq.onComplete += () => onCompleted?.Invoke();

            AddSequence(targetSr, seq, ImageDirectingType.Zoom);

            return seq;
        }


        public Sequence Fade(SpriteRenderer targetSr, float duration, float fadeValue, Action onCompleted = null)
        {
            if (_sequences.Exists(seq => seq.DirectingType == ImageDirectingType.Fade
                                         && seq.ImageID == targetSr.GetInstanceID()))
            {
                StopImageDirecting(targetSr, ImageDirectingType.Fade);
            }

            targetSr.gameObject.SetActive(true);
            Color originColor = targetSr.color;
            targetSr.color = fadeValue > 0
                ? new Color(originColor.r, originColor.g, originColor.b, 0)
                : new Color(originColor.r, originColor.g, originColor.b, 1);

            Sequence seq = DOTween.Sequence();
            seq.Join(targetSr.DOFade(fadeValue, duration));
            seq.onComplete += () => onCompleted?.Invoke();

            AddSequence(targetSr, seq, ImageDirectingType.Fade);

            return seq;
        }

        public Sequence Scale(SpriteRenderer targetSr, float duration, float scaleValue, Action onCompleted = null)
        {
            targetSr.gameObject.SetActive(true);

            Sequence seq = DOTween.Sequence();
            seq.Join(targetSr.transform.DOScale(scaleValue, duration));
            seq.onComplete += () => onCompleted?.Invoke();

            AddSequence(targetSr, seq, ImageDirectingType.Scale);

            return seq;
        }

        public Sequence ColorFade(SpriteRenderer targetSr, float duration, Color color, Action onCompleted = null)
        {
            targetSr.gameObject.SetActive(true);

            Sequence seq = DOTween.Sequence();
            seq.Join(targetSr.DOColor(color, duration));
            seq.onComplete += () => onCompleted?.Invoke();

            AddSequence(targetSr, seq, ImageDirectingType.Color);

            return seq;
        }

        public Sequence Dim(SpriteRenderer targetSr, float duration, Action onCompleted = null)
        {
            targetSr.gameObject.SetActive(true);

            Sequence seq = DOTween.Sequence();
            Color dimColor = new Color(0.85f, 0.85f, 0.85f);
            seq.Join(targetSr.DOColor(dimColor, duration));
            seq.onComplete += () => onCompleted?.Invoke();

            AddSequence(targetSr, seq, ImageDirectingType.Dim);

            return seq;
        }

        public Sequence ShakeRandom(SpriteRenderer targetSr, float strength = 2, int vibrato = 10,
            Action onCompleted = null)
        {
            targetSr.gameObject.SetActive(true);

            Sequence seq = DOTween.Sequence();
            seq.Join(targetSr.transform.DOShakePosition(float.MaxValue, strength, vibrato, 90f, false, false));
            AddSequence(targetSr, seq, ImageDirectingType.ShakeRandom);

            return seq;
        }

        public Sequence ShakeUpDown(SpriteRenderer targetSr, float duration = 0.6f, float strength = 10,
            int vibrato = 10)
        {
            if (targetSr == null)
                return null;

            targetSr.gameObject.SetActive(true);

            Sequence seq = DOTween.Sequence();
            seq.Join(targetSr.transform.DOShakePosition(duration, Vector2.up * strength, vibrato, 0, false,
                false));

            AddSequence(targetSr, seq, ImageDirectingType.ShakeUpDown);

            return seq;
        }

        public Sequence ShakeLeftRight(SpriteRenderer targetSr, float duration = 0.6f, float strength = 10,
            int vibrato = 10)
        {
            if (targetSr == null)
                return null;

            targetSr.gameObject.SetActive(true);

            Sequence seq = DOTween.Sequence();
            seq.Join(targetSr.transform.DOShakePosition(duration, Vector2.right * strength, vibrato, 0, false,
                false));

            AddSequence(targetSr, seq, ImageDirectingType.ShakeLeftRight);

            return seq;
        }

        public Sequence Move(SpriteRenderer targetSr, float duration, Vector2 pos, Ease ease = Ease.Unset)
        {
            if (targetSr == null)
                return null;

            targetSr.gameObject.SetActive(true);

            Sequence seq = DOTween.Sequence();
            seq.Join(targetSr.transform.DOMove(pos, duration)
                .SetEase(ease));

            AddSequence(targetSr, seq, ImageDirectingType.Move);

            return seq;
        }


        public void Overlap(SpriteRenderer originSr, SpriteRenderer overlapSr, Sprite sprite, float duration,
            bool isResizeScale, Action onCompleted = null)
        {
            originSr.DOKill();
            overlapSr.DOKill();

            overlapSr.gameObject.SetActive(true);
            overlapSr.enabled = true;
            overlapSr.sprite = sprite;
            if (isResizeScale)
                SpriteRendererUtil.CalculateScale(overlapSr);
            overlapSr.color = new Color(1, 1, 1, 0);

            Sequence seq = DOTween.Sequence();
            seq.Join(overlapSr.DOFade(1.0f, duration));
            seq.Join(originSr.DOFade(0f, duration));
            seq.OnComplete(() =>
            {
                originSr.sprite = sprite;
                if (isResizeScale)
                    SpriteRendererUtil.CalculateScale(originSr);
                originSr.color = Color.white;
                overlapSr.enabled = false;
                overlapSr.sprite = null;
                overlapSr.gameObject.SetActive(false);
                onCompleted?.Invoke();
            });

            AddSequence(overlapSr, seq, ImageDirectingType.Overlap);
        }

        public void Overlay(SpriteRenderer originSr, SpriteRenderer overlaySr, Sprite sprite, float duration,
            bool isCalculateScale, Action onCompleted = null)
        {
            originSr.DOKill();
            overlaySr.DOKill();

            overlaySr.gameObject.SetActive(true);
            overlaySr.enabled = true;
            overlaySr.sprite = sprite;
            if (isCalculateScale)
                SpriteRendererUtil.CalculateScale(overlaySr);
            overlaySr.color = new Color(1, 1, 1, 0);

            Sequence seq = DOTween.Sequence();
            seq.Join(overlaySr.DOFade(1.0f, duration));
            seq.OnComplete(() =>
            {
                originSr.sprite = sprite;
                if (isCalculateScale)
                    SpriteRendererUtil.CalculateScale(originSr);
                originSr.color = Color.white;
                overlaySr.enabled = false;
                overlaySr.sprite = null;
                overlaySr.gameObject.SetActive(false);
                onCompleted?.Invoke();
            });

            AddSequence(overlaySr, seq, ImageDirectingType.Overlay);
        }

        public void VignetteIn(SpriteRenderer targetSr, Sprite sprite, Color color, float duration,
            bool isCalculateScale,
            Action onCompleted = null)
        {
            const string IntensityPropertyName = "_Intensity";

            targetSr.DOKill();
            targetSr.gameObject.SetActive(true);
            targetSr.enabled = true;
            targetSr.color = color;
            targetSr.sprite = sprite;
            if (isCalculateScale)
                SpriteRendererUtil.CalculateScale(targetSr);
            targetSr.material = _vignetteMat;
            targetSr.material.SetFloat(IntensityPropertyName, 0);

            if (isCalculateScale)
                SpriteRendererUtil.CalculateScale(targetSr);

            Sequence seq = DOTween.Sequence();
            seq.Join(targetSr.material.DOFloat(1, IntensityPropertyName, duration));
            seq.OnComplete(() =>
            {
                targetSr.material = _zSpriteMat;
                onCompleted?.Invoke();
            });

            AddSequence(targetSr, seq, ImageDirectingType.Vignette);
        }

        public void VignetteInOverlay(SpriteRenderer originSr, SpriteRenderer overlaySr, Sprite sprite, Color color,
            float duration, bool isCalculateScale, Action onCompleted = null)
        {
            onCompleted += () =>
            {
                originSr.sprite = sprite;
                if (isCalculateScale)
                    SpriteRendererUtil.CalculateScale(originSr);
                originSr.color = Color.white;
                overlaySr.enabled = false;
                overlaySr.sprite = null;
                overlaySr.material = _zSpriteMat;
                overlaySr.gameObject.SetActive(false);
            };
            VignetteIn(overlaySr, sprite, color, duration, isCalculateScale, onCompleted);
        }

        public void StopImageDirecting(SpriteRenderer targetSr, ImageDirectingType directingType)
        {
            if (_sequences.Count == 0)
                return;

            int characterID = targetSr.GetInstanceID();
            SequenceInfo sequenceInfo =
                _sequences.FirstOrDefault(seq => seq.ImageID == characterID && seq.DirectingType == directingType);

            if (sequenceInfo == null)
                return;

            // DOTween Sequence Kill
            Sequence sequence = sequenceInfo.Sequence;
            sequence.Kill();
            RemoveSequence(sequenceInfo);


            targetSr.color = Color.white;
        }


        public void StopImageAllDirecting(SpriteRenderer targetSr)
        {
            int imageID = targetSr.GetInstanceID();
            List<SequenceInfo> sequences = _sequences.Where(seq => seq.ImageID == imageID).ToList();
            foreach (SequenceInfo sequence in sequences)
            {
                ImageDirectingType directingType = sequence.DirectingType;
                StopImageDirecting(targetSr, directingType);
            }
        }

        public void StopAllDirecting()
        {
            foreach (SequenceInfo sequenceInfo in _sequences)
            {
                Sequence sequence = sequenceInfo.Sequence;
                sequence.Kill();
            }

            _sequences.Clear();
        }

        private void AddSequence(SpriteRenderer sr, Sequence seq, ImageDirectingType directingType)
        {
            int imageID = sr.GetInstanceID();
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
