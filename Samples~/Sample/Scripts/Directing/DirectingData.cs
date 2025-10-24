using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Timeline;
using UnityEngine.UI;

namespace Lucecita.HappinessBlossom.Directing
{
    [System.Serializable]
    public class DirectingData
    {
        [SerializeField] private List<TimelineAsset> _timelineAssets;
        [SerializeField] private Material _vignetteMaterial;
        [SerializeField] private Material _vignetteBlinkMaterial;
        [SerializeField] private Material _zSpriteMat;
        [SerializeField] private Canvas _transitionCanvas;
        [SerializeField] private RawImage _customScreen;
        [SerializeField] private RawImage _screen;
        [SerializeField] private Volume _volume;
        [SerializeField] private PlayableDirector _playableDirector;

        public IReadOnlyList<TimelineAsset> TimelineAssets => _timelineAssets;
        public Material VignetteMaterial => _vignetteMaterial;
        public Material VignetteBlinkMaterial => _vignetteBlinkMaterial;
        public Material ZSpriteMat => _zSpriteMat;
        public Canvas TransitionCanvas => _transitionCanvas;
        public RawImage CustomScreen => _customScreen;
        public RawImage Screen => _screen;
        public Volume Volume => _volume;
        public PlayableDirector PlayableDirector => _playableDirector;
    }   
}