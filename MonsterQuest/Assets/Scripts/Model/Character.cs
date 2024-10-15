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
                    _deathSavingThrows.Add(false);
                    Console.WriteLine($"{DisplayName} fails a death saving throw");
                    yield return Presenter.PerformDeathSavingThrow(false);
                } else
                {
                    DeathSavingThrowFailures += 2;
                    _deathSavingThrows.Add(false);
                    _deathSavingThrows.Add(false);
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
            IAction action = null;
            if (LifeStatus == LifeStatus.Conscious)
            {
                action = new AttackAction(this, gameState.Combat.Monster, WeaponType);
            }
            
            if (LifeStatus == LifeStatus.UnconsciousUnstable)
            {
                action = new BeUnconsciousAction(this);
            }

            return action;
        }

        public override IEnumerator Heal(int amount)
        {
            yield return base.Heal(amount);
        }

        public IEnumerator HandleUnconsciousState()
        {
            int roll = DiceHelper.Roll("1d20");

            if (roll == 1)
            {
                // 2 death saving failures
                DeathSavingThrowFailures += 2;
                _deathSavingThrows.Add(false);
                _deathSavingThrows.Add(false);
                yield return Presenter.PerformDeathSavingThrow(false, roll);
                Console.WriteLine($"{DisplayName} critically fails a death saving throw!");
            } else if (roll == 20)
            {
                yield return Presenter.PerformDeathSavingThrow(true, roll);
                Console.WriteLine($"{DisplayName} critically succeeds a death saving throw and heals 1 HP!");
                ResetDeathSavingThrows();
                yield return Heal(1);
                yield return Presenter.RegainConsciousness();
                LifeStatus = LifeStatus.Conscious;
                Presenter.UpdateStableStatus();
                yield break;
            } else if (roll > 1 && roll < 10)
            {
                //  1 death saving failure
                DeathSavingThrowFailures += 1;
                _deathSavingThrows.Add(false);
                yield return Presenter.PerformDeathSavingThrow(false, roll);
                Console.WriteLine($"{DisplayName} fails a death saving throw");
            } else
            {
                // 1 death saving success
                DeathSavingThrowSuccesses += 1;
                _deathSavingThrows.Add(true);
                yield return Presenter.PerformDeathSavingThrow(true, roll);
                Console.WriteLine($"{DisplayName} succeeds a death saving throw");
            }

            if (DeathSavingThrowFailures >= 3)
            {
                LifeStatus = LifeStatus.Dead;
                Presenter.UpdateStableStatus();
                Console.Write($"{DisplayName} meets their untimely end");
                yield return Presenter.Die();
            }

            if (DeathSavingThrowSuccesses >= 3)
            {
                LifeStatus = LifeStatus.UnconsciousStable;
                Presenter.UpdateStableStatus();
                ResetDeathSavingThrows();
                Console.Write($"{DisplayName} is now stable");
            }

        }

        private void ResetDeathSavingThrows()
        {
            DeathSavingThrowFailures = 0;
            DeathSavingThrowSuccesses = 0;
            _deathSavingThrows.Clear();
            Presenter.ResetDeathSavingThrows();
        }
    }
}
