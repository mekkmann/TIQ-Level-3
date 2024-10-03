using System.Collections;
using UnityEngine;

namespace MonsterQuest
{
    public partial class CreaturePresenter
    {
        // Public methods

        public void UpdateStableStatus()
        {
            _standAnimator.SetBool(_stableHash, _creature.lifeStatus != LifeStatus.UnconsciousUnstable);
        }

        public IEnumerator Heal()
        {
            // Update hit points indicator.
            UpdateHitPoints();

            yield break;
        }

        public IEnumerator RegainConsciousness()
        {
            // The creature should stand.
            _bodySpriteAnimator.SetTrigger(_standHash);
            _standing = true;

            yield return new WaitForSeconds(1f);
        }

        public IEnumerator Die()
        {
            _animator.SetTrigger(_dieHash);

            yield return new WaitForSeconds(0.5f);

            Destroy(gameObject);
            _destroyed = true;
        }
    }
}
