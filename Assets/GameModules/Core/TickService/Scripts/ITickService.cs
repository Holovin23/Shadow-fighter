namespace TFPlay.Modules.Core.TickService
{
    public interface ITickService
    {
        /// <summary>
        /// Total time passed with multiplier
        /// </summary>
        public float Time { get; }
        
        /// <summary>
        /// Total time passed without multiplier
        /// </summary>
        public float TimeReal { get; }
        
        /// <summary>
        /// Delta time multiplier 
        /// </summary>
        public float TimeSpeed { get; }

        /// <summary>
        /// While paused, delta time sent will be equal zero.
        /// </summary>
        public bool IsPaused { get; }

        /// <summary>
        /// Multiplies time delta by given value.
        /// </summary>
        /// <param name="speed"> delta multiplier.</param>
        public void SetTimeSpeed(float speed);

        /// <summary>
        /// Set delta multiplier to 1
        /// </summary>
        public void ResetTimeSpeed();

        /// <summary>
        /// Set on pause. While paused, delta time sent will be equal zero. 
        /// </summary>
        public void Pause();

        /// <summary>
        /// Remove pause. While paused, delta time sent will be zero. 
        /// </summary>
        public void Unpause();
    }
}