namespace TFPlay.Modules.SaveLoadSystem
{
    public class SaveLoadSystem : ISaveLoadSystem
    {
        private readonly IDataRegister _dataRegister;
        private readonly IDataFormatter _formatter;
        private readonly IDataFileStreamer _fileStreamer;
        private readonly IFilePathProvider _filePathProvider;
        private readonly ISaveDataProvider _dataProvider;

        public SaveLoadSystem(IDataRegister dataRegister,
            IDataFormatter formatter,
            IDataFileStreamer fileStreamer,
            IFilePathProvider filePathProvider,
            ISaveDataProvider dataProvider)
        {
            _dataRegister = dataRegister;
            _formatter = formatter;
            _fileStreamer = fileStreamer;
            _filePathProvider = filePathProvider;
            _dataProvider = dataProvider;
        }

        public void Save<TData>(string id) where TData : RootSaveData
        {
            SaveInternal(id, GetData<TData>(id));
        }

        public void SaveAll()
        {
            foreach (var data in _dataProvider.AllData)
                SaveInternal(data.Key, data.Value);
        }

        public TData GetData<TData>(string id) where TData : RootSaveData
        {
            return _dataProvider.GetData(id) as TData;
        }

        public void Load()
        {
            foreach (var registeredData in _dataRegister.RegisterData())
            {
                var path = _filePathProvider.GetFilePath(registeredData.Key, _formatter.FileExtension);
                var fileType = registeredData.Value.GetType();
                var file = _fileStreamer.Read(path);
                var loadedData = (RootSaveData)_formatter.Deserialize(file, fileType);
                
                _dataProvider.SetData(loadedData ?? registeredData.Value, registeredData.Key);
            }
        }

        public void ClearData<TData>(string id) where TData : RootSaveData, new() =>
            ClearDataInternal(id, _dataRegister.RegisterData<TData>());

        public void ClearAll()
        {
            foreach (var newClearedData in _dataRegister.RegisterData())
                ClearDataInternal(newClearedData.Key, newClearedData.Value);
        }

        private void SaveInternal(string id, RootSaveData data)
        {
            string filePath = _filePathProvider.GetFilePath(id, _formatter.FileExtension);
            var serializedData = _formatter.Serialize(data);
            _fileStreamer.Write(filePath, serializedData);
        }

        private void ClearDataInternal( string id, RootSaveData newSaveData)
        {
            _dataProvider.SetData(newSaveData, id);
            SaveInternal(id, newSaveData);
        }
    }
}