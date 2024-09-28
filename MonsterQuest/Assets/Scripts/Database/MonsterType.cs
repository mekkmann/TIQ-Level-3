using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    [CreateAssetMenu]
    public class MonsterType : ScriptableObject
    {
        public string DisplayName;
        public SizeCategory SizeCategory;
        public string Alignment;
        public string HpRoll;
        public ArmorType ArmorType;
        public List<WeaponType> WeaponTypes;
        public Sprite BodySprite;

        // CONSTRUCTORS
        public MonsterType(string displayName, SizeCategory sizeCategory, string alignment, string hpRoll, ArmorType armorType, List<WeaponType> weaponTypes, Sprite bodySprite)
        {
            DisplayName = displayName;
            SizeCategory = sizeCategory;
            Alignment = alignment;
            HpRoll = hpRoll;
            ArmorType = armorType;
            WeaponTypes = new (weaponTypes);
            BodySprite = bodySprite;
        }
        //public void PrintMonsterData(Dictionary<ArmorTypeId, ArmorType> armorTypes)
        //{
        //    //prints name
        //    Console.WriteLine($"Name: {name}");
        //    //prints description
        //    Console.WriteLine($"Description: {description}");
        //    //prints alignment
        //    Console.WriteLine($"Alignment: {alignment}");
        //    //prints hp roll
        //    Console.WriteLine($"Hit points roll: {hpRoll}");
        //    Console.WriteLine($"Armor class: {armorClass}");

        //    // if the monsters armor types is of a specific category
        //    if (armorTypes.TryGetValue(armorType, out ArmorType value))
        //    {
        //        //prints armor type
        //        Console.WriteLine($"Armor type: {value.DisplayName}");
        //        // prints armor category
        //        Console.WriteLine($"Armor category: {value.Category}");
        //        // prints armor weight
        //        Console.WriteLine($"Armor weight: {value.Weight} lb");
        //    }
        //    else
        //    {
        //        //prints armor type
        //        Console.WriteLine($"Armor type: {armorType}");
        //    }
        //}
    }
}
