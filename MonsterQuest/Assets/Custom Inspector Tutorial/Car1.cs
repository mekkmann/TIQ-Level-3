using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public class Car1 : MonoBehaviour
    {
        public string Make = "Toyota";
        public int YearBuilt = 1980;
        public Color Color = Color.black;

        public Tire[] Tires = new Tire[4];
    }
}
