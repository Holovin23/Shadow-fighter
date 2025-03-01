using TFPlay.Modules.Core.TickService;
using Zenject;

namespace TFPlay.Modules.Delays
{
    public class Repeat : Delay
    {
        public override float RemainingTime => CompleteTime - TickService.Time;

        private SignalBus _signalBus;

        public Repeat(ITickService tickService, SignalBus signalBus) : base(tickService)
        {
            _signalBus = signalBus;
        }

        public override void Start()
        {
            base.Start();
            CompleteTime = TickService.Time + Time;
            _signalBus.Subscribe<FixedTickSignal>(Update);
        }

        public override void Stop()
        {
            base.Stop();
            _signalBus.Unsubscribe<FixedTickSignal>(Update);
        }

        private void Update(FixedTickSignal data)
        {
            if (IsComplete || TickService.Time < CompleteTime)
                return;

            Complete();
        }

        public override void Complete()
        {
            base.Complete();
            CompleteTime += Time;
        }
    }
}