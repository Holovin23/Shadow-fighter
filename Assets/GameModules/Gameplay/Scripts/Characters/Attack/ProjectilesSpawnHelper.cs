using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameModules.Gameplay.Scripts.Characters
{
    public class ProjectilesSpawnHelper : MonoBehaviour
    {
        public List<ProjectileSpawnData> spawnData = new List<ProjectileSpawnData>();

        private Dictionary<ProjectileSpawnType, ProjectileSpawnData> dataByType = new Dictionary<ProjectileSpawnType, ProjectileSpawnData>();

        private void Awake() =>
            dataByType = spawnData.ToDictionary(x => x.spawnType);

        public List<Transform> GetSpawnPoints(ProjectileSpawnType spawnType, int projectilesCount)
        {
            ProjectilesSpawnPreset spawnPreset = dataByType[spawnType].presets.FirstOrDefault(preset => preset.projectilesCount == projectilesCount);
            return spawnPreset == null ? dataByType[spawnType].presets.First().spawnPoints : spawnPreset.spawnPoints;
        }
    }

    public enum ProjectileSpawnType
    {
        None = 0,
        Front = 1,
        Back = 2,
        Side = 3,
        Diagonal = 4,
    }

    [Serializable]
    public class ProjectileSpawnData
    {
        public ProjectileSpawnType spawnType;
        public List<ProjectilesSpawnPreset> presets = new List<ProjectilesSpawnPreset>();
    }

    [Serializable]
    public class ProjectilesSpawnPreset
    {
        public int projectilesCount;
        public List<Transform> spawnPoints;
    }
}