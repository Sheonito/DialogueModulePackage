using System;
using System.Collections.Generic;
using Lucecita.StorylineEngine;
using DG.Tweening;
using TransitionsPlus;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using RenderMode = TransitionsPlus.RenderMode;

public enum TransitionRenderMode
{
    InsideUI,
    FullScreen
}

[RequireComponent(typeof(Canvas))]
public class TransitionDirector : TransitionAnimator
{
    [SerializeField] private List<TransitionProfile> _profiles;

    [SerializeField] private RawImage _fullScreen;
    [SerializeField] private RawImage _customScreen;
    private Canvas _canvas;


    private bool _isInit;

    private Material _vignetteBlinkMat;

    public void Init(Material vignetteBlinkMat)
    {
        if (_isInit)
            return;

        screen = _fullScreen;
        customScreen = _customScreen;
        _canvas = GetComponent<Canvas>();
        onTransitionEnd = new UnityEvent();

        _vignetteBlinkMat = vignetteBlinkMat;

        _isInit = true;
    }

    public void OffTransitionScreen()
    {
        _fullScreen.DOKill();
        _customScreen.DOKill();
        _fullScreen.color = new Color(1, 1, 1, 0);
        _customScreen.color = new Color(1, 1, 1, 0);
    }

    public void FadeIn(float duration, TransitionRenderMode _renderMode = TransitionRenderMode.InsideUI,
        Action onCompleted = null)
    {
        if (_renderMode == TransitionRenderMode.InsideUI)
        {
            _customScreen.DOKill();
            _customScreen.color = Color.black;
            _customScreen.DOFade(0, duration);
        }
        else
        {
            _canvas.enabled = true;
            _fullScreen.DOKill();
            _fullScreen.material = null;
            _fullScreen.color = Color.black;
            _fullScreen.DOFade(0, duration);
        }
    }

    public void FadeOut(float duration, TransitionRenderMode _renderMode = TransitionRenderMode.InsideUI,
        Action onCompleted = null)
    {
        if (_renderMode == TransitionRenderMode.InsideUI)
        {
            _customScreen.DOKill();
            _customScreen.color = new Color(0, 0, 0, 0);
            _customScreen.DOFade(1, duration);
        }
        else
        {
            _canvas.enabled = true;
            _fullScreen.DOKill();
            _fullScreen.material = null;
            _fullScreen.color = new Color(0, 0, 0, 0);
            _fullScreen.DOFade(1, duration);
        }
    }

    public void WhiteIn(float duration, TransitionRenderMode _renderMode = TransitionRenderMode.InsideUI,
        Action onCompleted = null)
    {
    }

    public void WhiteOut(float duration, TransitionRenderMode _renderMode = TransitionRenderMode.InsideUI,
        Action onCompleted = null)
    {
        if (_renderMode == TransitionRenderMode.InsideUI)
        {
            _customScreen.DOKill();
            _customScreen.color = new Color(1, 1, 1, 0);
            _customScreen.DOFade(1, duration);
        }
        else
        {
            _canvas.enabled = true;
            _fullScreen.DOKill();
            _fullScreen.material = null;
            _fullScreen.color = new Color(1, 1, 1, 0);
            _fullScreen.DOFade(1, duration);
        }
    }

    public void StarRotateIn(float duration, RenderMode _renderMode = RenderMode.InsideUI, Action onCompleted = null)
    {
        profile = FindProfile(TransitionDetailType.StarRotate);
        profile.invert = true;
        profile.color = Color.black;

        StartTransition(_renderMode, duration, onCompleted);
    }

    public void StarRotateOut(float duration, RenderMode _renderMode = RenderMode.InsideUI, Action onCompleted = null)
    {
        profile = FindProfile(TransitionDetailType.StarRotate);
        profile.invert = false;
        profile.color = Color.black;

        StartTransition(_renderMode, duration, onCompleted);
    }
    
    public void WipeLeftOut(float duration, RenderMode _renderMode = RenderMode.InsideUI, Action onCompleted = null)
    {
        profile = FindProfile(TransitionDetailType.WipeRight);
        profile.invert = false;
        profile.color = Color.black;

        StartTransition(_renderMode, duration, onCompleted);
    }

    public void WipeRightIn(float duration, RenderMode _renderMode = RenderMode.InsideUI, Action onCompleted = null)
    {
        profile = FindProfile(TransitionDetailType.WipeRight);
        profile.invert = true;
        profile.color = Color.black;

        StartTransition(_renderMode, duration, onCompleted);
    }

    public void FlashOut(float duration, RenderMode _renderMode = RenderMode.InsideUI, Action onCompleted = null)
    {
        throw new NotImplementedException();
    }

    public void FlashIn(float duration, RenderMode _renderMode = RenderMode.InsideUI, Action onCompleted = null)
    {
        throw new NotImplementedException();
    }

