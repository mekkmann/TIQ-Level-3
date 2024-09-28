using UnityEngine;

namespace MonsterQuest
{
    [CreateAssetMenu]
    public class ArmorType : ItemType
    {
        public ArmorCategory ArmorCategory;
        public int ArmorClass;

        // CONSTRUCTORS
        public ArmorType(ArmorCategory armorCategory, int weight, int armorClass, string displayName) : base(displayName, weight)
        {
            ArmorCategory = armorCategory;
            ArmorClass = armorClass;
        }
    }
}
