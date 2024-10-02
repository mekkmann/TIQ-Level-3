using UnityEngine;

namespace MonsterQuest
{
    public class Character : Creature
    {
        // PROPERTIES
        public WeaponType WeaponType { get; set; }
        public ArmorType ArmorType { get; set; }

        // CONSTRUCTORS
        public Character(string displayName, int hitPointsMaximum, Sprite bodySprite, SizeCategory sizeCat, WeaponType weaponType, ArmorType armorType)
            : base(displayName, bodySprite, sizeCat)
        {
            WeaponType = weaponType;
            ArmorType = armorType;
            HitPointsMaximum = hitPointsMaximum;
            Initialize();
        }
    }
}
