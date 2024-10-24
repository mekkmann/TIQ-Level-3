using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MonsterQuest
{
    [Serializable]
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
            Character target = null;
            if (AbilityScores[Ability.Intelligence] > 7)
            {
                target = aliveCharacters.OrderBy(chr => chr.HitPoints).First();
            } else
            {
                target = aliveCharacters.Random();
            }
            // choose weapon
            WeaponType weapon = Type.WeaponTypes.Random();

            AttackAction action = new(this, target, weapon);


            return action;
        }
    }
}
