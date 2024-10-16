using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MonsterQuest
{
    public class Monster : Creature
    {
        public MonsterType Type { get; }

        private static readonly bool[] _deathSavingThrows = new bool[0];
        public override IEnumerable<bool> DeathSavingThrows { get { return _deathSavingThrows; } protected set { } }
        public override int ArmorClass { get; set; }

        public override AbilityScores AbilityScores { 
            get
            {
                return Type.AbilityScores;
            } 
        }

        // CONSTRUCTORS
        public Monster(MonsterType type)
            : base(type.DisplayName, type.BodySprite, type.SizeCategory)
        {
            Type = type;
            HitPointsMaximum = DiceHelper.Roll(type.HpRoll);
            ArmorClass = type.ArmorType.ArmorClass;
            Initialize();
        }

        // METHODS
        public override IAction TakeTurn(GameState gameState)

        {
            Random rnd = new();
            // makes sure target is alive
            List<Character> aliveCharacters = gameState.Party.Characters.Where(chr => chr.LifeStatus != LifeStatus.Dead).ToList();
            // choose target
            Character target = aliveCharacters[rnd.Next(0, aliveCharacters.Count)];
            // choose weapon
            WeaponType weapon = Type.WeaponTypes[rnd.Next(0, Type.WeaponTypes.Count)];

            AttackAction action = new(this, target, weapon);

            return action;
        }
    }
}
