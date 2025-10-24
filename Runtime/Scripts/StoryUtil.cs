using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Lucecita.StorylineEngine
{
    public class StoryUtil
    {
        public static string Absolute_EnginePath = $"{Application.dataPath}/StorylineEngine";
        public static string Relative_EnginePath = $"Assets/StorylineEngine";

        public static string GetEpisodeCSVPath()
        {
            string path = $"{Relative_EnginePath}/Resource/Story/";
            if (!Directory.Exists(path))
            {
                Debug.Log($"Move episode files to \"{path}\"");
                Directory.CreateDirectory(path);
            }

            return path;
        }

        public static Dictionary<Type, T> CreateAllDerivedInstances<T>() where T : class
        {
            var interfaceType = typeof(T);
            var derivedTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsClass && !type.IsAbstract && interfaceType.IsAssignableFrom(type))
                .ToList();

            Dictionary<Type, T> instances = new Dictionary<Type, T>();

            foreach (var type in derivedTypes)
            {
                bool isParentType = derivedTypes.Any(t => t != type && type.IsAssignableFrom(t));
                bool isOpenGenericType = type.ContainsGenericParameters;

                if (!isParentType && !isOpenGenericType)
                {
                    if (Activator.CreateInstance(type) is T instance)
                    {
                        instances.Add(type, instance);
                    }
                }
            }

            return instances;
        }
        
        public static string CombinePath(params string[] parts)
        {
            return string.Join("/", parts);
        }

        public static bool ExistVoiceFile(string path)
        {
#if UNITY_EDITOR
            string[] extensions = { ".wav", ".mp3", ".ogg" };
            foreach (string ext in extensions)
            {
                if (AssetDatabase.AssetPathExists(path + ext))
                    return true;
            }
#endif
            return false;
        }
    }
}