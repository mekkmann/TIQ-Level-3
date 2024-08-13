using Random = System.Random;

namespace MonsterQuest
{
    public static class DiceHelper
    {
        static Random random = new();
        public static int Roll(int numberOfRolls, int diceSides, int fixedBonus = 0)
        {
            int total = fixedBonus;
            for (int i = 0; i < numberOfRolls; i++)
            {
                total += random.Next(1, diceSides + 1);
            }
            return total;
        }
    }
}
