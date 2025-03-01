using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using Random = UnityEngine.Random;

namespace TFPlay.SceneFader
{
    public class FaderWaveSquareAnimation : BaseSceneFaderAnimation
    {
        [Header("Prefabs")]
        [SerializeField] private List<Image> horizontalPartPrefabs;
        [SerializeField] private HorizontalLayoutGroup verticalPartPrefab;

        [Header("Objects")]
        [SerializeField] private Transform partParrent;

        [Header("Settings")]
        [Tooltip("How many parts animate in 1 iteration coef\n\n[vertical parts count]*[this coef]=[partsPerIterationCoef]")]
        [SerializeField, Min(.1f)] private float partsPerIterationCoef;
        [Tooltip("[Screen.width]/[this coef]=[horizontal part count]\n\n[Screen.height]/[this coef]=[vertical part count]")]
        [SerializeField] private float partCountCoef;

        [Header("Other")]
        [SerializeField] private CanvasGroup loadingText;

        private List<Image> parts = new List<Image>();
        private int partsPerIteration;

        private void Awake()
        {
            Generate();
        }

        private void Generate()
        {
            parts.Clear();
            int horizontalPartCount = Mathf.RoundToInt(Screen.width / partCountCoef);
            int verticalPartCount = Mathf.RoundToInt(Screen.height / partCountCoef);
            partsPerIteration = Mathf.RoundToInt(verticalPartCount * partsPerIterationCoef);

            for (int i = 0; i < verticalPartCount; i++)
            {
                int horizontalPartsSpawnIndex = 0;
                if (i % 2 == 0) horizontalPartsSpawnIndex = 1;
                var tempVerticalPart = Instantiate(verticalPartPrefab, partParrent);

                for (int j = 0; j < horizontalPartCount; j++)
                {
                    var part = Instantiate(horizontalPartPrefabs[horizontalPartsSpawnIndex], tempVerticalPart.transform);
                    horizontalPartsSpawnIndex++;
                    if (horizontalPartsSpawnIndex >= horizontalPartPrefabs.Count) horizontalPartsSpawnIndex = 0;
                    parts.Add(part);
                }
            }
        }

        public override void Hide(Action callback)
        {
            StopAllCoroutines();
            StartCoroutine(FadeAnimation(0, Help));
            void Help()
            {
                callback?.Invoke();
            }
        }
        public override void HideImmediately()
        {
            StopAllCoroutines();
            for (int i = 0; i < parts.Count; i++)
                parts[i].DOFade(0, 0);
            loadingText.alpha = 0;
        }
        public override void Show(Action callback)
        {
            StopAllCoroutines();
            StartCoroutine(FadeAnimation(1, Help));
            void Help()
            {
                callback?.Invoke();
            }
        }
        public override void ShowImmediately()
        {
            gameObject.SetActive();
            StopAllCoroutines();
            for (int i = 0; i < parts.Count; i++)
                parts[i].DOFade(1, 0);
            loadingText.alpha = 1;
        }

        private IEnumerator FadeAnimation(float targetAlpha, Action callback)
        {
            Vector2 startPoint = new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height));
            parts = new List<Image>(parts.OrderBy(x => Vector2.Distance(x.transform.position, startPoint)));

            loadingText.DOFade(targetAlpha, .3f);
            for (int i = 0; i < parts.Count; i++)
            {
                parts[i].DOFade(targetAlpha, .1f);
                if (i % partsPerIteration == 0)
                    yield return null;
            }
            callback?.Invoke();
        }
    }
}
