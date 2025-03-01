namespace TFPlay.Modules.Levels
{
    public interface ILevelsService
    {
        public int CurrentLevel { get; }
        public void Initialize();
        public void Next();
        public void Restart();
        public void ToLevel(int level);
    }
}