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

            // list of all MonsterTypes the party will fight
            List<MonsterType> monsterTypes = new()
            {
                Database.GetMonsterType("Orc"),
                Database.GetMonsterType("Bat"),
                Database.GetMonsterType("Swarm of Poisonous Snakes"),
                Database.GetMonsterType("Azer"),
                Database.GetMonsterType("Troll"),
            };

            // create GameState with party and list of MonsterTypes
            _gameState = new(party, monsterTypes);
        }

        private IEnumerator Simulate()
        {
            yield return _combatPresenter.InitializeParty(_gameState);
            List<string> characterDisplayNames = new();
            foreach (Character character in _gameState.Party.Characters)
            {
                characterDisplayNames.Add(character.DisplayName);
            }

            Console.WriteLine($"Fighters {StringHelper.JoinWithAnd(characterDisplayNames)} descend into the dungeon.\n");

            while (_gameState.CurrentMonsterIndex <= _gameState.AllMonsterTypes.Count && _gameState.Party.Characters.Count > 0)
            {
                if(_gameState.Combat == null)
                {
                    _gameState.EnterCombatWithMonster();
                }
                yield return StartCoroutine(_combatPresenter.InitializeMonster(_gameState));
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
