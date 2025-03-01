using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace TFPlay.DeveloperUtilities
{
    [RequireComponent(typeof(Light))]
    public class PerfomanceDirectionalLight : MonoBehaviour
    {
        [SerializeField] private Light _directionalLight;

        [Inject] private PerfomanceTester _perfomanceTester;

        private void OnValidate()
        {
            _directionalLight = GetComponent<Light>();
        }

        private void Start()
        {
            _perfomanceTester.SetupDirectionalLight(_directionalLight);
        }
    }
}
