using System;
using System.Globalization;

namespace Lucecita.StorylineEngine
{
    public readonly struct FunctionValue
    {
        public int Length => _values.Length;
        private readonly string[] _values;

        public FunctionValue(string[] values)
        {
            _values = values ?? Array.Empty<string>();
        }


        public T Get<T>(Enum value)
        {
            int index = Convert.ToInt32(value);
            string rawValue = (index >= 0 && index < _values.Length) ? _values[index] : null;
            return ConvertTo<T>(rawValue);
        }

        private static T ConvertTo<T>(string s)
        {
            var target = typeof(T);
            if (target == typeof(object)) return (T)(object)s;
            if (target == typeof(string))
                return (T)(object)s;

            if (string.IsNullOrEmpty(s))
                return default;

            try
            {
                if (target == typeof(int))
                    if (int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out var i)) return (T)(object)i; else return default;
                if (target == typeof(float))
                    if (float.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var f)) return (T)(object)f; else return default;
                if (target == typeof(double))
                    if (double.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var d)) return (T)(object)d; else return default;
                if (target == typeof(bool))
                {
                    if (bool.TryParse(s, out var b)) return (T)(object)b;
                    if (int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out var bi)) return (T)(object)(bi != 0);
                    return default;
                }
            }
            catch
            {
                return default;
            }

            return default;
        }

        public bool HasValue(Enum value)
        {
            if (value == null) 
                return false;
            
            int index = Convert.ToInt32(value);
            if (index < 0 || index >= _values.Length) 
                return false;
            
            string rawValue = _values[index];
            if (string.IsNullOrEmpty(rawValue)) 
                return false;

            return true;
        }
    }
}



