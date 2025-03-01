using System;

namespace TFPlay.Modules.Delays
{
    public interface IDelayService
    {
        public void Initialize();
        public Guid Do<T>(float time, Action onEnd, Action onStop = null) where T : IDelay;
        public void Stop(Guid guid, bool complete = false);
        public void StopAll(bool complete = false);
        public float RemainingTime(Guid guid);
    }
}