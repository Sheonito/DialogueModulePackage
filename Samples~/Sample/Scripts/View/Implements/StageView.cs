using Lucecita.HappinessBlossom.Stage;
using UnityEngine;

namespace Lucecita.HappinessBlossom
{
    public class StageView : MonoBehaviour
    {
        public static readonly Vector3 DefaultBGPos = new Vector3(0, 0, 10);
        public static readonly Vector3 DefaultBGScale = new Vector3(-1, -1, -1);
        public static readonly Vector3 DefaultECGPos = new Vector3(0, 0, 10);
        public static readonly Vector3 DefaultECGScale = new Vector3(-1, -1, -1);
        public static readonly Vector3 DefaultCameraPos = new Vector3(0, 0, -9.42f);
        public static readonly Vector3 DefaultStandingPos = new Vector3(0, -5.6f, 0);
        public static readonly Vector3 DefaultStandingScale = new Vector3(0.37f, 0.37f, 0.37f);

        public CharacterStage CharacterStage => _characterStage;
        public StageSpriteRenderer Bg => _bg;
        public StageSpriteRenderer Ecg => _ecg;

        [SerializeField] private CharacterStage _characterStage;
        [SerializeField] private StageSpriteRenderer _bg;
        [SerializeField] private StageSpriteRenderer _ecg;
    }
   
}