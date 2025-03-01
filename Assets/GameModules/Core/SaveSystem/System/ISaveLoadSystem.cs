namespace TFPlay.Modules.SaveLoadSystem
{
    public interface ISaveLoadSystem
    {
        void Save<TData>(string id) where TData : RootSaveData;
        void SaveAll();
        public TData GetData<TData>(string id) where TData : RootSaveData;
        void Load();
        void ClearData<TData>(string id) where TData : RootSaveData, new();
        void ClearAll();
    }
}