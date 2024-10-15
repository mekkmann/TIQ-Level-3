using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    [CreateAssetMenu]
    public class MonsterType : ScriptableObject
    {
        public string DisplayName;
        public SizeCategory SizeCategory;
        public string Alignment;
        public string HpRoll;
        public ArmorType ArmorType;
        public List<WeaponType> WeaponTypes;
        public Sprite BodySprite;
        public AbilityScores AbilityScores;

        // CONSTRUCTORS
        public MonsterType(string displayName, SizeCategory sizeCategory, string alignment, string hpRoll, ArmorType armorType, List<WeaponType> weaponTypes, Sprite bodySprite, AbilityScores abilityScores)
        {
            DisplayName = displayName;
            SizeCategory = sizeCategory;
            Alignment = alignment;
            HpRoll = hpRoll;
            ArmorType = armorType;
            WeaponTypes = new (weaponTypes);
            BodySprite = bodySprite;
            AbilityScores = abilityScores;
        }
    }
}
