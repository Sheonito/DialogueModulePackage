using System.Linq;
using Aftertime.StorylineEngine;
using UnityEngine;

namespace Aftertime.HappinessBlossom.Directing
{
    public class DirectingManager : MonoBehaviour
    {
        [SerializeField] private TransitionDirector _transitionDirector;
        [SerializeField] private DirectingData _data;
    
        public static TransitionDirector TransitionDirector { get; private set; }
        public static ImageDirector ImageDirector { get; private set; }
        public static SpriteRendererDirector SpriteRendererDirector { get; private set; }
        public static CameraDirector CameraDirector { get; private set; }
        public static TimelineDirector TimelineDirector { get; private set; }

        private void Awake()
        {
            // 트랜지션 디렉터 초기화
            TransitionDirector = _transitionDirector;
            TransitionDirector.Init(_data.VignetteBlinkMaterial);

            // 이미지 디렉터 초기화
            ImageDirector = new ImageDirector();
            ImageDirector.Init(_data.Screen,_data.CustomScreen,_data.VignetteMaterial);

            // 스프라이트 렌더러 디렉터 초기화
            SpriteRendererDirector = new SpriteRendererDirector();
            SpriteRendererDirector.Init(_data.VignetteMaterial,_data.ZSpriteMat);
        
            // 카메라 디렉터 초기화
            CameraDirector = new CameraDirector();
            CameraDirector.Init(_data.Volume);
        
            // 타임라인 디렉터 초기화
            TimelineDirector = new TimelineDirector(); 
            TimelineDirector.Init(_data.PlayableDirector,_data.TimelineAssets.ToList());
        }

        public static void ResetDirectors()
        {
            CameraDirector.ResetDirector();
        }
    }   
}