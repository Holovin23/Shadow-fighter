using System.IO;
using UnityEngine;

namespace TFPlay.Modules.SaveLoadSystem
{
    public class FileDataStreamer : IDataFileStreamer
    {
        public void Write(string filePath, string data)
        {
            try
            {
                File.WriteAllText(filePath, data);
            }
            catch (IOException ex)
            {
                Debug.LogError($"FileDataStreamer: Error writing to file at {filePath}. Exception: {ex.Message}");
            }
        }

        public string Read(string filePath)
        {
            try
            {
                return File.Exists(filePath) ? File.ReadAllText(filePath) : null;
            }
            catch (IOException ex)
            {
                Debug.LogError($"FileDataStreamer: Error reading from file at {filePath}. Exception: {ex.Message}");
                return null;
            }
        }

        public void Delete(string filePath)
        {
            if(File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}