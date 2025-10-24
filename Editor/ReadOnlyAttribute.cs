using UnityEditor;
using UnityEngine;

namespace Aftertime.StorylineEngine
{
    [CustomPropertyDrawer(typeof(global::ReadOnlyAttribute), true)]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var readOnly = (global::ReadOnlyAttribute)attribute;
            bool disabled = true;

            switch (readOnly.runtimeOnly)
            {
                case EReadOnlyType.FULLY_DISABLED:
                    disabled = true;
                    break;
                case EReadOnlyType.EDITABLE_RUNTIME:
                    disabled = !Application.isPlaying;
                    break;
                case EReadOnlyType.EDITABLE_EDITOR:
                    disabled = Application.isPlaying;
                    break;
            }

            using (new EditorGUI.DisabledGroupScope(disabled))
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }
    }

    [CustomPropertyDrawer(typeof(global::BeginReadOnlyAttribute))]
    public class BeginReadOnlyGroupDrawer : DecoratorDrawer
    {
        public override float GetHeight() => 0f;

        public override void OnGUI(Rect position)
        {
            EditorGUI.BeginDisabledGroup(true);
        }
    }

    [CustomPropertyDrawer(typeof(global::EndReadOnlyAttribute))]
    public class EndReadOnlyGroupDrawer : DecoratorDrawer
    {
        public override float GetHeight() => 0f;

        public override void OnGUI(Rect position)
        {
            EditorGUI.EndDisabledGroup();
        }
    }
}