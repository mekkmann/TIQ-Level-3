using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public class CombatManager : MonoBehaviour
    {
        public void Simulate(List<string> characterNames, string monsterName, int monsterHP, int savingThrowDC)
        {
            System.Random random = new();
            int monstHP = monsterHP;

            Console.WriteLine($"Watch out, {monsterName} with {monstHP} HP appears!\n");
            do
            {
                foreach (var characterName in characterNames)
                {
                    int totalDamage = DiceHelper.Roll("2d6");
                    monstHP -= totalDamage;
                    if (monstHP <= 0)
                    {
                        monstHP = 0;
                    }

                    Console.WriteLine($"{characterName} hits the {monsterName} for {totalDamage} damage. The {monsterName} has {monstHP} HP left.");
                    if (monstHP == 0)
                    {
                        break;
                    }
                }

                if (monstHP > 0)
                {
                    string chosenTarget = characterNames[random.Next(characterNames.Count)];
                    Console.WriteLine($"\nThe {monsterName} attacks {chosenTarget}");
                    int savingThrow = DiceHelper.Roll("1d20+3");
                    if (savingThrow >= savingThrowDC)
                    {
                        Console.WriteLine($"{chosenTarget} rolls a {savingThrow} and is saved from the attack.\n");
                    }
                    else
                    {
                        Console.WriteLine($"{chosenTarget} rolls a {savingThrow} and fails to be saved. {chosenTarget} is killed.\n");
                        characterNames.Remove(chosenTarget);
                    }
                }
            } while (monstHP > 0 && characterNames.Count > 0);

            if (monstHP <= 0)
            {
                Console.WriteLine($"The {monsterName} collapses and the heroes celebrate their victory!\n");

            }
            else
            {
                Console.WriteLine($"The party has failed and the {monsterName} continues to attack unsuspecting adventurers.");
            }
        }
    }
}
