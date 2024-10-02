using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MonsterQuest
{
    public class GameManager : MonoBehaviour
    {
        private GameState _gameState;
        private CombatManager _combatManager;
        private CombatPresenter _combatPresenter;

        [SerializeField] private MonsterType[] _monsterTypes;
        [SerializeField] private Sprite[] _characterSprites;

        private void Awake()
        {
            Transform combatTransform = transform.Find("Combat");
            _combatManager = combatTransform.GetComponent<CombatManager>();
            _combatPresenter = combatTransform.GetComponent<CombatPresenter>();
        }

        // Start is called before the first frame update
        IEnumerator Start()
        {
            yield return StartCoroutine(Database.Initialize());
            NewGame();
            yield return StartCoroutine(Simulate());
        }

        private void NewGame()
        {
            ArmorType studdedLeather = Database.GetItemType<ArmorType>("Studded Leather");

            List<WeaponType> weapons = Database.itemTypes.Where(
                    itemType => itemType is WeaponType { Weight: > 0 }
                ).Cast<WeaponType>().ToList();

            // create party
            Party party = new(
                new List<Character>() {
                    new("Assassin", 10, _characterSprites[0], SizeCategory.Medium, weapons[0], studdedLeather),
                    new("Mage", 10, _characterSprites[1], SizeCategory.Medium, weapons[1], studdedLeather),
                    new("Paladin", 10, _characterSprites[2], SizeCategory.Medium, weapons[2], studdedLeather),
                    new("Warrior", 10, _characterSprites[3], SizeCategory.Medium, weapons[3], studdedLeather)
                    }
                );

            // create GameState with party
            _gameState = new(party);
        }

        private IEnumerator Simulate()
        {
            // 1. move gameplay code from start into this method DONE
            // 2. instantiate the individual monsters and enter into combat with them
            // -- 2.1 (basically set the gamestate combat to which monster we're currently fighting for saving purposes)
            // -- 2.2 use the CombatManager to simulate the combat
            _combatPresenter.InitializeParty(_gameState);
            List<string> characterDisplayNames = new();
            foreach (Character character in _gameState.Party.Characters)
            {
                characterDisplayNames.Add(character.DisplayName);
            }
            Console.WriteLine($"Fighters {StringHelper.JoinWithAnd(characterDisplayNames)} descend into the dungeon.\n");

            Monster orc = new(_monsterTypes[0]);
            _gameState.EnterCombatWithMonster(orc);
            _combatPresenter.InitializeMonster(_gameState);

            yield return StartCoroutine(_combatManager.Simulate(_gameState));

            if (_gameState.Party.Characters.Count > 0)
            {
                Monster swarm = new(Database.GetMonsterType("Swarm of Poisonous Snakes"));
                _gameState.EnterCombatWithMonster(swarm);
                _combatPresenter.InitializeMonster(_gameState);
                yield return StartCoroutine(_combatManager.Simulate(_gameState));
            }

            if (_gameState.Party.Characters.Count > 0)
            {
                Monster azer = new(_monsterTypes[1]);
                _gameState.EnterCombatWithMonster(azer);
                _combatPresenter.InitializeMonster(_gameState);
                yield return StartCoroutine(_combatManager.Simulate(_gameState));
            }

            if (_gameState.Party.Characters.Count > 0)
            {
                Monster troll = new(_monsterTypes[2]);
                _gameState.EnterCombatWithMonster(troll);
                _combatPresenter.InitializeMonster(_gameState);

                yield return StartCoroutine(_combatManager.Simulate(_gameState));
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
