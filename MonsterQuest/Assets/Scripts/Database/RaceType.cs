using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    [CreateAssetMenu]
    public class RaceType : ScriptableObject
    {
        public string DisplayName;
        public Ability AbilityScoreToIncrease;
        public int IncreaseAbilityScoreBy;
        public SizeCategory SizeCategory;
        public int BaseMovementSpeed;
        public List<string> Languages;

        public RaceType(
            string displayName,
            Ability abilityScoreToIncrease,
            int increaseAbilityScoreBy,
            SizeCategory sizeCategory,
            int baseMovementSpeed,
            List<string> languages)
        {
            DisplayName = displayName;
            AbilityScoreToIncrease = abilityScoreToIncrease;
            IncreaseAbilityScoreBy = increaseAbilityScoreBy;
            SizeCategory = sizeCategory;
            BaseMovementSpeed = baseMovementSpeed;
            Languages = languages;
        }
    }
}