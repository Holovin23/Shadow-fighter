namespace TFPlay.Modules.SaveLoadSystem
{
    public static class SaveSystemExtensions
    {
        public static bool TryRead(this IDataFileStreamer fileStreamer, string filePath, out string data)
        {
            data = fileStreamer.Read(filePath);
            return data != null;
        }

        public static string Serialize<T>(this IDataFormatter formatter, T data) =>
            formatter.Serialize(data);

        public static T Deserialize<T>(this IDataFormatter formatter, string serializedData) =>
            (T)formatter.Deserialize(serializedData, typeof(T));

        public static string GetFilePath(this IFilePathProvider filePathProvider, string id, string fileFormat) =>
            filePathProvider.GetFilePath(id, fileFormat);
    }
}