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
            container.style.flexDirection = FlexDirection.Row;
            container.style.justifyContent = Justify.SpaceBetween;
            // Create drawer UI with C#
            _property = property;
            ScoreField = new(property.FindPropertyRelative("<Score>k__BackingField"), property.displayName);
            ScoreField.style.width = new StyleLength(Length.Percent(80f)); 
            ModifierLabel = new("(-10)");
            ModifierLabel.style.width = new StyleLength(Length.Percent(10f));
            container.Add(ScoreField);
            container.Add(ModifierLabel);
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
            int modifier = Mathf.FloorToInt((property.FindPropertyRelative("<Score>k__BackingField").intValue - 10) / 2);
            ModifierLabel.text = $"({(modifier < 0 ? "- " : "+ ")}{modifier})";
        }
    }
}
