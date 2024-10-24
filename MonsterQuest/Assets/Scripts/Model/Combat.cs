using System.Collections.Generic;
using System.Linq;

namespace MonsterQuest
{
    public class Combat
    {
        public Monster Monster { get; private set; }
        private readonly List<Creature> creaturesInOrderOfInitiative;
        private int currentCreatureIndex;
        public Combat(GameState gameState, Monster monster)
        {
            Monster = monster;
            creaturesInOrderOfInitiative = RollCombatInitiative(gameState);
            currentCreatureIndex = 0;
        }

        public Creature StartNextCreatureTurn()
        {
            if (currentCreatureIndex < creaturesInOrderOfInitiative.Count)
            {
                currentCreatureIndex++;
            } else
            {
                currentCreatureIndex = 0;
                currentCreatureIndex++;
            }

            return creaturesInOrderOfInitiative[currentCreatureIndex - 1];
        }

        public List<Creature> RollCombatInitiative(GameState gameState)
        {
            Dictionary<Creature, int> rolled = new();

            List<Creature> allCreatures = new();
            allCreatures.AddRange(gameState.Party.Characters);
            allCreatures.Add(Monster);

            foreach(Creature creature in allCreatures)
            {
                rolled.Add(creature, DiceHelper.Roll("1d20") + creature.AbilityScores[Ability.Dexterity].Modifier);
            }

            Dictionary<Creature, int> rolledInOrder = rolled.OrderByDescending(item => item.Value).ToDictionary(item => item.Key, item => item.Value);

            return new List<Creature>(rolledInOrder.Keys);

        }
    }
}
