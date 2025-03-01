namespace TFPlay.Modules.SaveLoadSystem
{
    public interface IFilePathProvider
    {
        string GetFilePath(string id, string fileFormat);
    }
}