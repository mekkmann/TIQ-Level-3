using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MonsterQuest
{
    public static class EnumerableHelper
    {
        private static readonly System.Random _random = new();

        public static T Random<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.ElementAt(_random.Next(0, enumerable.Count()));
        }
    }
}
