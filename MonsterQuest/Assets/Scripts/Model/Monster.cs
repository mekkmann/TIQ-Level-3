namespace MonsterQuest
{
    public class Monster : Creature
    {
        // PROPERTIES
        public MonsterType Type { get; }

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
