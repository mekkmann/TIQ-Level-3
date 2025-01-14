using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MonsterQuest
{
    [CustomPropertyDrawer(typeof(AbilityScores))]
    public class AbilityScores_PropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new();


            UnityEngine.UIElements.PopupWindow popup = new() { text = "Ability Scores" };

            AddAbilityScoreLine("Strength", popup, property);
            AddAbilityScoreLine("Dexterity", popup, property);
            AddAbilityScoreLine("Constitution", popup, property);
            AddAbilityScoreLine("Wisdom", popup, property);
            AddAbilityScoreLine("Charisma", popup, property);
            AddAbilityScoreLine("Intelligence", popup, property);
            root.Add(popup);

            return root; 
        }

        private void AddAbilityScoreLine(string abilityScoreName, UnityEngine.UIElements.PopupWindow popup, SerializedProperty serializedProperty)
        {
            SerializedProperty abilityScoreProperty = serializedProperty.FindPropertyRelative($"<{abilityScoreName}>k__BackingField");

            AbilityScore_PropertyDrawer newDrawer = new();
            popup.Add(newDrawer.CreatePropertyGUI(abilityScoreProperty));
        }
    }
}
