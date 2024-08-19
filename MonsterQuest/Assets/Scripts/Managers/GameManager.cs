using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public class GameManager : MonoBehaviour
    {
        private CombatManager _combatManager;

        private void Awake()
        {
            Transform combatTransform = transform.Find("Combat");
            _combatManager = combatTransform.GetComponent<CombatManager>();
        }

        // Start is called before the first frame update
        void Start()
        {
            List<string> characters = new() { "The Chosen Undead", "The Bearer of the Curse", "The Ashen One", "The Tarnished" };
            Console.WriteLine($"Fighters {StringHelper.JoinWithAnd(characters)} descend into the dungeon.\n");
            _combatManager.Simulate(characters, "orc", DiceHelper.Roll("2d8+6"), 10);
            if (characters.Count > 0) _combatManager.Simulate(characters, "azer", DiceHelper.Roll("6d8+12"), 18);
            if (characters.Count > 0) _combatManager.Simulate(characters, "troll", DiceHelper.Roll("8d10+40"), 16);
            if (characters.Count > 0) Console.WriteLine($"After three grueling battles, the heroes {StringHelper.JoinWithAnd(characters)} return from the dungeons to live another day.");
        }
    }
}
