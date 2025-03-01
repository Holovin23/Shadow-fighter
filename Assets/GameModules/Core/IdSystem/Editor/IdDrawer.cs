using System.Collections.Generic;
using Helpers;
using UnityEngine;
using UnityEditor;

namespace TFPlay.Modules.Core.IdSystem
{
    [CustomPropertyDrawer(typeof(IdAttribute))]
    public class IdDrawer : PropertyDrawer
    {
        private UnityEditor.IMGUI.Controls.AdvancedDropdownState _dropdownState;
        private SerializedObject _serializedObject;
        private SerializedProperty _property;
        private GlobalIdsHolder _globalIdsHolder;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _serializedObject = property.serializedObject;
            _property = property;

            var labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);
            EditorGUI.PrefixLabel(labelRect, label);

            var buttonWidth = position.width - EditorGUIUtility.labelWidth;
            Rect buttonRect = new Rect(position.x + EditorGUIUtility.labelWidth, position.y, buttonWidth, position.height);

            GUIStyle yellowBackgroundStyle = new GUIStyle(GUI.skin.button);
            yellowBackgroundStyle.normal.background = MakeBackgroundTexture(10, 10, Color.black);

            if (GUI.Button(buttonRect, new GUIContent(property.stringValue), yellowBackgroundStyle))
            {
                _dropdownState ??= new UnityEditor.IMGUI.Controls.AdvancedDropdownState();
                _globalIdsHolder ??= ProjectScriptableObjectFinder<GlobalIdsHolder>.GetFirstInstances();
                Dictionary<string, List<string>> data = new Dictionary<string, List<string>>();

                if (_globalIdsHolder != null)
                {
                    foreach (var category in _globalIdsHolder.IdCategories)
                    {
                        if (category.Ids.Count == 0) continue;

                        var groupName = category.name;
                        data.Add(groupName, new List<string>());

                        foreach (var itemId in category.Ids)
                            data[groupName].Add(itemId);
                    }
                }

                var dropdown = new FileDropdown(_dropdownState, data, OnSelectValue);
                dropdown.Show(buttonRect);
            }
        }

        private void OnSelectValue(FileDropdown.CallbackInfo info)
        {
            _serializedObject.Update();
            var propertyToModify = _serializedObject.FindProperty(_property.propertyPath);
            propertyToModify.stringValue = info.name;
            _serializedObject.ApplyModifiedProperties();
        }

        private Texture2D MakeBackgroundTexture(int width, int height, Color color)
        {
            Color[] pixels = new Color[width * height];

            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }

            Texture2D backgroundTexture = new Texture2D(width, height);
            backgroundTexture.SetPixels(pixels);
            backgroundTexture.Apply();

            return backgroundTexture;
        }
    }
}