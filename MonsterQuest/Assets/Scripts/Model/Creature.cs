using System;
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
    [Serializable]
    public abstract class Creature
    {
        public string DisplayName { get; protected set; }
        public int HitPoints { get; protected set; }
        public int HitPointsMaximum { get; protected set; }
        public Sprite BodySprite { get; protected set; }
        public SizeCategory SizeCat { get; protected set; }
        public float SpaceInFeet => SizeHelper.spaceInFeetPerSizeCategory[SizeCat];
        [field: NonSerialized] public CreaturePresenter Presenter { get; private set; }

        public LifeStatus LifeStatus { get; set; }
        public abstract IEnumerable<bool> DeathSavingThrows { get; protected set; }
        public int DeathSavingThrowFailures { get; set; }
        public int DeathSavingThrowSuccesses { get; set; }
        public abstract int ArmorClass { get; set; }
        protected abstract int ProficiencyBonusBase { get; }
        public int ProficiencyBonus { 
            get 
            {

                return 2 + Math.Max(0, (ProficiencyBonusBase - 1) / 4);
            } 
        }

        public abstract AbilityScores AbilityScores { get; }
        // CONSTRUCTORS
        public Creature(string displayName, Sprite bodySprite, SizeCategory sizeCat)
        {
            DisplayName = displayName;
            BodySprite = bodySprite;
            SizeCat = sizeCat;
            LifeStatus = LifeStatus.Conscious;
        }

        // METHODS
        protected void Initialize()
        {
            HitPoints = HitPointsMaximum;
        }

        public abstract IAction TakeTurn(GameState gameState);
        public abstract bool IsProficientWithWeaponType(WeaponType type);

        public virtual IEnumerator Heal(int amount)
        {
            HitPoints += Mathf.Clamp(amount, 0, HitPointsMaximum);
            yield return Presenter.Heal();
        }

        public virtual IEnumerator ReactToDamage(int damageAmount, bool wasCriticalHit)
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
