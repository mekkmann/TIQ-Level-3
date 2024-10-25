using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    [CreateAssetMenu]
    public class ClassType : ScriptableObject
    {
        public string DisplayName;
        public List<WeaponCategory> WeaponProficiencies;

        public ClassType(string displayName, List<WeaponCategory> weaponProficiencies)
        {
            DisplayName = displayName;
            WeaponProficiencies = weaponProficiencies;
        }
    }
}
