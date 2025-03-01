using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TFPlay.Modules.SaveLoadSystem
{
    public class FilePathProvider : IFilePathProvider
    {
        private readonly Dictionary<string, string> _filePathCache = new();
        
        public string GetFilePath(string id, string fileFormat)
        {
            if (!_filePathCache.TryGetValue(id, out string filePath)) 
                filePath = AddPath(id, fileFormat);

            return filePath;
        }

        private string AddPath(string  id, string fileFormat)
        {
            string fileName = id;
            string filePath = $"{Application.persistentDataPath}/{fileName}{fileFormat}";
            _filePathCache[id] = filePath;
            return filePath;
        }
    }
}