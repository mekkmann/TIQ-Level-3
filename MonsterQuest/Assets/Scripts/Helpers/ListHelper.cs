using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public static class ListHelper
    {
        public static System.Random Random = new();

        // generic list shuffler
        // returns a new list
        // Use: ListHelper.Shuffle1([whateverList])
        public static List<T> GenericShuffle<T>(IList<T> toShuffle)
        {
         
            List<T> shuffled = new(toShuffle);

            int copyCount = shuffled.Count;

            for (int i = copyCount - 1; i > 0; i--)
            {
                int j = Random.Next(0, copyCount--);
                (shuffled[j], shuffled[i]) = (shuffled[i], shuffled[j]);
            }

            return shuffled;
        }

        // extension list shuffler
        // modifies the list
        // Use: whateverList.Shuffle()
        public static List<T> ExtensionShuffle<T>(this IList<T> toShuffle)
        {
            int copyCount = toShuffle.Count;

            for (int i = copyCount - 1; i > 0; i--)
            {
                int j = Random.Next(0, copyCount--);
                (toShuffle[j], toShuffle[i]) = (toShuffle[i], toShuffle[j]);
            }

            return (List<T>)toShuffle;
        }
    }
}
