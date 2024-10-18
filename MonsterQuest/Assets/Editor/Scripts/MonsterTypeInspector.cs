using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MonsterQuest
{
    [CustomEditor(typeof(MonsterType))]
    //public class MonsterTypeInspector : Editor
    //{
    //    public string[] Options = MonsterTypeImporter.MonsterIndexNames;
    //    public int SelectedIndex = -1;
    //    public VisualTreeAsset Inspector_UXML;
    //    public override VisualElement CreateInspectorGUI()
    //    {
    //        // Create a new VisualElement to be the root of our Inspector UI
    //        VisualElement inspector = new();
    //        // Load from UXML
    //        Inspector_UXML.CloneTree(inspector);
    //        //OnInspectorGUI();

    //        // Return custom Inspector
    //        return inspector;
    //    }
    //    //public override void OnInspectorGUI()
    //    //{
    //    //    SelectedIndex = EditorGUILayout.Popup("Load Monster", SelectedIndex, Options);
    //    //    DrawDefaultInspector();
    //    //}
    //}
    public class MonsterTypeInspector : Editor
    {
        public string[] Options = MonsterTypeImporter.MonsterIndexNames;
        public int SelectedIndex = -1;
        public override void OnInspectorGUI()
        {
            SelectedIndex = EditorGUILayout.Popup("Load Monster", SelectedIndex, Options);
            DrawDefaultInspector();
        }
    }
}
