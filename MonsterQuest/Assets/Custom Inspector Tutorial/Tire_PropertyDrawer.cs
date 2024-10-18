using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace MonsterQuest
{
    [CustomPropertyDrawer(typeof(Tire))]
    public class Tire_PropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Create VisualElement to be the root of the property UI
            VisualElement container = new();

            // Create drawer UI with C#
            UnityEngine.UIElements.PopupWindow popup = new()
            {
                text = "Tire Details"
            };
            popup.Add(new PropertyField(property.FindPropertyRelative("AirPressure"), "Air Pressure (psi)"));
            popup.Add(new PropertyField(property.FindPropertyRelative("ProfileDepth"), "Profile Depth (mm)"));
            container.Add(popup);

            return container;
        }
    }
}
