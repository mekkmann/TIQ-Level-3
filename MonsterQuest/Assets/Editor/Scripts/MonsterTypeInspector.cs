using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MonsterQuest
{
    [CustomEditor(typeof(MonsterType))]
    public class MonsterTypeInspector : Editor
    {
        public VisualTreeAsset Inspector_UXML;
        public override VisualElement CreateInspectorGUI()
        {
            // Create a new VisualElement to be the root of our Inspector UI
            VisualElement inspector = new();

            // Load from UXML
            Inspector_UXML.CloneTree(inspector);

            // Return custom Inspector
            return inspector;
        }
    }
}
