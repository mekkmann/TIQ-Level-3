using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.VisualScripting;

namespace MonsterQuest
{
    [CustomPropertyDrawer(typeof(AbilityScore))]
    public class AbilityScore_PropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Create VisualElement to be the root of the property UI
            VisualElement container = new();

            // Create drawer UI with C#
            UnityEngine.UIElements.PopupWindow popup = new()
            {
                text = property.displayName,
            };


            popup.Add(new PropertyField(property.FindPropertyRelative("<Score>k__BackingField"), "Score"));

            container.Add(popup);

            return container;
        }
    }
}
