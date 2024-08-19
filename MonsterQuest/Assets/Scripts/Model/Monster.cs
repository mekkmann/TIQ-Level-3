namespace MonsterQuest
{
    public class Monster
    {
        // PROPERTIES
        public string DisplayName { get; private set; }
        public int HitPoints { get; private set; }
        public int SavingThrowDC { get; private set; }
        // CONSTRUCTORS
        public Monster(string displayName, int hitPoints, int savingThrowDC)
        {
            DisplayName = displayName;
            HitPoints = hitPoints;
            SavingThrowDC = savingThrowDC;
        }
        // METHODS
        public void ReactToDamage(int damageAmount)
        {

        }
    }
}
