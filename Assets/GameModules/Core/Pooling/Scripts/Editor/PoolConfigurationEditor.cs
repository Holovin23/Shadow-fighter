using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pooling
{
    [CustomEditor(typeof(PoolConfiguration))]
    public class PoolConfigurationEditor : UnityEditor.Editor
    {
        private PoolConfiguration _poolConfig;
        private SerializedProperty _prefabProperty;

        private Dictionary<string, Component> _componentsOnPrefabs = new();
        private Type[] _factories;
        private int _selectedComponentIndex;
        private int _selectedFactoryIndex;

        private void OnEnable()
        {
            _poolConfig = target as PoolConfiguration;
            _prefabProperty = serializedObject.FindProperty("_prefab");
            InitCurrentSelectedComponentType();
            InitCurrentSelectedFactoryType();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            UpdateComponentsDropDown();
            UpdateFactoriesDropDown();
        }

        private void InitCurrentSelectedComponentType()
        {
            if (_poolConfig.Prefab == null)
                return;

            GetAllComponentsOnPrefab();
            _selectedComponentIndex = Array.IndexOf(_componentsOnPrefabs.Values.ToArray(), _poolConfig.Prefab);
        }

        private void InitCurrentSelectedFactoryType()
        {
            _factories = Extensions.GetTypesImplementingInterface<IPoolableItemFactory>().ToArray();

            if (_poolConfig.FactoryTypeSerialized == string.Empty)
                return;

            _selectedFactoryIndex =
                Array.IndexOf(_factories, TypeSerializer.Deserialize(_poolConfig.FactoryTypeSerialized));
        }

        private void UpdateComponentsDropDown()
        {
            GetAllComponentsOnPrefab();

            int selectedIndex = EditorGUILayout.Popup("Poolable Component", _selectedComponentIndex,
                _componentsOnPrefabs.Keys.ToArray());

            if (selectedIndex != _selectedComponentIndex)
                SetAssetDirty();

            _selectedComponentIndex = selectedIndex;

            if (_selectedComponentIndex > _componentsOnPrefabs.Count - 1)
                return;

            _poolConfig.Prefab = _componentsOnPrefabs[_componentsOnPrefabs.Keys.ElementAt(_selectedComponentIndex)];
        }

        private void UpdateFactoriesDropDown()
        {
            int selectedIndex = EditorGUILayout.Popup("Poolable Factory", _selectedFactoryIndex,
                _factories.Select(factory => factory.Name).ToArray());

            if (selectedIndex != _selectedFactoryIndex)
                SetAssetDirty();

            _selectedFactoryIndex = selectedIndex;

            _poolConfig.FactoryTypeSerialized = TypeSerializer.Serialize(_factories[_selectedFactoryIndex]);
        }

        private void GetAllComponentsOnPrefab()
        {
            _componentsOnPrefabs.Clear();

            GameObject prefab = (_prefabProperty.objectReferenceValue as GameObject);
            if (prefab == null)
                return;

            Component[] components = prefab.GetComponents<Component>();

            foreach (Component component in components)
            {
                _componentsOnPrefabs[component.GetType().Name] = component;
            }
        }

        private void SetAssetDirty() => EditorUtility.SetDirty(_poolConfig);
    }
}