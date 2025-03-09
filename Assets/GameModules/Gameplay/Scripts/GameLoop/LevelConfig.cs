using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameModules.Gameplay.Scripts.GameLoop
{
    [CreateAssetMenu(menuName = "ScriptableObject/LevelConfig" , fileName = nameof(LevelConfig))]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] public List<WaveData> waves;
        [SerializeField] public float timeToFirstWave;
    }


    [Serializable]
    public class WaveData
    {
        public List<Enemy> enemies;
        public float timeAfterWave;
        public int enemiesCount;
        public SpawnEnemiesWaveType enemiesWaveType;
        public bool isBossWave;
    }
    
    public enum SpawnEnemiesWaveType
    {
      None = 0, 
      Circle = 1,
      Square = 2,
      Chaotic = 3
    }
}
