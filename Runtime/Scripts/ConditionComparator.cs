using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DynamicExpresso;
using UnityEngine;

namespace Lucecita.StorylineEngine
{
    public static class ConditionComparator
    {
        private static Interpreter _interpreter;
        private static Dictionary<string, object> _conditions;

        // If Function의 비교문 비교
        public static bool CompareCondition(string condition)
        {
            if (string.IsNullOrEmpty(condition) || string.IsNullOrWhiteSpace(condition))
                return false;

            // 공백 제거
            condition = condition.Replace(" ", "");
            // true,false 소문자로 변경
            condition = condition.Replace("False", "false");
            condition = condition.Replace("True", "true");
            // 조건에 . 제거
            condition = Regex.Replace(condition, @"(\b\w+)\.", "$1");  

            if (_interpreter == null)
            {
                Init();
            }

            // DynamicExpresso 라이브러리를 사용하여 string 값을 연산
            try
            {
                bool result = (bool)_interpreter.Eval(condition);
                return result;
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return false;
            }
        }

        public static void SetCondition(string key, object value)
        {
            if (_conditions.ContainsKey(key))
            {
                _conditions.Remove(key);
            }

            if (value.ToString() == "true")
            {
                value = true;
            }
            else if (value.ToString() == "false")
            {
                value = false;
            }

            _conditions.Add(key, value);
            key = key.Replace(" ", "");
            
            bool parseResult = int.TryParse(key, out int _);
            if (!parseResult)
            {
                key = key.Replace(".", "");
            }
            
            try
            {
                _interpreter.SetVariable(key, value);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        public static void Init()
        {
            _interpreter = new Interpreter();
            _conditions = new Dictionary<string, object>();
        }
    }
}