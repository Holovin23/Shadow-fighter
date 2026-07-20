using System;
using TFPlay.Modules.Levels;
using Zenject;

namespace GameModules.Gameplay.Scripts.GameLoop
{
    public class ExperienceService : IExperienceService
    {
        [Inject] 
        private ILevelsService _levelsService;
        public event Action<int, int> OnExperienceChange;
        public event Action<int, int> OnLevelUp;
        
        private int _currentExpirience;
        private int _level = 0;
        
        public void AddExperience(int amount)
        {
            _currentExpirience += amount;
            OnExperienceChange.Invoke(_currentExpirience, _level);
            //if(_levelsService.GetCurrentConfig)
        }

    }
}