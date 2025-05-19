using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Exiled.API.Features;
using Random = UnityEngine.Random;

namespace VeryUsualDay
{
    public static class Helpers
    {
        public static TimeSpan ConvertToTimeSpan(string timeSpan)
        {
            var l = timeSpan.Length - 1;
            var value = timeSpan.Substring(0, l);
            var type = timeSpan.Substring(l, 1);

            switch (type)
            {
                case "d": return TimeSpan.FromDays(double.Parse(value));
                case "h": return TimeSpan.FromHours(double.Parse(value));
                case "m": return TimeSpan.FromMinutes(double.Parse(value));
                case "s": return TimeSpan.FromSeconds(double.Parse(value));
                default: return TimeSpan.FromSeconds(double.Parse(value));
            }
        }
        
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

        /// <summary>
        /// Проверяет, является ли игрок SCP, включая кастомные SCP-классы.
        /// </summary>
        /// <returns></returns>
        public static bool IsScp(this Player player)
        {
            if (player == null) return false;
            return VeryUsualDay.Instance.ScpPlayers.ContainsKey(player.Id) || player.IsScp;
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