    public void WipeDoubleVertical(float duration, RenderMode _renderMode = RenderMode.InsideUI,
        Action onCompleted = null)
    {
        throw new NotImplementedException();
    }

    public void FadeOutEdge(float duration, RenderMode _renderMode = RenderMode.InsideUI, Action onCompleted = null)
    {
        throw new NotImplementedException();
    }

    public void Dissolve(float duration, RenderMode _renderMode = RenderMode.InsideUI, Action onCompleted = null)
    {
        throw new NotImplementedException();
    }

    public void WipeDownUp(float duration, RenderMode _renderMode = RenderMode.InsideUI, Action onCompleted = null)
    {
        throw new NotImplementedException();
    }

    public void WipeUp(float duration, RenderMode _renderMode = RenderMode.InsideUI, Action onCompleted = null)
    {
        throw new NotImplementedException();
    }

    public void WipeDown(float duration, RenderMode _renderMode = RenderMode.InsideUI, Action onCompleted = null)
    {
        throw new NotImplementedException();
    }


    public void EyesOpen(float duration, RenderMode _renderMode = RenderMode.InsideUI, Action onCompleted = null)
    {
        const string IntensityPropertyName = "_CustomTime";


        if (_renderMode == RenderMode.InsideUI)
        {
            _customScreen.color = Color.black;

            _customScreen.material = _vignetteBlinkMat;
            _customScreen.material.DOKill();
            _customScreen.material.SetFloat(IntensityPropertyName, 1f);
            _customScreen.material.DOFloat(-0.2f, IntensityPropertyName, duration);
        }
        else
        {
            _fullScreen.color = Color.black;
            
            _canvas.enabled = true;
            _fullScreen.material = _vignetteBlinkMat;
            _fullScreen.material.DOKill();
            _fullScreen.material.SetFloat(IntensityPropertyName, 1f);
            _fullScreen.material.DOFloat(-0.2f, IntensityPropertyName, duration);
        }
    }

    public void EyesClose(float duration, RenderMode _renderMode = RenderMode.InsideUI, Action onCompleted = null)
    {
        const string IntensityPropertyName = "_CustomTime";

        if (_renderMode == RenderMode.InsideUI)
        {
            _customScreen.color = Color.black;

            _customScreen.material = _vignetteBlinkMat;
            _customScreen.material.DOKill();
            _customScreen.material.SetFloat(IntensityPropertyName, -0.2f);
            _customScreen.material.DOFloat(1, IntensityPropertyName, duration);
        }
        else
        {
            _fullScreen.color = Color.black;

            _canvas.enabled = true;
            _fullScreen.material = _vignetteBlinkMat;
            _fullScreen.material.DOKill();
            _fullScreen.material.SetFloat(IntensityPropertyName, -0.2f);
            _fullScreen.material.DOFloat(1, IntensityPropertyName, duration);
        }
    }
    
    public void VignetteCircleOut(float duration, RenderMode _renderMode = RenderMode.InsideUI, Action onCompleted = null)
    {
        profile = FindProfile(TransitionDetailType.CircleVignette);
        profile.invert = false;
        profile.color = Color.black;

        StartTransition(_renderMode, duration, onCompleted);
    }
    
    public void VignetteCircleIn(float duration, RenderMode _renderMode = RenderMode.InsideUI, Action onCompleted = null)
    {
        profile = FindProfile(TransitionDetailType.CircleVignette);
        profile.invert = true;
        profile.color = Color.black;

        StartTransition(_renderMode, duration, onCompleted);
    }
    
    public void VignetteCustomOut(float duration, RenderMode _renderMode = RenderMode.InsideUI, Action onCompleted = null)
    {
        profile = FindProfile(TransitionDetailType.CustomVignette);
        profile.invert = false;
        profile.color = Color.black;

        StartTransition(_renderMode, duration, onCompleted);
    }
    
    public void VignetteCustomIn(float duration, RenderMode _renderMode = RenderMode.InsideUI, Action onCompleted = null)
    {
        profile = FindProfile(TransitionDetailType.CustomVignette);
        profile.invert = true;
        profile.color = Color.black;

        StartTransition(_renderMode, duration, onCompleted);
    }



    public void StopTransition()
    {
        profile = null;
    }

    private TransitionProfile FindProfile(TransitionDetailType type)
    {
        TransitionProfile profile = _profiles.Find(profile => profile.detailType == type);
        return profile;
    }

    private void StartTransition(RenderMode _renderMode, float duration, Action onCompleted = null)
    {
        profile.duration = duration;
        renderMode = _renderMode;

        if (renderMode == RenderMode.InsideUI)
        {
            screen = customScreen;
        }
        else
        {
            screen = _fullScreen;
        }

        screen.color = new Color(1, 1, 1, 1);

        UpdateMaterialProperties();

        onTransitionEnd.RemoveAllListeners();
        onTransitionEnd.AddListener(() => onCompleted?.Invoke());

        SetProgress(0);
        SetProfile(profile);
        base.StartTransition();
    }
}