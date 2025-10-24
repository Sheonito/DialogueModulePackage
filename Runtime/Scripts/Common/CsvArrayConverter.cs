using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Globalization;
using System.Linq;

namespace Aftertime.StorylineEngine
{
    public class CsvArrayConverter<T> : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrWhiteSpace(text))
                return Array.Empty<T>();

            // 쉼표 기준 Split → Trim → 타입 변환
            return text
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => (T)Convert.ChangeType(s.Trim(), typeof(T), CultureInfo.InvariantCulture))
                .ToArray();
        }
    }   
}