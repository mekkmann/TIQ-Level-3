using System.Collections;
using UnityEngine;

namespace MonsterQuest
{
    public class CreaturePresenter : MonoBehaviour
    {



        private const float _creatureUnitScale = 0.8f;

        private static readonly int _attackHash = Animator.StringToHash("Attack");
        private static readonly int _takeDamageHash = Animator.StringToHash("Take damage");
        private static readonly int _dieHash = Animator.StringToHash("Die");
        private static readonly int _liveHash = Animator.StringToHash("Live");

        [SerializeField] private Color damagedColor;
        [SerializeField] private Sprite[] standSprites;

        private Animator _bodySpriteAnimator;
        private Creature _creature;

        private float _currentHitPointRatio;
        private IEnumerator _hitPointAnimationCoroutine;

        private SpriteRenderer _bodySpriteRenderer;

        private Transform _bodyOrientationTransform;
        private Transform _bodySpriteTransform;
        private Transform _bodyTransform;
        private Transform _bodyVerticalDisplacementTransform;
        private Transform _hitPointsMaskTransform;
        private Transform _standBaseTransform;

        private void Awake()
        {
            _bodyTransform = transform.Find("Body");

            _bodyVerticalDisplacementTransform = _bodyTransform.Find("Vertical displacement");
            _bodyOrientationTransform = _bodyVerticalDisplacementTransform.Find("Orientation");

            _bodySpriteTransform = _bodyOrientationTransform.Find("Sprite");
            _bodySpriteAnimator = _bodySpriteTransform.GetComponent<Animator>();

            Transform standTransform = transform.Find("Stand");
            _standBaseTransform = standTransform.Find("Base");
            _hitPointsMaskTransform = standTransform.Find("Hit points mask");
        }

        public void Initialize(Creature creature)
        {
            // Connect model and presenter.
            _creature = creature;
            _creature.InitializePresenter(this);

            // Set body sprite.
            _bodySpriteRenderer = _bodySpriteTransform.GetComponent<SpriteRenderer>();
            _bodySpriteRenderer.sprite = _creature.BodySprite;

            // Set stand sprite.
            SpriteRenderer[] standSpriteRenderers = _standBaseTransform.GetComponentsInChildren<SpriteRenderer>();

            foreach (SpriteRenderer standSpriteRenderer in standSpriteRenderers)
            {
                standSpriteRenderer.sprite = standSprites[(int)_creature.SizeCat - 1];
            }

            // Set initial hit points.
            SetHitPointRatio((float)creature.HitPoints / creature.HitPointsMaximum);
        }

        public void FaceDirection(CardinalDirection direction)
        {
            // Get the angle we need to rotate by.
            float angleDegrees = CardinalDirectionHelper.cardinalDirectionRotationsDegrees[direction];

            // Account for sprites being positioned in the south direction.
            angleDegrees -= CardinalDirectionHelper.cardinalDirectionRotationsDegrees[CardinalDirection.South];

            // Rotate the body orientation and the stand base.
            _bodyOrientationTransform.localRotation = Quaternion.Euler(0, 0, angleDegrees);
            _standBaseTransform.localRotation = _bodyOrientationTransform.localRotation;
        }

        public IEnumerator Attack()
        {
            // Trigger the attack animation.
            _bodySpriteAnimator.SetTrigger(_attackHash);

            yield return new WaitForSeconds(15f / 60f);
        }

        public IEnumerator TakeDamage()
        {
            // Trigger the take damage animation.
            _bodySpriteAnimator.SetTrigger(_takeDamageHash);

            // Update hit points indicator.
            UpdateHitPoints();

            yield return new WaitForSeconds(1f);
        }

        public IEnumerator Heal()
        {
            // Update hit points indicator.
            UpdateHitPoints();

            yield break;
        }

        public IEnumerator FallUnconscious()
        {
            // Trigger the die animation.
            _bodySpriteAnimator.SetTrigger(_dieHash);

            // Update hit points indicator.
            UpdateHitPoints();

            yield return new WaitForSeconds(1.9f);
        }

        public IEnumerator RegainConsciousness()
        {
            // Trigger the live animation.
            _bodySpriteAnimator.SetTrigger(_liveHash);

            yield return new WaitForSeconds(1f);
        }

        public IEnumerator Die()
        {
            // Trigger the die animation.
            _bodySpriteAnimator.SetTrigger(_dieHash);

            // Update hit points indicator.
            UpdateHitPoints();

            yield return new WaitForSeconds(2f);

            Destroy(gameObject);

            yield return new WaitForSeconds(0.5f);
        }

        private void SetHitPointRatio(float ratio)
        {
            float spaceTaken = _creature.SpaceInFeet * _creatureUnitScale;
            _hitPointsMaskTransform.localPosition = new Vector3(0, -spaceTaken / 2, 0);
            _hitPointsMaskTransform.localScale = new Vector3(spaceTaken, spaceTaken * ratio, 1);

            _bodySpriteRenderer.color = Color.Lerp(damagedColor, Color.white, ratio);

            _currentHitPointRatio = ratio;
        }

        private void UpdateHitPoints()
        {
            AnimateToHitPointRatio((float)_creature.HitPoints / _creature.HitPointsMaximum);
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
