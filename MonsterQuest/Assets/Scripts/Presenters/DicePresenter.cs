using System.Collections;
using UnityEngine;

namespace MonsterQuest
{
    public class DicePresenter : MonoBehaviour
    {
        [SerializeField] private GameObject d20Prefab;

        public IEnumerator RollD20(Vector3 position, int rollResult, bool? success = null)
        {
            yield return Roll(d20Prefab, position, rollResult, success);
        }

        private IEnumerator Roll(GameObject prefab, Vector3 position, int rollResult, bool? success)
        {
            // Place the dice on the correct depth layer.
            position.z = transform.position.z;

            // Instantiate the dice prefab at specified position.
            GameObject diceObject = Instantiate(prefab, position, Quaternion.identity, transform);

            DiceRollPresenter diceRollPresenter = diceObject.GetComponent<DiceRollPresenter>();

            yield return diceRollPresenter.Roll(rollResult, success);
        }
    }
}
