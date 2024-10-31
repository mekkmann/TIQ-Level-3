using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    [CreateAssetMenu]
    public class ClassType : ScriptableObject
    {
        public string DisplayName;
        public List<WeaponCategory> WeaponProficiencies;
        public string HitDiceValue;

        public ClassType(string displayName, List<WeaponCategory> weaponProficiencies, string hitDiceValue)
        {
            DisplayName = displayName;
            WeaponProficiencies = weaponProficiencies;
            HitDiceValue = hitDiceValue;
        }
    }
}
