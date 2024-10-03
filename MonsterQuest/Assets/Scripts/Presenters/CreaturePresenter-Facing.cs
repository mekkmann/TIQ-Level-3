using System.Collections;
using UnityEngine;

namespace MonsterQuest
{
    public partial class CreaturePresenter
    {
        // Public methods

        public IEnumerator FaceDirection(CardinalDirection direction, bool immediate = false)
        {
            // Get the angle we need to rotate by.
            float angleDegrees = CardinalDirectionHelper.cardinalDirectionRotationsDegrees[direction];

            yield return FaceAngle(angleDegrees, immediate);
        }

        public IEnumerator FaceCreature(Creature creature, bool immediate = false)
        {
            // Make sure the creature is not destroyed.
            if (creature == null || creature.presenter == null) yield break;

            // Get the angle we need to rotate by.
            Vector3 directionToCreature = creature.presenter.transform.position - transform.position;
            float angleDegrees = Mathf.Atan2(directionToCreature.y, directionToCreature.x) * Mathf.Rad2Deg;

            yield return FaceAngle(angleDegrees, immediate);
        }

        // Private methods

        private IEnumerator FaceAngle(float angleDegrees, bool immediate)
        {
            // Account for sprites being positioned in the south direction.
            angleDegrees -= CardinalDirectionHelper.cardinalDirectionRotationsDegrees[CardinalDirection.South];

            if (immediate)
            {
                SetLocalRotation(angleDegrees);

                yield break;
            }

            yield return StartCoroutine(AnimateLocalRotation(angleDegrees));
        }

        private IEnumerator AnimateLocalRotation(float angleDegrees)
        {
            float startAngleDegrees = _bodyOrientationTransform.localRotation.eulerAngles.z;
            float deltaAngle = Mathf.DeltaAngle(startAngleDegrees, angleDegrees);
            angleDegrees = startAngleDegrees + deltaAngle;

            float startTime = Time.time;

            float transitionDuration = Mathf.Abs(angleDegrees - startAngleDegrees) * 0.01f;
            float transitionProgress;

            do
            {
                if (_destroyed) yield break;

                transitionProgress = transitionDuration > 0 ? (Time.time - startTime) / transitionDuration : 1;

                // Smoothly interpolate to desired height.
                float currentAngleDegrees = Mathf.SmoothStep(startAngleDegrees, angleDegrees, transitionProgress);
                SetLocalRotation(currentAngleDegrees);

                yield return null;
            } while (transitionProgress < 1);
        }

        private void SetLocalRotation(float angleDegrees)
        {
            // Rotate the body orientation.
            _bodyOrientationTransform.localRotation = Quaternion.Euler(0, 0, angleDegrees);
        }
    }
}
