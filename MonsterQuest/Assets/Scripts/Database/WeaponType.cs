using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    [CreateAssetMenu]
    public class WeaponType : ItemType
    {
        public string DamageRoll;

        // CONSTRUCTORS
        public WeaponType(string displayName, int weight, string damageRoll) : base(displayName, weight)
        {
            DamageRoll = damageRoll;
        }

    }
}
