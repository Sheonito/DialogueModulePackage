using Lucecita;
using UnityEditor;
using UnityEditor.UI;

namespace Lucecita.SecretSome
{
    [CustomEditor(typeof(ChoiceButton))]
    [CanEditMultipleObjects]
    public class ChoiceButtonEditor : ButtonEditor
    {
        private SerializedProperty _bgImage;

        private SerializedProperty _defaultBGColor;

        private SerializedProperty _hoverBGColor;

        private SerializedProperty _textMeshPro;
        private SerializedProperty _showScale;
        private SerializedProperty _showDuration;
        
        private SerializedProperty _canvasGroup;
        private SerializedProperty _rectTransform;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            _bgImage = serializedObject.FindProperty("_bgImage"); 
            _canvasGroup = serializedObject.FindProperty("_canvasGroup");
            _rectTransform = serializedObject.FindProperty("_rectTransform");
            
            _defaultBGColor = serializedObject.FindProperty("_defaultBGColor");
            
            _hoverBGColor = serializedObject.FindProperty("_hoverBGColor");
            
            _textMeshPro = serializedObject.FindProperty("_textMeshPro");
            _showScale = serializedObject.FindProperty("_showScale");
            _showDuration = serializedObject.FindProperty("_showDuration");
        }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            EditorGUILayout.PropertyField(_bgImage);
            EditorGUILayout.PropertyField(_canvasGroup);
            EditorGUILayout.PropertyField(_rectTransform);
            
            EditorGUILayout.PropertyField(_defaultBGColor);
            
            EditorGUILayout.PropertyField(_hoverBGColor);
            
            EditorGUILayout.PropertyField(_textMeshPro);
            EditorGUILayout.PropertyField(_showScale);
            EditorGUILayout.PropertyField(_showDuration);

            serializedObject.ApplyModifiedProperties();
        }
    }
}