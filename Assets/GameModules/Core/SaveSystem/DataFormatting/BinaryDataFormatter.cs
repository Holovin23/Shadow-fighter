using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace TFPlay.Modules.SaveLoadSystem
{
    public class BinaryDataFormatter : IDataFormatter
    {
        private const string FILE_EXTENSION = ".dat";

        public string FileExtension => FILE_EXTENSION;
        
        public string Serialize(object data)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, data);
                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }

        public object Deserialize(string serializedData, Type type)
        {
            if (string.IsNullOrEmpty(serializedData))
                return null;
            
            byte[] bytes = Convert.FromBase64String(serializedData);
            using (MemoryStream memoryStream = new MemoryStream(bytes))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(memoryStream);
            }
        }
    }
}