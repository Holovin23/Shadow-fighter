using System;
using System.Collections.Generic;
using Zenject;

namespace TFPlay.Infrastructure.StateMachine.Game
{
    public class GameplayStatesFactory
    {
        private readonly DiContainer _container;

        public GameplayStatesFactory(DiContainer container)
        {
            _container = container;
        }

        public Dictionary<Type, IExitableState> CreateStates() => new Dictionary<Type, IExitableState>
        {
            { typeof(StartGameState), _container.Instantiate<StartGameState>() },
            { typeof(GameState), _container.Instantiate<GameState>() },
            { typeof(WinGameState), _container.Instantiate<WinGameState>() },
            { typeof(LoseGameState), _container.Instantiate<LoseGameState>() },
        };
    }
}