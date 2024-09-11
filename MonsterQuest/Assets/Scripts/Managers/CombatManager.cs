using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public class CombatManager : MonoBehaviour
    {
        public IEnumerator Simulate(GameState gamestate)
        {
            System.Random random = new();
            Monster monster = gamestate.Combat.Monster;
            List<Character> characters = gamestate.Party.Characters;
            Console.WriteLine($"Watch out, {monster.DisplayName} with {monster.HitPoints} HP appears!\n");
            do
            {
                foreach (Character character in characters)
                {
                    if (monster.HitPoints == 0)
                    {
                        break;
                    }

                    int totalDamage = DiceHelper.Roll("2d6");
                    yield return StartCoroutine(character.Presenter.Attack());
                    yield return StartCoroutine(monster.ReactToDamage(totalDamage));

                    Console.WriteLine($"{character.DisplayName} hits the {monster.DisplayName} for {totalDamage} damage. The {monster.DisplayName} has {monster.HitPoints} HP left.");
                }

                if (monster.HitPoints > 0)
                {
                    Character chosenTarget = characters[random.Next(characters.Count)];
                    Console.WriteLine($"\nThe {monster.DisplayName} attacks {chosenTarget.DisplayName}");
                    int savingThrow = DiceHelper.Roll("1d20+3");
                    if (savingThrow >= monster.SavingThrowDC)
                    {
                        Console.WriteLine($"{chosenTarget.DisplayName} rolls a {savingThrow} and is saved from the attack.\n");
                    }
                    else
                    {
                        yield return StartCoroutine(monster.Presenter.Attack());
                        Console.WriteLine($"{chosenTarget.DisplayName} rolls a {savingThrow} and fails to be saved. {chosenTarget.DisplayName} is killed.\n");
                        yield return StartCoroutine(chosenTarget.ReactToDamage(10));
                        gamestate.Party.RemoveCharacter(chosenTarget);
                    }
                }
                yield return new WaitForSeconds(0.2f);
            } while (monster.HitPoints > 0 && characters.Count > 0);

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
