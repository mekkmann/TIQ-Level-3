using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    [CreateAssetMenu]
    public class MonsterType : ScriptableObject
    {
        public string DisplayName;
        public Sprite BodySprite;

        public string Alignment;
        public string HpRoll;

        public ArmorType ArmorType;
        public SizeCategory SizeCategory;

        public float ChallengeRating;
        public int ExperienceValue;

        public List<WeaponType> WeaponTypes;
        public AbilityScores AbilityScores;

        // CONSTRUCTORS
        public MonsterType(
            string displayName,
            SizeCategory sizeCategory,
            string alignment,
            string hpRoll,
            ArmorType armorType,
            List<WeaponType> weaponTypes, 
            Sprite bodySprite, // ask why it shows stacked in editor
            AbilityScores abilityScores, 
            int challengeRating,
            int experienceValue
            )
        {
            DisplayName = displayName;
            SizeCategory = sizeCategory;
            Alignment = alignment;
            HpRoll = hpRoll;
            ArmorType = armorType;
            WeaponTypes = new(weaponTypes);
            BodySprite = bodySprite;
            AbilityScores = abilityScores;
            ChallengeRating = challengeRating;
            ExperienceValue = experienceValue;
        }
    }
}
