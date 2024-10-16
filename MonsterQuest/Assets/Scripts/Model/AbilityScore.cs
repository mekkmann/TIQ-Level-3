using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    [Serializable]
    public class AbilityScore
    {
        [field: SerializeField]
        public int Score { get; set; }
        public int Modifier
        {
            get
            {
                float rawModifier = (Score / 2) - 10;
                return Mathf.FloorToInt(rawModifier);
            }
        }

        // CONSTRUCTOR
        public AbilityScore(int value)
        {
            Score = value;
        }

        // IMPLICIT CONVERSION OPERATOR
        public static implicit operator int(AbilityScore abilityScore) 
        { 
            return abilityScore.Score;
        }

    }
}
