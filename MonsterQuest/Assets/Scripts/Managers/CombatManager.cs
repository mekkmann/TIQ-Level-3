using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MonsterQuest
{
    public class CombatManager : MonoBehaviour
    {
        public IEnumerator Simulate(GameState gamestate)
        {
            Monster monster = gamestate.Combat.Monster;
            Console.WriteLine($"watch out, {monster.DisplayName} with {monster.HitPoints} HP appears!\n".ToUpperFirst());

            do
            {
                Creature creatureToTakeTurn = gamestate.Combat.StartNextCreatureTurn();
                if (monster.HitPoints == 0)
                {
                    break;
                }

                if (creatureToTakeTurn.LifeStatus == LifeStatus.Dead || creatureToTakeTurn.LifeStatus == LifeStatus.UnconsciousStable)
                {
                    continue;
                }
                IAction turn = creatureToTakeTurn.TakeTurn(gamestate);
                yield return StartCoroutine(turn.Execute());
                
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
