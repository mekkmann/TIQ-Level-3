using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public class AttackAction : IAction
    {
        private readonly Creature _attacker;
        private readonly Creature _target;
        private readonly WeaponType _weaponType;
        private readonly Ability? _ability;

        // CONSTRUCTORS
        public AttackAction(Creature attacker, Creature target, WeaponType weaponType, Ability? ability = Ability.None)
        {
            _attacker = attacker;
            _target = target;
            _weaponType = weaponType;
            _ability = ability;
        }

        // METHODS
        public IEnumerator Execute()
        {
            // A
            yield return _attacker.Presenter.FaceCreature(_target);

            // B
            int attackRoll = 0;
            switch(_target.LifeStatus)
            {
                case LifeStatus.UnconsciousStable:
                case LifeStatus.UnconsciousUnstable:
                    attackRoll = 20;
                    break;
                case LifeStatus.Conscious:
                    attackRoll = DiceHelper.Roll("1d20");
                    break;
                default:
                    break;
            }


            if (_attacker.IsProficientWithWeaponType(_weaponType))
            {
                attackRoll += _attacker.ProficiencyBonus;
            }


            int modifier = 0;
            if (!_weaponType.IsRanged)
            {
                modifier += _attacker.AbilityScores[Ability.Strength].Modifier;
            } else if (_weaponType.IsRanged)
            {
                modifier += _attacker.AbilityScores[Ability.Dexterity].Modifier;
            } else if (_weaponType.IsFinesse)
            {
                if (_ability != Ability.None)
                {
                    modifier += _attacker.AbilityScores[(Ability)_ability].Modifier;
                } else
                {
                    modifier += _attacker.AbilityScores[Ability.Strength] > _attacker.AbilityScores[Ability.Dexterity]
                        ? _attacker.AbilityScores[Ability.Strength].Modifier : _attacker.AbilityScores[Ability.Dexterity].Modifier;
                }
            }
            // C
            if (attackRoll == 1)
            {
                Console.WriteLine($"{_attacker.DisplayName} attacks {_target.DisplayName} with {_weaponType.DisplayName} but misses");
                yield return _attacker.Presenter.Attack();
            }
            else if (attackRoll >= 20)
            {
                int totalDamage = 0;
                totalDamage += DiceHelper.Roll(_weaponType.DamageRoll);
                totalDamage += DiceHelper.Roll(_weaponType.DamageRoll);
                Console.WriteLine($"{_attacker.DisplayName} attacks {_target.DisplayName} with {_weaponType.DisplayName} for a Critical Hit of {totalDamage} damage!");
                yield return _attacker.Presenter.Attack();
                yield return _target.ReactToDamage(totalDamage, true);
            }
            else if (attackRoll + modifier >= _target.ArmorClass)
            {
                int totalDamage = modifier;
                totalDamage += DiceHelper.Roll(_weaponType.DamageRoll);
                if (totalDamage < 0)
                {
                    totalDamage = 0;
                }
                Console.WriteLine($"{_attacker.DisplayName} attacks {_target.DisplayName} with {_weaponType.DisplayName} for {totalDamage} damage!");
                yield return _attacker.Presenter.Attack();
                yield return _target.ReactToDamage(totalDamage, false);
            }
            else
            {
                Console.WriteLine($"{_attacker.DisplayName} attacks {_target.DisplayName} with {_weaponType.DisplayName} but misses");
                yield return _attacker.Presenter.Attack();
            }

        }
    }
}
