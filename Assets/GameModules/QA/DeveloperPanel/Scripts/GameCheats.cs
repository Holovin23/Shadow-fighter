using TFPlay.Modules.GameResources;
using TFPlay.Modules.Levels;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace TFPlay.DeveloperUtilities
{
    public class GameCheats : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private Key addCoinsKeyCode = Key.C;
        [SerializeField] private Key addResource1Code = Key.Digit1;
        [SerializeField] private Key addResource2Code = Key.Digit2;
        [SerializeField] private Key addResource3Code = Key.Digit3;
        [SerializeField] private Key addResource4Code = Key.Digit4;
        [SerializeField] private Key addResource5Code = Key.Digit5;
        [SerializeField] private Key nextLevelKeyCode = Key.N;
        [SerializeField] private Key restartLevelKeyCode = Key.R;
        [SerializeField] private Key qaConsoleKeyCode = Key.Q;

        [Inject] private DeveloperPanel _developerPanel;
        [Inject] private IGameResourcesService _gameResourcesService;
        [Inject] private ILevelsService _progressService;

        private void Update()
        {
            if (Keyboard.current[addCoinsKeyCode].wasPressedThisFrame)
            {
                _gameResourcesService.AddResource(ResourceType.Coins, 10000);
            }

            if (Keyboard.current[addResource1Code].wasPressedThisFrame)
            {
                _gameResourcesService.AddResource((ResourceType)1, 10000);
            }

            if (Keyboard.current[addResource2Code].wasPressedThisFrame)
            {
                _gameResourcesService.AddResource((ResourceType)2, 10000);
            }

            if (Keyboard.current[addResource3Code].wasPressedThisFrame)
            {
                _gameResourcesService.AddResource((ResourceType)3, 10000);
            }

            if (Keyboard.current[addResource4Code].wasPressedThisFrame)
            {
                _gameResourcesService.AddResource((ResourceType)4, 10000);
            }

            if (Keyboard.current[addResource5Code].wasPressedThisFrame)
            {
                _gameResourcesService.AddResource((ResourceType)5, 10000);
            }

            if (Keyboard.current[nextLevelKeyCode].wasPressedThisFrame)
            {
                _progressService.Next();
            }

            if (Keyboard.current[restartLevelKeyCode].wasPressedThisFrame)
            {
                _progressService.Restart();
            }

            if (Keyboard.current[qaConsoleKeyCode].wasPressedThisFrame)
            {
                _developerPanel.ShowTools();
            }
        }
#endif
    }
}