namespace MonsterQuest
{
    public class GameState
    {
        public Party Party { get; private set; }
        public Combat Combat { get; private set; }

        public GameState(Party party)
        {
            Party = party;
        }

        public void EnterCombatWithMonster(Monster monster)
        {
            // not yet implemented
        }
    }
}
