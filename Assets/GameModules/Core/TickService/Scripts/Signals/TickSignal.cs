namespace TFPlay.Modules.Core.TickService
{
    public struct TickSignal
    {
        public readonly float Delta;
        public readonly float DeltaRealTime;

        public TickSignal(float delta, float deltaRealTime)
        {
            Delta = delta;
            DeltaRealTime = deltaRealTime;
        }
    }
}