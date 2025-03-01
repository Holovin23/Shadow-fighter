using System;
using UnityEngine;

namespace TFPlay.Modules.SaveLoadSystem
{
    public class JsonDataFormatter : IDataFormatter
    {
        private const string FILE_EXTENSION = ".json";

        public string FileExtension => FILE_EXTENSION;
        
        public string Serialize(object data)
        {
            return JsonUtility.ToJson(data);
        }

        public object Deserialize(string serializedData, Type type)
        {
            return JsonUtility.FromJson(serializedData, type);
        }
    }
}