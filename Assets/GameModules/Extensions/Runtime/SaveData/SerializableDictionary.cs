using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Serialization;

[System.Serializable]
public class SerializableDictionary<TKey, TValue>
{
    public List<TKey> Keys;
    public List<TValue> Values;

    public SerializableDictionary(Dictionary<TKey, TValue> dictionary)
    {
        Keys = dictionary.Keys.ToList();
        Values = dictionary.Values.ToList();
    }

    public Dictionary<TKey, TValue> GetDictionary()
    {
        var result = new Dictionary<TKey, TValue>();

        for (var i = 0; i < Keys.Count; i++) 
            result[Keys[i]] = Values[i];

        return result;
    }
}