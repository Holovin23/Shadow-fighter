using TFPlay.Infrastructure.StateMachine.Game;
using TFPlay.UI;
using TFPlay.UI.SettingsUI;
using Zenject;

namespace TFPlay.Infrastructure.GameScene
{
    public class GameSceneInitializer : IInitializable
    {
        private readonly GameplayStateMachine _gameplayStateMachine;
        private readonly StartUI _startUI;
        private readonly IHud _hud;
        private readonly ISettingsUiService _settingsUi;

        public GameSceneInitializer(GameplayStateMachine gameplayStateMachine, StartUI startUI, IHud hud, ISettingsUiService settingsUi)
        {
            _gameplayStateMachine = gameplayStateMachine;
            _startUI = startUI;
            _hud = hud;
            _settingsUi = settingsUi;
        }

        public void Initialize()
        {
            _hud.Initialize();
            _settingsUi.Initialize();
            _startUI.Initialize();

            _gameplayStateMachine.Initialize();
            _gameplayStateMachine.Enter<GameState>();
        }
    }
}