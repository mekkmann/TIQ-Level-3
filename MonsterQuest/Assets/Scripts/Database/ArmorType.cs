using UnityEngine;

namespace MonsterQuest
{
    [CreateAssetMenu]
    public class ArmorType : ScriptableObject
    {
        public string DisplayName;
        public ArmorCategory ArmorCategory;
        public int Weight;
        public int ArmorClass;

        // CONSTRUCTORS
        public ArmorType(string displayName, ArmorCategory armorCategory, int weight, int armorClass)
        {
            DisplayName = displayName;
            ArmorCategory = armorCategory;
            Weight = weight;
            ArmorClass = armorClass;
        }
    }
}
