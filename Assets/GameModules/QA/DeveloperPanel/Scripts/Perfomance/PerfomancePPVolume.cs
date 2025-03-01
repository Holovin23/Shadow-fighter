using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace TFPlay.DeveloperUtilities
{
    [RequireComponent(typeof(Volume))]
    public class PerfomancePPVolume : MonoBehaviour
    {
        [SerializeField] private Volume _volume;

        [Inject] private PerfomanceTester _perfomanceTester;

        private void OnValidate()
        {
            _volume = GetComponent<Volume>();
        }

        private void Start()
        {
            _perfomanceTester.SetupVolume(_volume);
        }
    }
}
