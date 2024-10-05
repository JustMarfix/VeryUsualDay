using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Random = UnityEngine.Random;

namespace VeryUsualDay
{
    public static class Helpers
    {
        private static string GetCustomDescription(object objEnum)
        {
            var fi = objEnum.GetType().GetField(objEnum.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : objEnum.ToString();
        }

        public static string Description(this Enum value)
        {
            return GetCustomDescription(value);
        }
        
        public static bool In<T>(this T val, params T[] vals) => vals.Contains(val);

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable)
        {
            var enumerable1 = enumerable.ToList();
            var newEnum = Enumerable.Empty<T>();
            while (enumerable1.Count != 0)
            {
                var index = Random.Range(0, enumerable1.Count);
                newEnum = newEnum.Append(enumerable1[index]);
                enumerable1.RemoveAt(index);
            }
            return newEnum;
        }
    }
}