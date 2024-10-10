using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MonsterQuest
{
    public class CombatManager : MonoBehaviour
    {
        private List<Creature> turnOrder = new();
        public IEnumerator Simulate(GameState gamestate)
        {
            Monster monster = gamestate.Combat.Monster;
            List<Character> characters = gamestate.Party.Characters;
            Console.WriteLine($"Watch out, {monster.DisplayName} with {monster.HitPoints} HP appears!\n");

            turnOrder.Clear();
            turnOrder.AddRange(characters);
            turnOrder.Add(monster);
            turnOrder = ListHelper.Shuffle(turnOrder);

            do
            {
                foreach (Creature creature in turnOrder)
                {
                    if (creature.LifeStatus != LifeStatus.Conscious)
                    {
                        continue;
                    }

                    if (creature is Monster && creature.HitPoints == 0)
                    {
                        break;
                    }

                    IAction turn = creature.TakeTurn(gamestate);
                    yield return StartCoroutine(turn.Execute());
                }
            } while (monster.HitPoints > 0 && gamestate.Party.Characters.Any(chr => chr.LifeStatus != LifeStatus.Dead));




            if (monster.HitPoints <= 0)
            {
                Console.WriteLine($"The {monster.DisplayName} collapses and the heroes celebrate their victory!\n");
            }
            else
            {
                Console.WriteLine($"The party has failed and the {monster.DisplayName} continues to attack unsuspecting adventurers.");
            }
        }
    }
}
