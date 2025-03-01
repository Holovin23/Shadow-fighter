namespace TFPlay.Modules.Core.TickService
{
    public struct FixedTickSignal
    {
        public readonly float Delta;
        public readonly float DeltaRealTime;

        
        public FixedTickSignal(float delta, float deltaRealTime)
        {
            Delta = delta;
            DeltaRealTime = deltaRealTime;
        }
    }
}