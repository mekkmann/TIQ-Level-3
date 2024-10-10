using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public static class ListHelper
    {
        public static System.Random Random = new();
        public static List<Creature> Shuffle(List<Creature> toShuffle)
        {
         
            List<Creature> shuffled = new(toShuffle);

            int copyCount = shuffled.Count;

            for (int i = copyCount - 1; i > 0; i--)
            {
                int j = Random.Next(0, copyCount--);
                (shuffled[j], shuffled[i]) = (shuffled[i], shuffled[j]);
            }

            return shuffled;
        }
    }
}
