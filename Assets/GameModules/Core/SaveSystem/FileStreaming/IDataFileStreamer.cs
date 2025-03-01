namespace TFPlay.Modules.SaveLoadSystem
{
    public interface IDataFileStreamer
    {
        void Write(string filePath, string data);
        string Read(string filePath);
        void Delete(string filePath);
    }
}