using UnityEngine;
using Zenject;

namespace TFPlay.DeveloperUtilities
{
//TODO Break installer for each qa system after refactoring 
    public class QaInstaller : MonoInstaller
    {
        [SerializeField] private QAConsole _qaConsole;
        [SerializeField] private DeveloperPanel _developerPanel;
        [SerializeField] private PerfomanceTester _perfomanceTester;

        public override void InstallBindings()
        {
            Container.Bind<QAConsole>().FromInstance(_qaConsole).AsSingle();
            Container.Bind<DeveloperPanel>().FromInstance(_developerPanel).AsSingle();
            Container.Bind<PerfomanceTester>().FromInstance(_perfomanceTester).AsSingle();
        }
    }
}