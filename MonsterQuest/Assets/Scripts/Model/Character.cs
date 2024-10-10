using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MonsterQuest
{
    public class Character : Creature
    {
        // PROPERTIES
        public WeaponType WeaponType { get; set; }
        public ArmorType ArmorType { get; set; }
        private List<bool> _deathSavingThrows = new();
        public override IEnumerable<bool> DeathSavingThrows { get { return _deathSavingThrows; } protected set { } }
        public override int ArmorClass { get; set; }

        // CONSTRUCTORS
        public Character(string displayName, int hitPointsMaximum, Sprite bodySprite, SizeCategory sizeCat, WeaponType weaponType, ArmorType armorType)
            : base(displayName, bodySprite, sizeCat)
        {
            WeaponType = weaponType;
            ArmorType = armorType;
            HitPointsMaximum = hitPointsMaximum;
            ArmorClass = armorType.ArmorClass;
            Initialize();
        }

        public override IEnumerator ReactToDamage(int damageAmount, bool wasCriticalHit)
        {
            if (LifeStatus == LifeStatus.Conscious)
            {
                if (HitPoints + HitPointsMaximum <= damageAmount)
                {
                    HitPoints -= damageAmount;
                    LifeStatus = LifeStatus.Dead;
                    Presenter.UpdateStableStatus();
                    Console.WriteLine($"{DisplayName} takes so much damage they're immediately killed!");
                    yield return Presenter.TakeDamage(true);
                    yield return Presenter.Die();

                } else
                {
                    HitPoints -= damageAmount;
                    if(HitPoints <= 0)
                    {
                        HitPoints = 0;
                        LifeStatus = LifeStatus.UnconsciousUnstable;
                        Presenter.UpdateStableStatus();
                        Console.WriteLine($"{DisplayName} falls unconscious");
                    }
                    yield return Presenter.TakeDamage(false);

                }
            } else if (LifeStatus == LifeStatus.UnconsciousUnstable)
            {
                if (!wasCriticalHit) 
                {
                    DeathSavingThrowFailures++;
                    Console.WriteLine($"{DisplayName} fails a death saving throw");
                    yield return Presenter.PerformDeathSavingThrow(false);
                } else
                {
                    DeathSavingThrowFailures += 2;
                    Console.WriteLine($"{DisplayName} fails two death saving throws");
                    yield return Presenter.PerformDeathSavingThrow(false);
                    if (DeathSavingThrowFailures < 3)
                    {
                        yield return Presenter.PerformDeathSavingThrow(false);
                    }
                }
                
                if (DeathSavingThrowFailures >= 3)
                {
                    LifeStatus = LifeStatus.Dead;
                    Presenter.UpdateStableStatus();
                    Console.Write($"{DisplayName} meets their untimely end");
                    yield return Presenter.Die();
                }
            }
        }
        public override IAction TakeTurn(GameState gameState)
        {
            AttackAction action = new(this, gameState.Combat.Monster, WeaponType);

            return action;
        }
    }
}
