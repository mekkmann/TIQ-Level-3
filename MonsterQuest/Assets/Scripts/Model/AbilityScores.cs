using System;
using UnityEngine;

namespace MonsterQuest
{
    public enum Ability
    {
        None,
        Strength,
        Dexterity,
        Constitution,
        Intelligence,
        Wisdom,
        Charisma,
        All
    }

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

        // INDEXER
        public AbilityScore this[Ability ability]
        {
            get
            {
                return ability switch
                {
                    Ability.Strength => Strength,
                    Ability.Dexterity => Dexterity,
                    Ability.Constitution => Constitution,
                    Ability.Intelligence => Intelligence,
                    Ability.Wisdom => Wisdom,
                    Ability.Charisma => Charisma,
                    _ => null,
                };
            }
        }

        // PRIMARY CONSTRUCTOR
        public AbilityScores(AbilityScore strength, AbilityScore dexterity, AbilityScore constitution, AbilityScore intelligence, AbilityScore wisdom, AbilityScore charisma)
        {
            Strength = strength;
            Dexterity = dexterity;
            Constitution = constitution;
            Intelligence = intelligence;
            Wisdom = wisdom;
            Charisma = charisma;
        }

        // CONSTRUCTOR CHAINING
        public AbilityScores(int strength, int dexterity, int constitution, int intelligence, int wisdom, int charisma) 
            : this(new AbilityScore(strength), new AbilityScore(dexterity), new AbilityScore(constitution), new AbilityScore(intelligence), new AbilityScore(wisdom), new AbilityScore(charisma))
        {
            // EMPTY ON PURPOSE
        }
    }
}
