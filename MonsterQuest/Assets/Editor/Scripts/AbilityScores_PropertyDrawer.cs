using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MonsterQuest
{
    //[CustomPropertyDrawer(typeof(AbilityScores))]
    public class AbilityScores_PropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new();

            UnityEngine.UIElements.PopupWindow popup = new() { text = "Ability Scores" };
            root.Add(popup);

            return root; 
        }
    }
}
