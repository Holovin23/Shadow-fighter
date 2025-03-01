using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TFPlay.Modules.GameResources;
using TFPlay.Modules.Levels;
using TFPlay.Infrastructure.StateMachine.Application;
using Zenject;

namespace TFPlay.DeveloperUtilities
{
    public class QAConsole : BaseDeveloperTool
    {
        [Header("Level")] [SerializeField] private TMP_InputField levelInputField;
        [SerializeField] private Button loadLevelButton;
        [SerializeField] private TextMeshProUGUI levelNameText;
        [SerializeField] private TextMeshProUGUI levelNumberText;
        [SerializeField] private TextMeshProUGUI buildVersionText;

        [Header("Coins")] [SerializeField] private Button addCoinsButton;
        [SerializeField] private Button removeCoinsButton;
        [SerializeField] private int coinAmount;

        [Header("Core")] [SerializeField] private Button winLevelButton;
        [SerializeField] private Button loseLevelButton;

        [Header("UI")] [SerializeField] private Button toggleMenuUIButton;
        [SerializeField] private TextMeshProUGUI toggleMenuUIText;

        [Inject] private ApplicationStateMachine _applicationStateMachine;
        [Inject] private ILevelsService _progressService;

        private bool showUI = true;
        private static List<IQAHideableContent> hideableContents = new List<IQAHideableContent>();
        private SceneCheatsHelper _sceneCheatsHelper;

        public static void RegisterContent(IQAHideableContent hideableContent)
        {
            hideableContents.Add(hideableContent);
        }

        public static void UnregisterContent(IQAHideableContent hideableContent)
        {
            hideableContents.Remove(hideableContent);
        }

        public void SetSceneCheatHelper(SceneCheatsHelper sceneCheatsHelper)
        {
            _sceneCheatsHelper = sceneCheatsHelper;
        }

        protected override void InitInternal()
        {
            loadLevelButton.onClick.AddListener(LoadLevel);
            addCoinsButton.onClick.AddListener(AddCoins);
            removeCoinsButton.onClick.AddListener(ClearCoins);
            winLevelButton.onClick.AddListener(WinLevel);
            loseLevelButton.onClick.AddListener(LoseLevel);
            toggleMenuUIButton.onClick.AddListener(ToggleMenuUI);
        }

        protected override void TogglePanel()
        {
            base.TogglePanel();
            if (isOpened)
            {
                ShowInformation();
            }
        }

        private void LoadLevel()
        {
            if (int.TryParse(levelInputField.text, out int levelNumber))
            {
                isOpened = false;
                content.SetInactive();

                _progressService.ToLevel(levelNumber);
            }
        }

        private void ShowInformation()
        {
            if (SceneManager.sceneCount > 1)
            {
                levelNameText.text = string.Format("SCENE NAME: {0}", SceneManager.GetSceneAt(1).name);
                levelNumberText.text =
                    string.Format("SCENE NUMBER IN BUILD: {0}", SceneManager.GetSceneAt(1).buildIndex);
            }

            buildVersionText.text = string.Format("BUILD VERSION: {0}", Application.version);
        }

        private void AddCoins()
        {
            Debug.LogError($"TEMPORARY NO COINS");
        }

        private void ClearCoins()
        {
            Debug.LogError($"TEMPORARY CANNOT CLEAR");
        }

        private void WinLevel()
        {
            _sceneCheatsHelper.WinUI();
        }

        private void LoseLevel()
        {
            _sceneCheatsHelper.LoseUI();
        }

        private void ToggleMenuUI()
        {
            showUI = !showUI;
            toggleMenuUIText.text = showUI ? "Hide UI" : "Show UI";
            foreach (var hideableContent in hideableContents)
            {
                hideableContent.ToggleContent();
            }
        }
    }
}