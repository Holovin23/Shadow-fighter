using TFPlay.Modules.Levels;
using TMPro;
using UnityEngine;
using Zenject;

namespace NewUI
{
    public class LevelPanel : MonoBehaviour
    {
        private const string LEVEL_FORMAT = "LEVEL {0}";
        [Inject] private ILevelsService _levelsService;

        [SerializeField] private TextMeshProUGUI _text;

        private void OnEnable()
        {
            UpdateLevelText();
        }

        private void UpdateLevelText()
        {
            var level = _levelsService.CurrentLevel;
            _text.text = string.Format(LEVEL_FORMAT, level);
        }
    }
}