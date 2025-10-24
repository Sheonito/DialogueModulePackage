using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Lucecita.HappinessBlossom.Directing
{
 public class CameraDirector
{
    #region Const Variables

    private const int blurFocalLength = 49;

    #endregion

    private Camera _cam;
    private Volume _volume;

    public void Init(Volume volume)
    {
        _cam = Camera.main;
        _volume = volume;
    }

    public void ResetDirector()
    {
        _volume.profile.TryGet(out DepthOfField depthOfField);
        if (depthOfField != null)
        {
            depthOfField.focalLength.overrideState = false;
            depthOfField.focalLength.value = 0;
            depthOfField.active = false;
        }

        _volume.profile.TryGet(out ColorAdjustments colorAdjustments);
        if (colorAdjustments != null)
        {
            colorAdjustments.colorFilter.value = Color.white;
            colorAdjustments.active = false;
            colorAdjustments.colorFilter.overrideState = false;
        }

        _volume.profile.TryGet(out Vignette vignette);
        if (vignette != null)
        {
            vignette.color.value = Color.white;
            vignette.intensity.value = 0;
            vignette.active = false;
            vignette.color.overrideState = false;
            vignette.intensity.overrideState = false;
        }

        _volume.profile.TryGet(out Bloom bloom);
        if (bloom != null)
        {
            bloom.intensity.value = 0;
            bloom.active = false;
            bloom.intensity.overrideState = false;
        }
    }

    public void SetBlur(bool isActive, float duration = 1, Action onCompleted = null)
    {
        int focalLength = isActive ? blurFocalLength : 1;

        _volume.profile.TryGet(out DepthOfField depthOfField);
        if (depthOfField == null)
            return;

        depthOfField.active = isActive;
        depthOfField.focalLength.overrideState = true;
        depthOfField.focalLength.value = 0;
        DOTween.To(() => depthOfField.focalLength.value,
            value => depthOfField.focalLength.value = value,
            focalLength,
            duration).onComplete += () => onCompleted?.Invoke();
    }

    public void SetColorFilter(bool isActive, Color color, float duration = 1, Action onCompleted = null)
    {
        _volume.profile.TryGet(out ColorAdjustments colorAdjustments);
        if (colorAdjustments == null)
            return;

        if (isActive)
        {
            colorAdjustments.active = true;
            colorAdjustments.colorFilter.overrideState = true;
        }
        else
        {
            onCompleted += () => colorAdjustments.active = false;
            onCompleted += () => colorAdjustments.colorFilter.overrideState = false;
        }

        DOTween.To(() => colorAdjustments.colorFilter.value,
            value => colorAdjustments.colorFilter.value = value,
            color,
            duration).onComplete += () => onCompleted?.Invoke();
    }

    public Tween SetVignette(bool isActive, Color color, float intensity, float duration = 1, Action onCompleted = null)
    {
        _volume.profile.TryGet(out Vignette vignette);
        if (vignette == null)
            return null;

        if (isActive)
        {
            vignette.active = true;
            vignette.color.overrideState = true;
            vignette.intensity.overrideState = true;

            vignette.intensity.value = 0;
            vignette.color.value = color;
        }
        else
        {
            onCompleted += () => vignette.active = false;
            onCompleted += () => vignette.color.overrideState = false;
            onCompleted += () => vignette.intensity.overrideState = false;
        }

        Tween _vignetteTween = DOTween.To(() => vignette.intensity.value,
            value => vignette.intensity.value = value,
            intensity,
            duration);

        _vignetteTween.onComplete += () => onCompleted?.Invoke();

        return _vignetteTween;
    }

    public Tween SetBloom(bool isActive, float intensity, float duration = 1, Action onCompleted = null)
    {
        _volume.profile.TryGet(out Bloom bloom);
        if (bloom == null)
            return null;

        if (isActive)
        {
            bloom.active = true;
            bloom.intensity.overrideState = true;

            bloom.intensity.value = 0;
        }
        else
        {
            onCompleted += () => bloom.active = false;
            onCompleted += () => bloom.intensity.overrideState = false;
        }

        Tween _bloomTween = DOTween.To(() => bloom.intensity.value,
            value => bloom.intensity.value = value,
            intensity,
            duration);

        _bloomTween.onComplete += () => onCompleted?.Invoke();

        return _bloomTween;
    }

    public Tween BloomYoYo(float intensity, float duration = 1, Action onCompleted = null)
    {
        _volume.profile.TryGet(out Bloom bloom);
        if (bloom == null)
            return null;

        bloom.active = true;
        bloom.intensity.overrideState = true;
        bloom.intensity.value = 0;

        Tween _bloomTween = DOTween.To(() => bloom.intensity.value,
                value => bloom.intensity.value = value,
                intensity,
                duration / 2f
            )
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .OnComplete(() => onCompleted?.Invoke());

        return _bloomTween;
    }
}   
}