using System.Collections;
using UnityEngine;

namespace MonsterQuest
{
    public partial class CreaturePresenter
    {
        // Private methods

        private void SetHitPointRatio(float ratio)
        {
            float spaceInFeet = _creature.spaceInFeet * _creatureUnitScale;
            _hitPointsMaskTransform.localPosition = new Vector3(0, -spaceInFeet / 2, 0);
            _hitPointsMaskTransform.localScale = new Vector3(spaceInFeet, spaceInFeet * ratio, 1);

            _bodySpriteRenderer.color = Color.Lerp(damagedColor, Color.white, ratio);

            _currentHitPointRatio = ratio;
        }

        private void UpdateHitPoints()
        {
            AnimateToHitPointRatio(Mathf.Max(0f, _creature.hitPoints) / _creature.hitPointsMaximum);
        }

        private void AnimateToHitPointRatio(float ratio)
        {
            if (_hitPointAnimationCoroutine != null)
            {
                StopCoroutine(_hitPointAnimationCoroutine);
            }

            _hitPointAnimationCoroutine = AnimateToHitPointRatioCoroutine(ratio, 0.5f);
            StartCoroutine(_hitPointAnimationCoroutine);
        }

        private IEnumerator AnimateToHitPointRatioCoroutine(float endRatio, float transitionDuration)
        {
            float startRatio = _currentHitPointRatio;
            float startTime = Time.time;

            float transitionProgress;

            do
            {
                transitionProgress = transitionDuration > 0 ? (Time.time - startTime) / transitionDuration : 1;

                // Ease out to desired ratio.
                float easedTransitionProgress = Mathf.Sin(transitionProgress * Mathf.PI / 2);
                float newRatio = Mathf.Lerp(startRatio, endRatio, easedTransitionProgress);
                SetHitPointRatio(newRatio);

                yield return null;
            } while (transitionProgress < 1);

            _hitPointAnimationCoroutine = null;
        }
    }
}
