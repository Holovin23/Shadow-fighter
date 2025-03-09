using GameModules.Gameplay.Scripts.GameLoop;
using Pooling;
using TFPlay.Modules.Core.TickService;
using UnityEngine;
using Zenject;

public class GameLoopController : MonoBehaviour
{
    [Inject] private SignalBus _signalBus;
    [Inject] private IPoolService _poolService;
    [SerializeField] private LevelConfig _levelConfig;
    [SerializeField] private Vector2 _mapSize = new Vector2(10,10);
    [SerializeField] private SpawnPoints _spawnPoints;

    private EnemyFactory _enemyFactory;
    
    private int waveIndex = 0;
    private float timeToNextWave = 0f;
    
    private void Start()
    {
        Init();
    }
    
    private void Init()
    {
        timeToNextWave = _levelConfig.timeToFirstWave;
        _enemyFactory = new EnemyFactory(_poolService,_spawnPoints);
        _signalBus.Subscribe<TickSignal>(UpdateByTick);
    }

    
    
    private void UpdateByTick()
    {
        timeToNextWave -= Time.deltaTime;

        if (timeToNextWave <= 0 && waveIndex < _levelConfig.waves.Count)
        {
            var currentWave = _levelConfig.waves[waveIndex];
            foreach (var enemy in currentWave.enemies)
            {
                _enemyFactory.SpawnEnemy(enemy,currentWave.enemiesCount,currentWave.enemiesWaveType); 
            }

            timeToNextWave = currentWave.timeAfterWave;
            waveIndex++;
            if (currentWave.isBossWave)
            {
                //TODO stopTimer and wait on boss death
            }
        }
        
    }
}