using System;
using UnityEngine;
using TMPro;

namespace TFPlay.DeveloperUtilities.FpsCounter
{
    [RequireComponent(typeof(FpsCounter))]
    public class FpsDisplay : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _averageLabel;
        [SerializeField]
        private TextMeshProUGUI _highestLabel;
        [SerializeField]
        private TextMeshProUGUI _lowestLabel;

        [SerializeField]
        private FPSColor[] _fpsColors;

        private FpsCounter _fpsCounter;

        private string[] _fpsStringCache;

        private void Awake()
        {
            _fpsCounter = GetComponent<FpsCounter>();
        }

        private void Start()
        {
            _fpsStringCache = new string[1000];
            for (int i = 0; i < _fpsStringCache.Length; i++)
            {
                _fpsStringCache[i] = i.ToString("D2");
            }
        }

        private void Update()
        {
            Display(_averageLabel, _fpsCounter.AverageFPS);
            Display(_highestLabel, _fpsCounter.HighestPFS);
            Display(_lowestLabel, _fpsCounter.LowersFPS);
        }

        private void Display(TextMeshProUGUI label, int fps)
        {
            label.text = _fpsStringCache[Mathf.Clamp(fps, 0, _fpsStringCache.Length)];
            for (int i = 0; i < _fpsColors.Length; i++)
            {
                if (fps >= _fpsColors[i].MinFPS)
                {
                    label.color = _fpsColors[i].Color;
                    break;
                }
            }
        }

        [Serializable]
        public struct FPSColor
        {
            public Color Color;
            public int MinFPS;
        }
    }
}
