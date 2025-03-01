using System.Collections.Generic;

namespace TFPlay.Modules.SaveLoadSystem
{
    public interface IDataRegister
    {
        Dictionary<string, RootSaveData> RegisterData();
        TData RegisterData<TData>() where TData : RootSaveData, new();
    }
}