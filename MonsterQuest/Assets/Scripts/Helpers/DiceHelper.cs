using Random = System.Random;

namespace MonsterQuest
{
    public static class DiceHelper
    {
        static Random random = new();
        private static int Roll(int numberOfRolls, int diceSides, int fixedBonus = 0)
        {
            int total = fixedBonus;
            for (int i = 0; i < numberOfRolls; i++)
            {
                total += random.Next(1, diceSides + 1);
            }
            return total;
        }

        public static int Roll(string diceNotation)
        {
            // splits the diceNotation string based on delimiters
            string[] notationParts = diceNotation.Split('d', '-', '+');
            // if where the number of rolls should be is null or whitespace, default to 1 else get the number of rolls
            int numberOfRolls = string.IsNullOrWhiteSpace(notationParts[0]) ? 1 : int.Parse(notationParts[0]);
            // parsing the dice sides part of the string array
            int diceSides = int.Parse(notationParts[1]);

            // if the split notation contains more than 2 parts
            if (notationParts.Length > 2)
            {
                // parse the fixed bonus as a positive int
                int fixedBonus = int.Parse(notationParts[2]);
                // if the dicenotation contains '-'
                if (diceNotation.Contains('-'))
                {
                    // make the int negative
                    fixedBonus *= -1;
                }

                // return the result of diceroll including a fixed bonus
                return Roll(numberOfRolls, diceSides, fixedBonus);
            }
            else
            {
                // return the result of diceroll without any bonus
                return Roll(numberOfRolls, diceSides);
            }
        }
    }
}
