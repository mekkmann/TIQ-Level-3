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

                    int totalDamage = DiceHelper.Roll(character.WeaponType.DamageRoll);
                    Console.WriteLine($"{character.DisplayName} hits the {monster.DisplayName} with {character.WeaponType.DisplayName} for {totalDamage} damage.");
                    yield return StartCoroutine(character.Presenter.Attack());
                    yield return StartCoroutine(monster.ReactToDamage(totalDamage));

                }

                if (monster.HitPoints > 0)
                {
                    Character chosenTarget = characters[random.Next(characters.Count)];

                    WeaponType monsterWeapon = monster.Type.WeaponTypes[Random.Range(0, monster.Type.WeaponTypes.Count - 1)];
                    int monsterDamage = DiceHelper.Roll(monsterWeapon.DamageRoll);
                    Console.WriteLine($"\nThe {monster.DisplayName} attacks {chosenTarget.DisplayName} with {monsterWeapon.DisplayName} for {monsterDamage} damage.");
                    yield return StartCoroutine(monster.Presenter.Attack());
                    yield return StartCoroutine(chosenTarget.ReactToDamage(monsterDamage));
                    
                    if (chosenTarget.HitPoints <= 0)
                    {
                        gamestate.Party.RemoveCharacter(chosenTarget);
                    }
                }
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
