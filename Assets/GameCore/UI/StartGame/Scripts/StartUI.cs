using TFPlay.Infrastructure.StateMachine.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TFPlay.UI
{
    public class StartUI : BaseUI
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private TMP_Text _continueText;

        [Inject] private GameplayStateMachine _gameplayStateMachine;

        public override void Initialize()
        {
            base.Initialize();
            _startButton.onClick.AddListener(StartClicked);
        }

        private void StartClicked()
        {
            _gameplayStateMachine.Enter<GameState>();
            Hide();
        }
    }
}