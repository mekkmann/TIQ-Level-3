using System.Collections;
using UnityEngine;

namespace MonsterQuest
{
    public partial class CreaturePresenter
    {
        // Public methods

        public IEnumerator Attack()
        {
            // Trigger the attack animation.
            _bodySpriteAnimator.SetTrigger(_attackHash);

            yield return new WaitForSeconds(15f / 60f);
        }

        public IEnumerator TakeDamage(bool instantDeath = false)
        {
            // Update hit points indicator.
            UpdateHitPoints();

            if (_standing && _creature.hitPoints == 0)
            {
                if (instantDeath)
                {
                    // The more extreme version of dying should play.
                    _bodySpriteAnimator.SetTrigger(_attackedToInstantDeathHash);
                }
                else
                {
                    // The creature should get attacked and end up lying down.
                    _bodySpriteAnimator.SetTrigger(_attackedToLyingHash);
                }

                _standing = false;

                yield return new WaitForSeconds(2f);
            }
            else
            {
                // The creature gets attacked in its current state.
                _bodySpriteAnimator.SetTrigger(_attackedHash);

                yield return new WaitForSeconds(1f);
            }
        }
    }
}
