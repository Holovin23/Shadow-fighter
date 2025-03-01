using TFPlay.DeveloperUtilities;
using TFPlay.Infrastructure.StateMachine.Game;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class SceneCheatsHelper : MonoBehaviour
{
    [SerializeField] private Key winLevelKeyCode = Key.W;
    [SerializeField] private Key loseLevelKeyCode = Key.L;

    [Inject] private QAConsole _qaConsole;
    [Inject] private GameplayStateMachine _gameplayStateMachine;

#if UNITY_EDITOR
    private void Update()
    {
        if (Keyboard.current[winLevelKeyCode].wasPressedThisFrame)
        {
            WinUI();
        }

        if (Keyboard.current[loseLevelKeyCode].wasPressedThisFrame)
        {
            LoseUI();
        }
    }
#endif

    private void Start()
    {
        _qaConsole.SetSceneCheatHelper(this);
    }

    public void WinUI()
    {
        _gameplayStateMachine.Enter<WinGameState>();
    }

    public void LoseUI()
    {
        _gameplayStateMachine.Enter<LoseGameState>();
    }
}