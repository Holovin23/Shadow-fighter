using TFPlay.Modules.Levels;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TFPlay.UI
{
    public class RestartButton : BaseButton
    {
        [SerializeField] protected Button button;

        [Inject] private ILevelsService _levelsService;

        public override void Initialize()
        {
            button.onClick.AddListener(_levelsService.Restart);
        }
    }
}