using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TFPlay.Modules.SaveLoadSystem.Data
{
    [System.Serializable]
    public class AdaptivePerformanceSaveData : RootSaveData
    {
        public bool AutoQualitySettings = true;
        public int QualitySettings = 2;
    }
}