using TMPro;
using UnityEngine;

namespace TFPlay.DeveloperUtilities
{
    public class VersionDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _versionLabel;

        private void Start()
        {
            UpdateVersionLabel();
        }

        private void UpdateVersionLabel()
        {
            _versionLabel.text = $"v{Application.version}";
        }
    }
}