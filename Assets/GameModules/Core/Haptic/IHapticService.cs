namespace TFPlay.Modules.Core.Haptic
{
    public interface IHapticService
    {
        public void Enable(bool enable);
        
        public void Light();
        public void Medium();
        public void Heavy();

        public void TapUI();
    }
}