using System;
using Zenject;

namespace TFPlay.Modules.Delays
{
    public class DelayFactory
    {
        private DiContainer _container;

        public DelayFactory(DiContainer container)
        {
            _container = container;
        }

        public IDelay CreateDelay<T>(float time, Action onEnd, Action onStop) where T : IDelay
        {
            var delay = _container.Instantiate<T>();
            delay.Init(time, onEnd, onStop);
            return delay;
        }
    }
}