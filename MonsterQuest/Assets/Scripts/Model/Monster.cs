namespace MonsterQuest
{
    public class Monster : Creature
    {
        // PROPERTIES
        public int SavingThrowDC { get; private set; }
        public MonsterType Type { get; }

        // CONSTRUCTORS
        public Monster(MonsterType type, int savingThrowDC)
            : base(type.DisplayName, type.BodySprite, type.SizeCategory)
        {
            SavingThrowDC = savingThrowDC;
            Type = type;
            HitPointsMaximum = DiceHelper.Roll(type.HpRoll);
            Initialize();
        }
    }
}
