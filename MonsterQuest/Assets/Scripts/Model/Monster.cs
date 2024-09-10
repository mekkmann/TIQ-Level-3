namespace MonsterQuest
{
    public class Monster : Creature
    {
        // PROPERTIES
        public int SavingThrowDC { get; private set; }
        // CONSTRUCTORS
        public Monster(string displayName, int hitPointsMaximum, UnityEngine.Sprite bodySprite, int savingThrowDC, SizeCategory sizeCat)
            : base(hitPointsMaximum, displayName, bodySprite, sizeCat)
        {
            SavingThrowDC = savingThrowDC;
        }

    }
}
