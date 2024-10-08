using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public enum LifeStatus
    {
        Conscious,
        UnconsciousStable,
        UnconsciousUnstable,
        Dead
    }
    public class Creature
    {
        public string DisplayName { get; protected set; }
        public int HitPoints { get; protected set; }
        public int HitPointsMaximum { get; protected set; }
        public Sprite BodySprite { get; protected set; }
        public SizeCategory SizeCat { get; protected set; }
        public float SpaceInFeet => SizeHelper.spaceInFeetPerSizeCategory[SizeCat];
        public CreaturePresenter Presenter { get; private set; }

        public LifeStatus LifeStatus { get; set; }
        public List<bool> DeathSavingThrows { get; protected set; }
        public int DeathSavingThrowFailures { get; set; }
        public int DeathSavingThrowSuccesses { get; set; }
        // CONSTRUCTORS
        public Creature(string displayName, Sprite bodySprite, SizeCategory sizeCat)
        {
            DisplayName = displayName;
            BodySprite = bodySprite;
            SizeCat = sizeCat;
            DeathSavingThrows = new List<bool>();
            LifeStatus = LifeStatus.Conscious;
        }

        // METHODS
        protected void Initialize()
        {
            HitPoints = HitPointsMaximum;
        }
        public virtual IEnumerator ReactToDamage(int damageAmount)
        {
            if (HitPoints > 0)
            {
                HitPoints -= damageAmount;
                if (HitPoints <= 0)
                {
                    HitPoints = 0;
                    yield return Presenter.TakeDamage();
                    yield return Presenter.Die();
                }
                else
                {
                    yield return Presenter.TakeDamage();
                    Console.WriteLine($"{DisplayName} has {HitPoints} health left.");
                }
            }
        }

        public void InitializePresenter(CreaturePresenter presenter)
        {
            Presenter = presenter;
        }
    }
}
