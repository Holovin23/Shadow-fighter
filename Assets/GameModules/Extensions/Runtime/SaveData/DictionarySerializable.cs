using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DictionarySerializable<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<KeyValueData> keyValues = new List<KeyValueData>();

    public void OnBeforeSerialize()
    {
        keyValues.Clear();
        foreach (KeyValuePair<TKey, TValue> pair in this)
            keyValues.Add(new KeyValueData(pair.Key, pair.Value));
    }

    public void OnAfterDeserialize()
    {
        this.Clear();

        for (int i = 0; i < keyValues.Count; i++)
            this.Add(keyValues[i].Key, keyValues[i].Value);
    }

    [Serializable]
    private class KeyValueData
    {
        public TKey Key;
        public TValue Value;

        public KeyValueData(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}