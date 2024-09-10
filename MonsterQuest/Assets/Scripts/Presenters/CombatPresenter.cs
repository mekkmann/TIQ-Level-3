using UnityEngine;

namespace MonsterQuest
{
    public class CombatPresenter : MonoBehaviour
    {
        [SerializeField] private GameObject creaturePrefab;

        private Transform _creaturesTransform;

        private void Awake()
        {
            _creaturesTransform = transform.Find("Creatures");
        }

        public void InitializeParty(GameState gameState)
        {
            // Create the character views.
            for (int i = 0; i < gameState.Party.Characters.Count; i++)
            {
                Creature character = gameState.Party.Characters[i];

                GameObject characterGameObject = Instantiate(creaturePrefab, _creaturesTransform);
                characterGameObject.name = character.DisplayName;
                characterGameObject.transform.position = new Vector3(((gameState.Party.Characters.Count - 1) * -0.5f + i) * 5, character.SpaceInFeet / 2, 0);

                CreaturePresenter creaturePresenter = characterGameObject.GetComponent<CreaturePresenter>();
                creaturePresenter.Initialize(character);
                creaturePresenter.FaceDirection(CardinalDirection.South);
            }
        }

        public void InitializeMonster(GameState gameState)
        {
            Combat combat = gameState.Combat;

            // Create the monster view.
            GameObject monsterGameObject = Instantiate(creaturePrefab, _creaturesTransform);
            monsterGameObject.name = combat.Monster.DisplayName;
            monsterGameObject.transform.position = new Vector3(0, -combat.Monster.SpaceInFeet / 2, 0);

            CreaturePresenter creaturePresenter = monsterGameObject.GetComponent<CreaturePresenter>();
            creaturePresenter.Initialize(combat.Monster);
            creaturePresenter.FaceDirection(CardinalDirection.North);
        }
    }
}
