using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TFPlay.Modules.SaveLoadSystem;
using TFPlay.Modules.SaveLoadSystem.Data;
using UnityEngine;
using Zenject;

namespace TFPlay.Modules.AdaptivePerformance
{
    public class AdaptivePerformanceService : MonoBehaviour, IAdaptivePerformanceService
    {
        [Serializable]
        public class PresetSettings
        {
            public bool isAutoChange = true;
            public int qualityLevel;
            public float screenResolutionPercent;
            public Action startPresetAction;
        }

        [FoldoutGroup("Editor"), SerializeField] private bool enableInEditor = false;
        [FoldoutGroup("Editor"), SerializeField, ReadOnly] private float avgFPS;
        [FoldoutGroup("Editor"), SerializeField, ReadOnly] private int qualityLevelDebug;

        [SerializeField] private int targetFPS;
        [SerializeField] private int setDownSettingsOffset = 15;

        [FoldoutGroup("Presets"), SerializeField] private PresetSettings highQualityPreset;
        [FoldoutGroup("Presets"), SerializeField] private PresetSettings mediumQualityPreset;
        [FoldoutGroup("Presets"), SerializeField] private PresetSettings lowQualityPreset;

        [SerializeField] private List<int> checkFPSPause = new List<int>();

        private List<PresetSettings> settings = new List<PresetSettings>();

        private bool isRecorded = true;
        private int curCheckFPSIndex = 0;
        private float recordFPS;
        private int recordFrame;
        private bool autoQualityEnabled;
        private Vector2Int screenResolution;
        private PresetSettings curGraphicPreset;

        private Coroutine autoSettingsRoutine;
        private Coroutine recordFPSRoutine;

        public static event Action<int> OnQualityChangeEvent;

        private AdaptivePerformanceSaveData SaveData => _saveLoadSystem.GetData<AdaptivePerformanceSaveData>(SaveDataIds.ADAPTIVE_PERFORMANCE);
        
        private ISaveLoadSystem _saveLoadSystem;
        private SceneLoader _sceneLoader;

        [Inject]
        private void Construct(ISaveLoadSystem saveLoadSystem, SceneLoader sceneLoader)
        {
            _saveLoadSystem = saveLoadSystem;
            _sceneLoader = sceneLoader;
        }

        public void Initialize()
        {
            screenResolution = new Vector2Int(Screen.width, Screen.height);

            _sceneLoader.OnLevelStartLoading += SceneLoader_OnLocationLoadStart;
            _sceneLoader.OnLevelLoaded += SceneLoader_OnLocationLoadEnd;

            //AdController.OnInterstitialStart += AdController_OnInterstitialStart;
            //AdController.OnInterstitialEnd += AdController_OnInterstitialEnd;
            //AdController.OnRewardedStart += AdController_OnRewardedStart;
            //AdController.OnRewardedEnd += AdController_OnRewardedEnd;

            lowQualityPreset.startPresetAction = SetLowSettings;
            mediumQualityPreset.startPresetAction = SetMediumSettings;
            highQualityPreset.startPresetAction = SetHighSettings;

            settings.Add(highQualityPreset);
            settings.Add(mediumQualityPreset);
            settings.Add(lowQualityPreset);

            settings.OrderBy(x => Math.Abs(SaveData.QualitySettings - x.qualityLevel)).FirstOrDefault()?.startPresetAction?.Invoke();
            qualityLevelDebug = curGraphicPreset.qualityLevel;
            autoQualityEnabled = SaveData.AutoQualitySettings;

            if (autoQualityEnabled)
                EnableAutoSettings();
        }

        private void OnDestroy()
        {
            _sceneLoader.OnLevelStartLoading -= SceneLoader_OnLocationLoadStart;
            _sceneLoader.OnLevelLoaded -= SceneLoader_OnLocationLoadEnd;

            //AdController.OnInterstitialStart -= AdController_OnInterstitialStart;
            //AdController.OnInterstitialEnd -= AdController_OnInterstitialEnd;
            //AdController.OnRewardedStart -= AdController_OnRewardedStart;
            //AdController.OnRewardedEnd -= AdController_OnRewardedEnd;
        }

        public void EnableAutoSettings()
        {
            DisableAutoSettings();
#if UNITY_EDITOR
            if (!enableInEditor)
                return;
#endif
            autoSettingsRoutine = StartCoroutine(CheckSettingsRoutine());
        }

        public void DisableAutoSettings()
        {
            recordFPS = 0;
            recordFrame = 0;
            if (autoSettingsRoutine != null)
                StopCoroutine(autoSettingsRoutine);
            if (recordFPSRoutine != null)
                StopCoroutine(recordFPSRoutine);
        }

