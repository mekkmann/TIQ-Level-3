namespace MonsterQuest
{
    public class Combat
    {
        public Monster Monster { get; private set; }
        public Combat(Monster monster)
        {
            Monster = monster;
        }
    }
}
