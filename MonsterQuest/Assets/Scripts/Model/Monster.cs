using System.Collections.Generic;

namespace MonsterQuest
{
    public class Monster : Creature
    {
        // PROPERTIES
        public MonsterType Type { get; }

        private static readonly bool[] _deathSavingThrows = new bool[0];
        public override IEnumerable<bool> DeathSavingThrows { get { return _deathSavingThrows; } protected set { } }


        // CONSTRUCTORS
        public Monster(MonsterType type)
            : base(type.DisplayName, type.BodySprite, type.SizeCategory)
        {
            Type = type;
            HitPointsMaximum = DiceHelper.Roll(type.HpRoll);
            Initialize();
        }
    }
}
