namespace TFPlay.Infrastructure.StateMachine.Game
{
   public class GameplayStateMachine : StateMachine
    {
        private GameplayStatesFactory _gameplayStatesFactory;

        public GameplayStateMachine(GameplayStatesFactory gameplayStatesFactory)
        {
            _gameplayStatesFactory = gameplayStatesFactory;
        }

        public override void Initialize()
        {
            _states = _gameplayStatesFactory.CreateStates();
        }
    }
}