        private IEnumerator RecordFPSRoutine()
        {
            while (true)
            {
                if (!isRecorded)
                {
                    yield return null;
                    continue;
                }
                recordFPS += 1 / Time.unscaledDeltaTime;
                recordFrame++;
                yield return new WaitForSeconds(Time.unscaledDeltaTime);
            }
        }

        private IEnumerator CheckSettingsRoutine()
        {
            while (true)
            {
                recordFPSRoutine = StartCoroutine(RecordFPSRoutine());
                if (curCheckFPSIndex >= checkFPSPause.Count()) curCheckFPSIndex = checkFPSPause.Count() - 1;
                yield return new WaitForSeconds(checkFPSPause[curCheckFPSIndex]);
                if (recordFPSRoutine != null)
                {
                    StopCoroutine(recordFPSRoutine);
                    recordFPSRoutine = null;
                    avgFPS = recordFPS / recordFrame;
                    recordFPS = 0;
                    recordFrame = 0;
                    curCheckFPSIndex++;
                    ChoseSettings();
                }
            }
        }

        private void ChoseSettings()
        {
            if (Mathf.RoundToInt(avgFPS) < Mathf.RoundToInt(targetFPS) && Math.Abs(targetFPS - avgFPS) > setDownSettingsOffset)
            {
                var preset = settings.FirstOrDefault(x => x.qualityLevel < curGraphicPreset.qualityLevel && x.isAutoChange);
                if (preset != default) preset.startPresetAction?.Invoke();
                if (preset != default) OnQualityChangeEvent?.Invoke(preset.qualityLevel);
            }

            qualityLevelDebug = curGraphicPreset.qualityLevel;
            SaveData.QualitySettings = curGraphicPreset.qualityLevel;
            _saveLoadSystem.Save<AdaptivePerformanceSaveData>(SaveDataIds.ADAPTIVE_PERFORMANCE);
        }

        [Button]
        private void SetHighSettings()
        {
            curGraphicPreset = highQualityPreset;
            qualityLevelDebug = curGraphicPreset.qualityLevel;
            QualitySettings.SetQualityLevel(curGraphicPreset.qualityLevel, true);
            SetScreenResolution(curGraphicPreset.screenResolutionPercent);
        }

        [Button]
        private void SetMediumSettings()
        {
            curGraphicPreset = mediumQualityPreset;
            qualityLevelDebug = curGraphicPreset.qualityLevel;
            QualitySettings.SetQualityLevel(curGraphicPreset.qualityLevel, false);
            SetScreenResolution(curGraphicPreset.screenResolutionPercent);
        }

        [Button]
        private void SetLowSettings()
        {
            curGraphicPreset = lowQualityPreset;
            qualityLevelDebug = curGraphicPreset.qualityLevel;
            QualitySettings.SetQualityLevel(curGraphicPreset.qualityLevel, false);
            SetScreenResolution(curGraphicPreset.screenResolutionPercent);
        }

        public void SetScreenResolution(float screenResolutionPercent)
        {
            var newWidth = (int)(screenResolution.x * screenResolutionPercent);
            var newHeight = (int)(screenResolution.y * screenResolutionPercent);
            var newScreenSize = new Vector2Int(newWidth, newHeight);
            Screen.SetResolution(newScreenSize.x, newScreenSize.y, true);
        }


#region Events_Callbacks

        private void AdController_OnInterstitialStart() => isRecorded = false;

        private void AdController_OnRewardedStart() => isRecorded = false;

        private void AdController_OnInterstitialEnd() => isRecorded = true;

        private void AdController_OnRewardedEnd() => isRecorded = true;

        private void SceneLoader_OnLocationLoadStart() => isRecorded = false;

        private void SceneLoader_OnLocationLoadEnd() => isRecorded = true;

        private void OnAutoQualitySettingsChanged(bool autoQualitySettingsEnabled)
        {
            if (autoQualitySettingsEnabled && !autoQualityEnabled)
                EnableAutoSettings();
            else if (!autoQualitySettingsEnabled && autoQualityEnabled)
                DisableAutoSettings();

            autoQualityEnabled = SaveData.AutoQualitySettings;
        }

        private void OnQualityLevelSettingsChanged(int qualityLevel)
        {
            var newQualitySettings = settings.OrderBy(x => Math.Abs(SaveData.QualitySettings - x.qualityLevel)).FirstOrDefault();
            if (newQualitySettings == null || newQualitySettings.qualityLevel == curGraphicPreset.qualityLevel)
                return;
            newQualitySettings.startPresetAction?.Invoke();
            qualityLevelDebug = curGraphicPreset.qualityLevel;
        }
#endregion
    }
}