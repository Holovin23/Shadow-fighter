using System;
using TFPlay.Modules.SaveLoadSystem;
using TFPlay.Modules.SaveLoadSystem.Data;
using TFPlay.Infrastructure.StateMachine.Application;
using UnityEngine.SceneManagement;
using Zenject;

namespace TFPlay.Modules.Levels
{
    public class LevelsService : ILevelsService
    {
        private ISaveLoadSystem _saveLoadSystem;
        private ApplicationStateMachine _applicationStateMachine;

        public Action OnProressChanged;

        private int LevelsCount = SceneManager.sceneCountInBuildSettings - 1;

        public int CurrentLevel
        {
            get => _currentLevel;
            private set
            {
                _currentLevel = value;
                OnProressChanged?.Invoke();
            }
        }

        private int _currentLevel;

        public LevelsService(ISaveLoadSystem saveLoadSystem, ApplicationStateMachine applicationStateMachine)
        {
            _saveLoadSystem = saveLoadSystem;
            _applicationStateMachine = applicationStateMachine;
        }

        public void Initialize()
        {
            CurrentLevel = _saveLoadSystem.GetData<LevelsSaveData>(SaveDataIds.LEVELS).Level;
            OnProressChanged += Save;
        }

        public void Next()
        {
            ToLevel(CurrentLevel + 1);
        }

        public void Restart()
        {
            ToLevel(CurrentLevel);
        }

        public void ToLevel(int level)
        {
            CurrentLevel = level;
            _applicationStateMachine.Enter<LoadingSceneApplicationState, int>(GetLevelScene(CurrentLevel));
        }

        private void Save()
        {
            _saveLoadSystem.GetData<LevelsSaveData>(SaveDataIds.LEVELS).Level = CurrentLevel;
            _saveLoadSystem.Save<LevelsSaveData>(SaveDataIds.LEVELS);
        }

        private int GetLevelScene(int levelNumber)
        {
            var levelIndex = levelNumber - 1;
            levelIndex = levelIndex % LevelsCount;
            return levelIndex + 1;
        }
    }
}