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
            Console.WriteLine($"watch out, {monster.DisplayName} with {monster.HitPoints} HP appears!\n".ToUpperFirst());

            turnOrder.Clear();
            turnOrder.AddRange(characters);
            turnOrder.Add(monster);
            turnOrder.ExtensionShuffle();
            do
            {
                foreach (Creature creature in turnOrder)
                {
                    if (monster.HitPoints == 0)
                    {
                        break;
                    }

                    if (creature.LifeStatus == LifeStatus.Dead || creature.LifeStatus == LifeStatus.UnconsciousStable)
                    {
                        continue;
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
