using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public class GameManager : MonoBehaviour
    {
        private GameState _gameState;
        private CombatManager _combatManager;

        private void Awake()
        {
            Transform combatTransform = transform.Find("Combat");
            _combatManager = combatTransform.GetComponent<CombatManager>();
        }

        // Start is called before the first frame update
        void Start()
        {
            NewGame();
            Simulate();
        }

        private void NewGame()
        {
            // create party
            Party party = new(
                new List<Character>() {
                    new("Fighter 1"),
                    new("Fighter 2"),
                    new("Fighter 3"),
                    new("Fighter 4")
                    }
                );

            // create GameState with party
            _gameState = new(party);
        }

        private void Simulate()
        {
            // 1. move gameplay code from start into this method DONE
            // 2. instantiate the individual monsters and enter into combat with them
            // -- 2.1 (basically set the gamestate combat to which monster we're currently fighting for saving purposes)
            // -- 2.2 use the CombatManager to simulate the combat
            List<string> characterDisplayNames = new();
            foreach (Character character in _gameState.Party.Characters)
            {
                characterDisplayNames.Add(character.DisplayName);
            }
            Console.WriteLine($"Fighters {StringHelper.JoinWithAnd(characterDisplayNames)} descend into the dungeon.\n");
            Monster orc = new("orc", DiceHelper.Roll("2d8+6"), 10);
            _gameState.EnterCombatWithMonster(orc);
            _combatManager.Simulate(_gameState);

            if (_gameState.Party.Characters.Count > 0)
            {
                Monster azer = new("azer", DiceHelper.Roll("6d8+12"), 18);
                _gameState.EnterCombatWithMonster(azer);
                _combatManager.Simulate(_gameState);
            }

            if (_gameState.Party.Characters.Count > 0)
            {
                Monster troll = new("troll", DiceHelper.Roll("8d10+40"), 16);
                _gameState.EnterCombatWithMonster(troll);
                _combatManager.Simulate(_gameState);
            }

            if (_gameState.Party.Characters.Count > 0)
            {
                characterDisplayNames.Clear();
                foreach (Character character in _gameState.Party.Characters)
                {
                    characterDisplayNames.Add(character.DisplayName);
                }
                Console.WriteLine($"After three grueling battles, the heroes {StringHelper.JoinWithAnd(characterDisplayNames)} return from the dungeons to live another day.");
            }

        }
    }
}
