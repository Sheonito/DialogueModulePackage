using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Lucecita.StorylineEngine
{
    public static class PresenterInjector
    {
        private static readonly Dictionary<Type, List<MemberInfo>> _cache = new();

        public static void Bind(object target)
        {
            if (target == null) return;
            var type = target.GetType();
            if (!_cache.TryGetValue(type, out var members))
            {
                members = CollectMembers(type);
                _cache[type] = members;
            }

            foreach (var member in members)
            {
                var memberType = GetMemberType(member);
                if (!typeof(IPresenter).IsAssignableFrom(memberType))
                {
                    Debug.LogWarning($"[PresenterInjector] {type.Name}.{member.Name} 타입 {memberType.Name}은(는) IPresenter가 아닙니다. 주입 대상에서 제외합니다.");
                    continue;
                }

                var presenter = PresenterRegistry.Resolve(memberType);
                if (presenter == null)
                {
                    Debug.LogWarning($"[PresenterInjector] {memberType.Name} 등록이 없습니다. {type.Name}.{member.Name} 주입 실패.");
                    continue;
                }
                SetMemberValue(target, member, presenter);
            }
        }

        private static List<MemberInfo> CollectMembers(Type type)
        {
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var list = new List<MemberInfo>();
            foreach (var f in type.GetFields(flags))
            {
                if (Attribute.IsDefined(f, typeof(PresenterAttribute)))
                    list.Add(f);
            }
            foreach (var p in type.GetProperties(flags))
            {
                if (!p.CanWrite) continue;
                if (Attribute.IsDefined(p, typeof(PresenterAttribute)))
                    list.Add(p);
            }
            return list;
        }

        private static Type GetMemberType(MemberInfo m)
        {
            return m switch
            {
                FieldInfo f => f.FieldType,
                PropertyInfo p => p.PropertyType,
                _ => typeof(void)
            };
        }

        private static void SetMemberValue(object target, MemberInfo member, object value)
        {
            switch (member)
            {
                case FieldInfo f:
                    f.SetValue(target, value);
                    break;
                case PropertyInfo p:
                    p.SetValue(target, value);
                    break;
            }
        }
    }
}

