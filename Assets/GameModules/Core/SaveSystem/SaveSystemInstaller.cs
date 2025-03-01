using TFPlay.Modules.SaveLoadSystem.Data;
using Zenject;

namespace TFPlay.Modules.SaveLoadSystem
{
    public class SaveSystemInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IDataRegister>().To<DataRegister>().AsSingle();
            Container.Bind<IFilePathProvider>().To<FilePathProvider>().AsSingle();
            Container.Bind<ISaveDataProvider>().To<SaveDataProvider>().AsSingle();

#if UNITY_EDITOR
            Container.Bind<IDataFormatter>().To<JsonDataFormatter>().AsSingle();
#else
            Container.Bind<IDataFormatter>().To<BinaryDataFormatter>().AsSingle();
#endif

            Container.Bind<IDataFileStreamer>().To<FileDataStreamer>().AsSingle();

            Container.Bind<ISaveLoadSystem>().To<SaveLoadSystem>().AsSingle().NonLazy();
        }
    }
}