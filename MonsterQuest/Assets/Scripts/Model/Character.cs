using UnityEngine;

namespace MonsterQuest
{
    public class Character : Creature
    {
        // PROPERTIES

        // CONSTRUCTORS
        public Character(string displayName, int hitPointsMaximum, Sprite bodySprite, SizeCategory sizeCat)
            : base(hitPointsMaximum, displayName, bodySprite, sizeCat)
        {
        }
    }
}
