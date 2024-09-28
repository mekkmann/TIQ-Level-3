using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public class ItemType : ScriptableObject
    {
        public string DisplayName;
        public int Weight;

        // CONSTRUCTORS
        public ItemType(string displayName, int weight)
        {
            DisplayName = displayName;
            Weight = weight;
        }
    }
}
