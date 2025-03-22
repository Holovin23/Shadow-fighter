using System.Collections.Generic;
using UnityEngine;

namespace GameModules.Gameplay.Scripts.GameLoop
{
    [CreateAssetMenu(fileName = "ScriptableObjects/GameSettings", menuName = "Settings/GameSettings")]
    public class AllLevelsData : ScriptableObject
    {
        public List<LevelConfig> allConfigs = new List<LevelConfig>();
    }
}
