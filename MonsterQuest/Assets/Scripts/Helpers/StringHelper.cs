using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MonsterQuest
{
    public static class StringHelper
    {
        public static string JoinWithAnd(IEnumerable<string> items, bool useSerialComma = true)
        {
            int itemCount = items.Count();

            if (itemCount == 0) return "";
            if (itemCount == 1) return items.First();
            if (itemCount == 2) return string.Join(" and ", items);

            List<string> charactersCopy = new(items);

            if (useSerialComma)
            {
                charactersCopy[itemCount - 1] = $"and {charactersCopy[itemCount - 1]}";
            }
            else
            {
                charactersCopy[itemCount - 2] = $"{charactersCopy[itemCount - 2]} and {charactersCopy[itemCount - 1]}";
                charactersCopy.RemoveAt(itemCount - 1);
            }

            return string.Join(", ", charactersCopy);
        }

        public static string ToUpperFirst(this string value)
        {
            return value[0].ToString().ToUpper() + value[1..];
        }
    
    }
}
