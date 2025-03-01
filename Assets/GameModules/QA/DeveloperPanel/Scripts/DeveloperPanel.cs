using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TFPlay.DeveloperUtilities
{
    public class DeveloperPanel : MonoBehaviour
    {
        [SerializeField] private AccessPanel accessPanel;
        [SerializeField] private List<BaseDeveloperTool> developerTools;

        private void Start()
        {
            accessPanel.OnAccessGranted += ShowTools;
            accessPanel.OnExit += HideTools;
        }

        private void OnDestroy()
        {
            accessPanel.OnAccessGranted -= ShowTools;
            accessPanel.OnExit -= HideTools;
        }

        public void TogglePanel()
        {
            accessPanel.Show();
        }

        public void ShowTools()
        {
            foreach (var tool in developerTools)
            {
                tool.Show();
            }
        }

        public void HideTools()
        {
            foreach (var tool in developerTools)
            {
                tool.Hide();
            }
        }
    }
}
