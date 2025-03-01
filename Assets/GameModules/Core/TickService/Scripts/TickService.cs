using UnityEngine;
using Zenject;

namespace TFPlay.Modules.Core.TickService
{
    public class TickService : MonoBehaviour, ITickService
    {
        public float Time { get; private set; }
        public float TimeReal => UnityEngine.Time.unscaledTime;

        public float TimeSpeed => IsPaused ? 0f : _timeSpeed;

        public bool IsPaused { get; private set; }

        private float _timeSpeed = 1f;

        [Inject] private SignalBus _signalBus;

        public void SetTimeSpeed(float speed)
        {
            _timeSpeed = speed;
        }

        public void ResetTimeSpeed()
        {
            _timeSpeed = 1f;
        }

        public void Pause()
        {
            IsPaused = true;
        }

        public void Unpause()
        {
            IsPaused = false;
        }

        private void Update()
        {
            var delta = CalculateDelta(UnityEngine.Time.deltaTime);

            _signalBus.Fire(new TickSignal(delta, UnityEngine.Time.unscaledDeltaTime));
            Time += delta;
        }

        private void FixedUpdate()
        {
            var delta = CalculateDelta(UnityEngine.Time.fixedTime);
            _signalBus.Fire(new FixedTickSignal(delta, UnityEngine.Time.fixedTime));
        }

        private float CalculateDelta(float delta)
        {
            return IsPaused ? 0 : delta * TimeSpeed;
        }
    }
}