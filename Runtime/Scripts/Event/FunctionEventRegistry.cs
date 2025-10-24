using System;
using System.Collections.Generic;

namespace Lucecita.StorylineEngine
{
    public static class FunctionEventRegistry
    {
        private static readonly Dictionary<Type, Function> _functions = new();

        public static void Init(IEnumerable<Function> functions)
        {
            _functions.Clear();
            foreach (var function in functions)
            {
                if (function == null)
                    continue;

                _functions[function.GetType()] = function;
            }
        }
        

        public static void Subscribe<T>(Action<T> subscribe) where T : Function
        {
            if (subscribe == null)
                return;
            
            T function = GetFunction<T>();
            subscribe(function);
        }

        public static void Unsubscribe<T>(Action<T> unsubscribe) where T : Function
        {
            if (unsubscribe == null)
                return;
            
            T function = GetFunction<T>();
            unsubscribe(function);
        }
        
        private static T GetFunction<T>() where T : Function
        {
            if (!_functions.TryGetValue(typeof(T), out var function))
                throw new InvalidOperationException($"Function {typeof(T).Name} is not registered.");

            return (T)function;
        }
    }
}
