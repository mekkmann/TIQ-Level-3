using System.Collections;
using UnityEngine;

namespace MonsterQuest
{
    public class DeathSavingThrowsPresenter : MonoBehaviour
    {
        private static readonly int _fail = Animator.StringToHash("Fail");
        private static readonly int _failState = Animator.StringToHash("Death saving throw fail");

        [SerializeField] private GameObject deathSavingThrowPrefab;

        private GameObject _lastDeathSavingThrow;

        public void Reset()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            SetXPosition(0);
        }

        public void AddDeathSavingThrow(bool success)
        {
            InstantiateSavingThrow();

            if (!success)
            {
                _lastDeathSavingThrow.GetComponent<Animator>().Play(_failState, 0, 1);
            }
        }

        private void InstantiateSavingThrow()
        {
            // Instantiate a new death saving throw.
            int index = transform.childCount;
            _lastDeathSavingThrow = Instantiate(deathSavingThrowPrefab, transform);
            _lastDeathSavingThrow.transform.localPosition = new Vector3(index, 0, 0);

            // Recenter the UI.
            SetXPosition(-index / 2f);
        }

        public Vector3 StartDeathSavingThrow()
        {
            // Instantiate a new death saving throw.
            int index = transform.childCount;
            _lastDeathSavingThrow = Instantiate(deathSavingThrowPrefab, transform);
            _lastDeathSavingThrow.transform.localPosition = new Vector3(index, 0, 0);

            // Recenter the UI.
            SetXPosition(-index / 2f);

            return _lastDeathSavingThrow.transform.position;
        }

        public IEnumerator EndDeathSavingThrow(bool success)
        {
            // Unless it was a success, animate the failure.
            if (!success)
            {
                _lastDeathSavingThrow.GetComponent<Animator>().SetTrigger(_fail);
            }

            yield return new WaitForSeconds(1);
        }

        private void SetXPosition(float x)
        {
            Vector3 localPosition = transform.localPosition;
            localPosition.x = x;
            transform.localPosition = localPosition;
        }
    }
}
