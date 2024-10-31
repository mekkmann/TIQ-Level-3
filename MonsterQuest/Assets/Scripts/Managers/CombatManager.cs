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
            List<Character> participatedInCombat = gamestate.Party.Characters.Where(c => c.LifeStatus != LifeStatus.Dead).ToList();
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
                SaveGameHelper.Save(gamestate);
                yield return StartCoroutine(turn.Execute());

            } while (monster.HitPoints > 0 && gamestate.Party.Characters.Any(chr => chr.LifeStatus != LifeStatus.Dead));



            if (monster.HitPoints <= 0)
            {
                // TODO: Does this xp method work?
                Console.WriteLine($"The {monster.DisplayName} collapses and the heroes celebrate their victory!\n");
                float xpForEachPartyMember = monster.Type.ExperienceValue / participatedInCombat.Count();
                foreach (Character chr in participatedInCombat.Where(c => c.LifeStatus != LifeStatus.Dead))
                {
                    yield return StartCoroutine(chr.GainExperiencePoints(xpForEachPartyMember));
                }
                yield return new WaitForSeconds(2f);
                //yield return new WaitForSeconds(2f);
                //foreach (Character chr in participatedInCombat.Where(c => c.LifeStatus != LifeStatus.Dead))
                //{
                //    yield return StartCoroutine(chr.TakeShortRest());
                //}
                SaveGameHelper.Save(gamestate);
            }
            else
            {
                Console.WriteLine($"The party has failed and the {monster.DisplayName} continues to attack unsuspecting adventurers.");
                SaveGameHelper.Delete();
            }
            gamestate.ExitCombat();
        }
    }
}
