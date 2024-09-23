using UnityEngine;

namespace MonsterQuest
{
    public class Character : Creature
    {
        // PROPERTIES

        // CONSTRUCTORS
        public Character(string displayName, int hitPointsMaximum, Sprite bodySprite, SizeCategory sizeCat)
            : base(displayName, bodySprite, sizeCat)
        {
            HitPointsMaximum = hitPointsMaximum;
            Initialize();
        }
    }
}
