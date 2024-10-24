using System.Collections.Generic;

namespace MonsterQuest
{
    public class GameState
    {
        public Party Party { get; private set; }
        public Combat Combat { get; private set; }
        public List<MonsterType> AllMonsterTypes { get; private set; }
        public int CurrentMonsterIndex { get; private set; }

        public GameState(Party party, List<MonsterType> allMonsterTypes)
        {
            Party = party;
            AllMonsterTypes = allMonsterTypes;
            CurrentMonsterIndex = 0;
        }

        public void EnterCombatWithMonster()
        {
            CurrentMonsterIndex++;
            Monster nextMonster = new(AllMonsterTypes[CurrentMonsterIndex - 1]);
            Combat = new(this, nextMonster);
        }

        public void ExitCombat()
        {
            Combat = null;
        }
    }
}
