using System.Collections;
using UnityEngine;

namespace MonsterQuest
{
    public partial class CreaturePresenter
    {
        // Public methods

        public IEnumerator PerformDeathSavingThrow(bool success, int? rollResult = null)
        {
            Vector3 deathSavingThrowPosition = _deathSavingThrowsPresenter.StartDeathSavingThrow();

            if (rollResult is not null)
            {
                yield return _dicePresenter.RollD20(deathSavingThrowPosition + Vector3.right * 2.5f, rollResult.Value, success);
            }

            yield return _deathSavingThrowsPresenter.EndDeathSavingThrow(success);

            UpdateDeathSavingThrowFailures();
        }

        public void ResetDeathSavingThrows()
        {
            _deathSavingThrowsPresenter.Reset();
            UpdateDeathSavingThrowFailures();
        }

        // Private methods

        private void UpdateDeathSavingThrowFailures()
        {
            _standAnimator.SetInteger(_deathSavingThrowFailuresHash, _creature.DeathSavingThrowFailures);
        }
    }
}
