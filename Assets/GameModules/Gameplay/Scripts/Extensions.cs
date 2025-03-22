using System.Collections.Generic;
using UnityEngine;

public static partial class Extensions
{
    public static T Copy<T>(this T scriptableObject) where T : ScriptableObject
    {
        T clone = ScriptableObject.Instantiate(scriptableObject);
        return clone;
    }
    
    public static T GetNotRepeatItem<T>(this List<T> pool, ref List<T> tempPool, int nonRepeatCount, bool isRandom = true)
    {
        if (pool.Count == 0)
        {
            if (tempPool.Count > 0)
            {
                pool.Add(tempPool[0]);
                tempPool.Remove(tempPool[0]);
            }
            else
                Debug.LogError($"It's impossible to get a random element from empty list.");
        }

        int itemIndex = 0;
        if(isRandom)
            itemIndex = Random.Range(0, pool.Count);
        var randomItem = pool[itemIndex];

        if (tempPool.Count > nonRepeatCount)
        {
            pool.Add(tempPool[0]);
            tempPool.Remove(tempPool[0]);
        }

        tempPool.Add(randomItem);
        pool.Remove(randomItem);
       
        return randomItem;
    }
}