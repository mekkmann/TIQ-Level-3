using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MonsterQuest
{
    public static class CardinalDirectionHelper
    {
        private static readonly CardinalDirection[] _cardinalDirections = Enum.GetValues(typeof(CardinalDirection)).Cast<CardinalDirection>().ToArray();

        public static readonly Dictionary<CardinalDirection, Vector2> cardinalDirectionVectors = new()
        {
            { CardinalDirection.South, new Vector2(0, 1) },
            { CardinalDirection.Southwest, new Vector2(-1, 1) },
            { CardinalDirection.West, new Vector2(-1, 0) },
            { CardinalDirection.Northwest, new Vector2(-1, -1) },
            { CardinalDirection.North, new Vector2(0, -1) },
            { CardinalDirection.Northeast, new Vector2(1, -1) },
            { CardinalDirection.East, new Vector2(1, 0) },
            { CardinalDirection.Southeast, new Vector2(1, 1) }
        };

        public static readonly Dictionary<CardinalDirection, float> cardinalDirectionRotationsDegrees = new();

        static CardinalDirectionHelper()
        {
            foreach (CardinalDirection direction in _cardinalDirections)
            {
                cardinalDirectionRotationsDegrees[direction] = 45 * (int)direction;
            }
        }

        public static CardinalDirection GetCardinalDirectionForVector(Vector2 vector)
        {
            float angleDegrees = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg + 22.5f;

            if (angleDegrees < 0)
            {
                angleDegrees += 360;
            }

            int angleBracket = (int)Math.Floor(angleDegrees / 45);

            return (CardinalDirection)angleBracket;
        }
    }
}
