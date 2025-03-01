namespace TFPlay.Modules.Core.Haptic
{
    public class TapticService : IHapticService
    {
        public void Enable(bool enable)
        {
            Taptic.TapticOn = enable;
        }

        public void Light()
        {
            Taptic.Light();
        }

        public void Medium()
        {
            Taptic.Medium();
        }

        public void Heavy()
        {
            Taptic.Heavy();
        }

        public void TapUI()
        {
            Taptic.Selection();
        }
    }
}