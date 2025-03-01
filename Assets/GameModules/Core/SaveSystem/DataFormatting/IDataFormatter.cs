using System;

namespace TFPlay.Modules.SaveLoadSystem
{
    public interface IDataFormatter
    {
        string FileExtension { get; }
        string Serialize(object data);
        object Deserialize(string serializedData, Type type);
    }
}