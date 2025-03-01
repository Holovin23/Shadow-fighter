namespace TFPlay.Infrastructure.StateMachine.Application
{
    public class ApplicationStateMachine : StateMachine
    {
        private ApplicationStatesFactory _applicationStatesFactory;

        public ApplicationStateMachine(ApplicationStatesFactory applicationStatesFactory)
        {
            _applicationStatesFactory = applicationStatesFactory;
        }

        public override void Initialize()
        {
            _states = _applicationStatesFactory.CreateStates();
        }
    }
}