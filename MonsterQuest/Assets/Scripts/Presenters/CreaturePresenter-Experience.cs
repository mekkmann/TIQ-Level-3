using System.Collections;
using UnityEngine;

namespace MonsterQuest
{
    public partial class CreaturePresenter
    {
        // Public methods

        public void GainExperiencePoints()
        {
            StartCoroutine(CelebrateWithDelay());
        }

        private IEnumerator CelebrateWithDelay()
        {
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
            _bodySpriteAnimator.SetTrigger(_celebrateHash);
        }

        public IEnumerator LevelUp()
        {
            // Wait for the celebrate animation to be close to finished.
            yield return new WaitForSeconds(1f);

            // Update hit points indicator.
            UpdateHitPoints();

            _standAnimator.SetTrigger(_levelUpHash);

            yield return new WaitForSeconds(1f);
        }
    }
}
