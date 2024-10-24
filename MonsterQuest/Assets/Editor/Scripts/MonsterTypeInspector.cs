using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace MonsterQuest
{
    [CustomEditor(typeof(MonsterType))]
    public class MonsterTypeInspector : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new();

            VisualElement importContainer = new();
            importContainer.Add(new Label("Import Monster"));
            DropdownField dropDown = new();
            dropDown.choices.AddRange(MonsterTypeImporter.MonsterIndexNames);
            dropDown.RegisterValueChangedCallback(OnDropdownValueChanged);
            importContainer.Add(dropDown);

            root.Add(importContainer);

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            return root;
        }

        private void OnDropdownValueChanged(ChangeEvent<string> changeEvent)
        {
            MonsterTypeImporter.ImportData(changeEvent.newValue, serializedObject.targetObject as MonsterType);
        }
    }
}
