using System.Collections;
using UnityEngine;

namespace MonsterQuest
{
    public class Creature : MonoBehaviour
    {
        public string DisplayName { get; protected set; }
        public int HitPoints { get; protected set; }
        public int HitPointsMaximum { get; protected set; }
        public Sprite BodySprite { get; protected set; }
        public SizeCategory SizeCat { get; protected set; }
        public float SpaceInFeet => SizeHelper.spaceInFeetPerSizeCategory[SizeCat];
        public CreaturePresenter Presenter { get; private set; }

        // CONSTRUCTORS
        public Creature(int hitPointsMaximum, string displayName, Sprite bodySprite, SizeCategory sizeCat)
        {
            HitPointsMaximum = hitPointsMaximum;
            HitPoints = hitPointsMaximum;
            DisplayName = displayName;
            BodySprite = bodySprite;
            SizeCat = sizeCat;
        }

        // METHODS
        public IEnumerator ReactToDamage(int damageAmount)
        {
            if (HitPoints > 0)
            {
                HitPoints -= damageAmount;
                yield return StartCoroutine(Presenter.TakeDamage());
                if (HitPoints < 0)
                {
                    HitPoints = 0;
                    yield return StartCoroutine(Presenter.Die());
                }
            }
        }

        public void InitializePresenter(CreaturePresenter presenter)
        {
            Presenter = presenter;
        }
    }
}
