using UnityEngine;

namespace TFPlay.Modules.SaveLoadSystem
{
    public class PlayerPrefsDataStreamer : IDataFileStreamer
    {
        public void Write(string filePath, string data)
        {
            PlayerPrefs.SetString(filePath, data);
            PlayerPrefs.Save();
        }

        public string Read(string filePath) => 
            PlayerPrefs.HasKey(filePath) ? PlayerPrefs.GetString(filePath) : null;

        public void Delete(string filePath) => 
            PlayerPrefs.DeleteKey(filePath);
    }
}