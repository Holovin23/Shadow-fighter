using System.Collections.Generic;

namespace TFPlay.Modules.SaveLoadSystem
{
    public abstract class BaseDataRegister : IDataRegister
    {
        public abstract Dictionary<string, RootSaveData> RegisterData();
        
        public TData RegisterData<TData>() where TData : RootSaveData, new()
        {
            return new TData();
        }
    }
}