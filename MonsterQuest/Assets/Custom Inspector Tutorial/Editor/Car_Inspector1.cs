using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace MonsterQuest
{
    // Using the UI Builder to make the Inspector UI
    [CustomEditor(typeof(Car1))]
    public class Car_Inspector1 : Editor
    {
        public VisualTreeAsset InspectorUXML;
        public override VisualElement CreateInspectorGUI()
        {
            // Create a new VisualElement to be the root of our Inspector UI
            VisualElement inspector = new();

            // Add a simple label
            inspector.Add(new Label("This label is from script"));

            // Load from UXML
            InspectorUXML.CloneTree(inspector);

            // Reference to default inspector foldout control
            VisualElement inspectorFoldout = inspector.Q("Default_Inspector");

            // Attach a default Inspector to the foldout
            InspectorElement.FillDefaultInspector(inspectorFoldout, serializedObject, this);

            return inspector;
        }
    }
}
