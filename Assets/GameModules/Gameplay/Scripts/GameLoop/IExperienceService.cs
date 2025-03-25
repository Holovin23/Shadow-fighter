using System;

namespace GameModules.Gameplay.Scripts.GameLoop
{
    public interface IExperienceService
    {
        public event Action<int,int> OnExperienceChange;
        public event Action<int, int> OnLevelUp;

        public void AddExperience(int amount);
    }
}