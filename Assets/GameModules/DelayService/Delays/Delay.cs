using System;
using TFPlay.Modules.Core.TickService;
using UnityEngine;

namespace TFPlay.Modules.Delays
{
    public abstract class Delay : IDelay
    {
        private event Action OnComplete;
        private event Action OnStop;

        public virtual float RemainingTime => CompleteTime - TickService.Time;
        public bool IsComplete { get; protected set; }

        protected float CompleteTime;
        protected float Time;

        protected ITickService TickService;

        public Delay(ITickService tickService)
        {
            TickService = tickService;
        }

        public virtual void Init(float time, Action onComplete, Action onStop)
        {
            Time = time;
            OnComplete = onComplete;
            OnStop = onStop;
        }

        public virtual void Start()
        {
            IsComplete = false;
        }

        public virtual void Stop()
        {
            OnStop?.Invoke();
        }

        public virtual void Complete()
        {
            OnComplete?.Invoke();
        }
    }
}