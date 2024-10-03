using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MonsterQuest
{
    public class DiceRollPresenter : MonoBehaviour
    {
        private const float _rollDuration = 1.5f;
        private const float _outcomeDuration = 0.5f;
        private static readonly int _succeedHash = Animator.StringToHash("Succeed");
        private static readonly int _failHash = Animator.StringToHash("Fail");

        [SerializeField] private int diceSides;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public IEnumerator Roll(int result, bool? success)
        {
            // Set numbers on dice sides.
            List<int> displayedNumbers = new();

            for (int diceSideIndex = 0; diceSideIndex < transform.childCount; diceSideIndex++)
            {
                int number;

                if (diceSideIndex == 0)
                {
                    number = result;
                }
                else
                {
                    do
                    {
                        number = Random.Range(1, diceSides + 1);
                    } while (displayedNumbers.Contains(number));
                }

                transform.GetChild(diceSideIndex).GetComponent<TextMeshPro>().text = number.ToString();

                displayedNumbers.Add(number);
            }

            yield return new WaitForSeconds(_rollDuration);

            if (success.HasValue)
            {
                // Transition to correct state.
                _animator.SetTrigger(success.Value ? _succeedHash : _failHash);

                StartCoroutine(DestroyAfterSeconds(_outcomeDuration));
            }
            else
            {
                yield return DestroyAfterSeconds(0);
            }
        }

        private IEnumerator DestroyAfterSeconds(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            Destroy(gameObject);
        }
    }
}
