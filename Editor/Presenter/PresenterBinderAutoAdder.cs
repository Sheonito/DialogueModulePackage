using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Aftertime.StorylineEngine.Editor
{
    [InitializeOnLoad]
    public static class PresenterBinderAutoAdder
    {
        static PresenterBinderAutoAdder()
        {
            EditorApplication.hierarchyChanged += OnHierarchyChanged;
            EditorSceneManager.sceneOpened += (scene, mode) => EnsureBindersInScene(scene);
        }

        private static void OnHierarchyChanged()
        {
            // 가벼운 방식: 현재 활성 씬만 검사
            var scene = SceneManager.GetActiveScene();
            EnsureBindersInScene(scene);
        }

        private static void EnsureBindersInScene(Scene scene)
        {
            if (!scene.IsValid() || !scene.isLoaded) return;
            foreach (var go in scene.GetRootGameObjects())
            {
                EnsureBindersInHierarchy(go);
            }
        }

        private static void EnsureBindersInHierarchy(GameObject root)
        {
            var all = root.GetComponentsInChildren<Transform>(true).Select(t => t.gameObject);
            foreach (var go in all)
            {
                // 이미 바인더가 있으면 스킵
                if (go.GetComponent<PresenterBinder>() != null) continue;

                // 같은 GO에 IPresenter를 구현한 컴포넌트가 있으면 바인더 추가
                var hasPresenter = go.GetComponents<MonoBehaviour>().Any(m => m is IPresenter);
                if (hasPresenter)
                {
                    Undo.AddComponent<PresenterBinder>(go);
                    EditorUtility.SetDirty(go);
                }
            }
        }
    }
}

