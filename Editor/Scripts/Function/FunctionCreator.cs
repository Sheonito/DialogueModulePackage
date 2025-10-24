using System.IO;
using UnityEditor;
using UnityEngine;

namespace Lucecita.StorylineEngine
{
    public static class FunctionCreator
    {
        private const string FunctionScriptTemplate = 
@"using Lucecita.StorylineEngine;
public class {0} : Function, IElementChangeFunc
{{
    public TextElement GetNextElement()
    {{
        return null;
    }}
}}";
        
        private const string TypedFunctionScriptTemplate = 
@"using Lucecita.StorylineEngine;

public class {0} : TypedFunction<{0}Value>, IElementChangeFunc
{{
    public TextElement GetNextElement()
    {{
        return null;
    }}
}}

public enum {0}Value
{{
    
}}";
        
        [MenuItem("Assets/Create/StorylineEngine/Function Script", false, 81)]
        public static void CreateFunction()
        {
            string path = GetSelectedPathOrFallback();

            // 기본 이름
            string fileName = "NewFunction.cs";
            path = Path.Combine(path, fileName);

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
                0,
                ScriptableObject.CreateInstance<DoCreateFunction>(),
                path,
                EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D,
                null
            );
        }
        

        [MenuItem("Assets/Create/StorylineEngine/TypedFunction Script", false, 82)]
        public static void CreateTypedFunction()
        {
            string path = GetSelectedPathOrFallback();

            // 기본 이름
            string fileName = "NewTypedFunction.cs";
            path = Path.Combine(path, fileName);

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
                0,
                ScriptableObject.CreateInstance<DoCreateTypedFunction>(),
                path,
                EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D,
                null
            );
        }

        private static string GetSelectedPathOrFallback()
        {
            string path = "Assets";
            foreach (Object obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
            {
                path = AssetDatabase.GetAssetPath(obj);
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    path = Path.GetDirectoryName(path);
                }

                break;
            }

            return path;
        }
        
        private class DoCreateFunction : UnityEditor.ProjectWindowCallback.EndNameEditAction
        {
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                string className = Path.GetFileNameWithoutExtension(pathName);

                string scriptContent = string.Format(FunctionScriptTemplate, className);

                File.WriteAllText(pathName, scriptContent);
                AssetDatabase.ImportAsset(pathName);
                Object obj = AssetDatabase.LoadAssetAtPath<Object>(pathName);
                ProjectWindowUtil.ShowCreatedAsset(obj);
            }
        }

        private class DoCreateTypedFunction : UnityEditor.ProjectWindowCallback.EndNameEditAction
        {
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                string className = Path.GetFileNameWithoutExtension(pathName);

                string scriptContent = string.Format(TypedFunctionScriptTemplate, className);

                File.WriteAllText(pathName, scriptContent);
                AssetDatabase.ImportAsset(pathName);
                Object obj = AssetDatabase.LoadAssetAtPath<Object>(pathName);
                ProjectWindowUtil.ShowCreatedAsset(obj);
            }
        }
        
    }
}
