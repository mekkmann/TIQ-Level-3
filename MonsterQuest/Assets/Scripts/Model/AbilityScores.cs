using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    [Serializable]
    public class AbilityScores
    {
        [field: SerializeField] 
        public AbilityScore Strength { get; private set; }
        [field: SerializeField] 
        public AbilityScore Dexterity { get; private set; }
        [field: SerializeField] 
        public AbilityScore Constitution { get; private set; }
        [field: SerializeField] 
        public AbilityScore Intelligence { get; private set; }
        [field: SerializeField] 
        public AbilityScore Wisdom { get; private set; }
        [field: SerializeField] 
        public AbilityScore Charisma { get; private set; }

        public AbilityScores(AbilityScore strength, AbilityScore dexterity, AbilityScore constitution, AbilityScore intelligence, AbilityScore wisdom, AbilityScore charisma)
        {
            Strength = strength;
            Dexterity = dexterity;
            Constitution = constitution;
            Intelligence = intelligence;
            Wisdom = wisdom;
            Charisma = charisma;
        }
    }
}
