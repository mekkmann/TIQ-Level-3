using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MonsterQuest
{
    // Using purely script to make the Inspector UI
    [CustomEditor(typeof(Car))]
    public class Car_Inspector : Editor
    {

        public override VisualElement CreateInspectorGUI()
        {
            // Create a new VisualElement to be the root of our Inspector UI
            VisualElement inspector = new();

            // Add a simple label
            inspector.Add(new Label("This is a custom Inspector!"));

            return inspector;
        }
    }
}
