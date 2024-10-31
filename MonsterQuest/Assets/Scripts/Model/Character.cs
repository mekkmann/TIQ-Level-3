using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MonsterQuest
{
    [Serializable]
    public class Character : Creature
    {
        // PROPERTIES AND FIELDS
        public WeaponType WeaponType { get; set; }
        public ArmorType ArmorType { get; set; }
        private List<bool> _deathSavingThrows = new();
        public override IEnumerable<bool> DeathSavingThrows { get { return _deathSavingThrows; } protected set { } }
        public override int ArmorClass { get; set; }
        public override AbilityScores AbilityScores { get; }
        public int Level { get; private set; }
        protected override int ProficiencyBonusBase
        {
            get { return Level; }
        }
        public ClassType ClassType { get; private set; }
        public float CurrentExperiencePoints { get; private set; }
        public int HitDiceMaximum { get; private set; }
        public int HitDiceRemaining { get; private set; }

        // CONSTRUCTORS
        public Character(string displayName, int hitPointsMaximum, Sprite bodySprite, SizeCategory sizeCat, WeaponType weaponType, ArmorType armorType, ClassType classType)
            : base(displayName, bodySprite, sizeCat)
        {
            WeaponType = weaponType;
            ArmorType = armorType;
            ArmorClass = armorType.ArmorClass;
            ClassType = classType;

            List<int> abilityScores = new();
            System.Random random = new();
            for (int i = 0; i < 6; i++)
            {
                List<int> temp = new();
                for (int j = 0; j < 4; j++)
                {
                    temp.Add(random.Next(1, 7));
                }
                temp.Remove(temp.Min());
                int totalTemp = temp.Sum();
                abilityScores.Add(totalTemp);
            }
            AbilityScores = new(abilityScores[0], abilityScores[1], abilityScores[2], abilityScores[3], abilityScores[4], abilityScores[5]);
            Level = 1;
            HitDiceMaximum = Level;
            HitPointsMaximum = RollHitDie();
            HitDiceRemaining = HitDiceMaximum;
            Initialize();
        }

        // METHODS
        private int RollHitDie()
        {
            HitDiceRemaining--;
            // assuming we're using class specific hit die
            return DiceHelper.Roll(ClassType.HitDiceValue) + AbilityScores.Constitution.Modifier;
        }
        private IEnumerator LevelUp()
        {
            // Increase Level
            Level++;
            // Increase MaximumHitDice by 1
            HitDiceMaximum += 1;
            HitDiceRemaining += 1;
            // Increase max hitpoints
            int hitDieResult = RollHitDie();
            HitPointsMaximum += hitDieResult > 1 ? hitDieResult : 1;
            Console.WriteLine($"{DisplayName} levels up to level {Level}! Their maximum HP increases to {HitPointsMaximum}.");
            yield return Presenter.LevelUp();

        }
        public IEnumerator GainExperiencePoints(float amount)
        {
            CurrentExperiencePoints += amount;
            Presenter.GainExperiencePoints();
            if (CurrentExperiencePoints >= LevelHelper.LevelRequirements[Level + 1])
            {
                yield return LevelUp();
            }
        }
        public IEnumerator TakeShortRest()
        {
            if (HitPoints < HitPointsMaximum)
            {
                int toHeal = 0;
                while (HitPoints + toHeal < HitPointsMaximum && HitDiceRemaining > 0)
                {
                    int hitDieResult = RollHitDie();
                    toHeal += hitDieResult > 0 ? hitDieResult : 0;
                }
                int actualHeal = Mathf.Clamp(toHeal, 0, HitPointsMaximum - HitPoints);
                yield return Heal(actualHeal);

                if (HitPoints == HitPointsMaximum)
                {
                    Console.WriteLine($"{DisplayName} heals {actualHeal} and is at full health ({HitPoints}/{HitPointsMaximum}).");
                }
                else
                {
                    Console.WriteLine($"{DisplayName} heals {actualHeal} and is at {HitPoints}/{HitPointsMaximum} health.");
                }
            }
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

                }
                else
                {
                    HitPoints -= damageAmount;
                    if (HitPoints <= 0)
                    {
                        HitPoints = 0;
                        LifeStatus = LifeStatus.UnconsciousUnstable;
                        Presenter.UpdateStableStatus();
                        Console.WriteLine($"{DisplayName} falls unconscious");
                    }
                    yield return Presenter.TakeDamage(false);

                }
            }
            else if (LifeStatus == LifeStatus.UnconsciousUnstable)
            {
                if (!wasCriticalHit)
                {
                    DeathSavingThrowFailures++;
                    _deathSavingThrows.Add(false);
                    Console.WriteLine($"{DisplayName} fails a death saving throw");
                    yield return Presenter.PerformDeathSavingThrow(false);
                }
                else
                {
                    DeathSavingThrowFailures += 2;
                    _deathSavingThrows.Add(false);
                    _deathSavingThrows.Add(false);
                    Console.WriteLine($"{DisplayName} fails two death saving throws");
                    yield return Presenter.PerformDeathSavingThrow(false);
                    if (DeathSavingThrowFailures <= 3)
                    {
                        yield return Presenter.PerformDeathSavingThrow(false);
                    }
                }

                if (DeathSavingThrowFailures >= 3)
                {
                    LifeStatus = LifeStatus.Dead;
                    Presenter.UpdateStableStatus();
                    Console.WriteLine($"{DisplayName} meets their untimely end");
                    yield return Presenter.Die();
                }
            }
        }
        public override IAction TakeTurn(GameState gameState)
        {
            IAction action = null;
            if (LifeStatus == LifeStatus.Conscious)
            {
                Ability ability = Ability.None;
                if (WeaponType.IsFinesse)
                {
                    ability = AbilityScores[Ability.Strength] > AbilityScores[Ability.Dexterity]
                        ? Ability.Strength : Ability.Dexterity;
                }
                action = new AttackAction(this, gameState.Combat.Monster, WeaponType, ability);
            }

            if (LifeStatus == LifeStatus.UnconsciousUnstable)
            {
                action = new BeUnconsciousAction(this);
            }

            return action;
        }

        public override IEnumerator Heal(int amount)
        {
            // TODO: Unconscious characters dont automatically come back to "life" after healing
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
            }
            else if (roll == 20)
            {
                yield return Presenter.PerformDeathSavingThrow(true, roll);
                Console.WriteLine($"{DisplayName} critically succeeds a death saving throw and heals 1 HP!");
                ResetDeathSavingThrows();
                yield return Heal(1);
                yield return Presenter.RegainConsciousness();
                LifeStatus = LifeStatus.Conscious;
                Presenter.UpdateStableStatus();
                yield break;
            }
            else if (roll > 1 && roll < 10)
            {
                //  1 death saving failure
                DeathSavingThrowFailures += 1;
                _deathSavingThrows.Add(false);
                yield return Presenter.PerformDeathSavingThrow(false, roll);
                Console.WriteLine($"{DisplayName} fails a death saving throw");
            }
            else
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

        public override bool IsProficientWithWeaponType(WeaponType type)
        {
            return type.WeaponCategories.Any(wepType => ClassType.WeaponProficiencies.Contains(wepType));
        }
    }
}
