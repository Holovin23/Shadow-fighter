using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameModules.Gameplay.Scripts.GameLoop
{
    public class SpawnPoints : MonoBehaviour
    {
        [SerializeField] public List<Transform> spawnPointsTransforms;
        
        public List<Vector3> spawnPoints = new List<Vector3>();

        [Button]
        private void RecalculateSpawnPoints()
        {
            spawnPoints.Clear();
            foreach (var transform in spawnPointsTransforms)
            {
                spawnPoints.Add(transform.position);
            }
        }
    }
}