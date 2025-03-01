using TFPlay.Modules.Core.TickService;
using Zenject;

namespace TFPlay.Modules.Delays
{
    public class AfterRealTime : Delay
    {
        public override float RemainingTime => CompleteTime - TickService.TimeReal;

        private SignalBus _signalBus;

        public AfterRealTime(ITickService tickService, SignalBus signalBus) : base(tickService)
        {
            _signalBus = signalBus;
        }

        public override void Start()
        {
            base.Start();
            CompleteTime = TickService.TimeReal + Time;
            _signalBus.Subscribe<TickSignal>(Update);
        }

        public override void Stop()
        {
            base.Stop();
            _signalBus.Unsubscribe<TickSignal>(Update);
        }

        private void Update(TickSignal data)
        {
            if (IsComplete || TickService.TimeReal < CompleteTime)
                return;

            Complete();
        }

        public override void Complete()
        {
            base.Complete();
            IsComplete = true;
            _signalBus.Unsubscribe<TickSignal>(Update);
        }
    }
}