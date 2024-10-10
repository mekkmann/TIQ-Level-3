using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public class AttackAction : IAction
    {
        private Creature _attacker;
        private Creature _target;
        private WeaponType _weaponType;

        // CONSTRUCTORS
        public AttackAction(Creature attacker, Creature target, WeaponType weaponType)
        {
            _attacker = attacker;
            _target = target;
            _weaponType = weaponType;
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

            // C
            if (attackRoll == 1)
            {
                Console.WriteLine($"{_attacker.DisplayName} attacks {_target.DisplayName} with {_weaponType.DisplayName} but misses");
                yield return _attacker.Presenter.Attack();
            }
            else if (attackRoll == 20)
            {
                int totalDamage = 0;
                totalDamage += DiceHelper.Roll(_weaponType.DamageRoll);
                totalDamage += DiceHelper.Roll(_weaponType.DamageRoll);
                Console.WriteLine($"{_attacker.DisplayName} attacks {_target.DisplayName} with {_weaponType.DisplayName} for a Critical Hit of {totalDamage} damage!");
                yield return _attacker.Presenter.Attack();
                yield return _target.ReactToDamage(totalDamage, true);
            }
            else if (attackRoll >= _target.ArmorClass)
            {
                int totalDamage = 0;
                totalDamage += DiceHelper.Roll(_weaponType.DamageRoll);
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
