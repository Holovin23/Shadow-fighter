#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

namespace Helpers
{
    public static class ProjectScriptableObjectFinder<T> where T : ScriptableObject
    {
        public static T GetFirstInstances(string specialName,string[] specialPath = null)
        {
            return GetAllInstances(specialPath).Find(x=>x.name==specialName);
        } public static T GetFirstInstances(string[] specialPath = null)
        {
            return GetAllInstances(specialPath).First();
        }
        public static List<T> GetAllInstances(string[] specialPath = null)
        {
            var assets = UnityEditor.AssetDatabase.FindAssets($"t: {typeof(T).Name}",specialPath).ToList();
            var paths = assets.Select( UnityEditor.AssetDatabase.GUIDToAssetPath);
            var objects = paths.Select(s =>  UnityEditor.AssetDatabase.LoadAssetAtPath(s, typeof(T)));
            List<T> convertedObjects = new List<T>();
            objects.ForEach(x => convertedObjects.Add((T)x));
            return convertedObjects;
        }
    }
}
#endif