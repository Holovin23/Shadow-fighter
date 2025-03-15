using UnityEngine;

public static partial class Extensions
{
    public static T Copy<T>(this T scriptableObject) where T : ScriptableObject
    {
        T clone = ScriptableObject.Instantiate(scriptableObject);
        return clone;
    }
}