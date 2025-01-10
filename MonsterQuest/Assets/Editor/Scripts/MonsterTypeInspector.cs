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
            AddMonsterImportDropdown(root);

            AddUXML("MonsterType_Inspector", root);

            //InspectorElement.FillDefaultInspector(root, serializedObject, this);

            return root;
        }

        private void AddMonsterImportDropdown(VisualElement root)
        {
            UnityEngine.UIElements.PopupWindow popup = new() 
            { 
                text = "Monster Import"
            };
            DropdownField dropDown = new();
            dropDown.choices.AddRange(MonsterTypeImporter.MonsterIndexNames);
            dropDown.RegisterValueChangedCallback(OnDropdownValueChanged);
            popup.Add(dropDown);

            root.Add(popup);
        }

        private void OnDropdownValueChanged(ChangeEvent<string> changeEvent)
        {
            MonsterTypeImporter.ImportData(changeEvent.newValue, serializedObject.targetObject as MonsterType);
        }

        private void AddUXML(string uxmlName, VisualElement root)
        {
            VisualTreeAsset uiAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"Assets/Editor/{uxmlName}.uxml");
            VisualElement ui = uiAsset.Instantiate();

            root.Add(ui);
        }
    }
}
