using System;

namespace TFPlay.Modules.Delays
{
    public interface IDelay
    {
        public float RemainingTime { get; }
        public bool IsComplete { get; }
        public void Init(float time, Action onComplete, Action onStop);
        public void Start();
        public void Stop();
        public void Complete();
    }
}