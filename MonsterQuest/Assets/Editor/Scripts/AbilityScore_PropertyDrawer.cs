using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using Codice.Client.BaseCommands;
using Newtonsoft.Json;
using System.Linq;
using System.Reflection;

namespace MonsterQuest
{
    [CustomPropertyDrawer(typeof(AbilityScore))]
    public class AbilityScore_PropertyDrawer : PropertyDrawer
    {
        private SerializedProperty _property;
        public PropertyField ScoreField;
        public Label ModifierLabel;
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Create VisualElement to be the root of the property UI
            VisualElement container = new();

            // Create drawer UI with C#
            UnityEngine.UIElements.PopupWindow popup = new()
            {
                text = property.displayName,
            };
            _property = property;
            ScoreField = new(property.FindPropertyRelative("<Score>k__BackingField"), "Score");
            popup.Add(ScoreField);
            ModifierLabel = new("(-10)");
            popup.Add(ModifierLabel);
            container.Add(popup);
            OnValueChanged();

            return container;
        }

        public void OnValueChanged()
        {
            ModifierLabel.Unbind();

            ModifierLabel.TrackPropertyValue(_property, GetModifier);

            ModifierLabel.BindProperty(_property);

            GetModifier(_property);

        }

        private void GetModifier(SerializedProperty property)
        {
            // works, but can I do it with the getter of Modifier???
            ModifierLabel.text = $"({Mathf.FloorToInt((property.FindPropertyRelative("<Score>k__BackingField").intValue - 10) / 2)})";

            // Test


        }
    }
}
