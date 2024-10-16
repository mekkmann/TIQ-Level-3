using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    [CreateAssetMenu]
    public class WeaponType : ItemType
    {
        public string DamageRoll;
        public bool IsRanged;
        public bool IsFinesse;

        // CONSTRUCTORS
        public WeaponType(string displayName, int weight, string damageRoll, bool isRanged, bool isFinesse) : base(displayName, weight)
        {
            DamageRoll = damageRoll;
            IsRanged = isRanged;
            IsFinesse = isFinesse;
        }

    }
}
