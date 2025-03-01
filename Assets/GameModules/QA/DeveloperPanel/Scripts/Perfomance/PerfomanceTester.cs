using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace TFPlay.DeveloperUtilities
{
    public class PerfomanceTester : BaseDeveloperTool
    {
        [SerializeField] private Transform holder;
        [SerializeField] private PerfomanceTesterButton perfomanceTesterButtonPrefab;
        [SerializeField] private PerfomanceTesterSlider perfomanceTesterSliderPrefab;

        private Vector2 initialScreenResolution;
        private Material skyboxMaterial;
        Dictionary<string, GameObject> testers = new Dictionary<string, GameObject>();

        protected override void InitInternal()
        {
            SetupScreenResolution();
            SetupDPIFactor();
            SetupSkybox();
        }

        protected override void TogglePanel()
        {
            base.TogglePanel();
            if (isOpened)
            {
                skyboxMaterial = RenderSettings.skybox;
            }
        }

        private void SetupScreenResolution()
        {
            initialScreenResolution = new Vector2(Screen.width, Screen.height);

            var resolutionTesterSlider = Instantiate(perfomanceTesterSliderPrefab, holder);
            resolutionTesterSlider.Init("Resolution: ", "{0:0.0}", 1f, v =>
            {
                var screenSize = new Vector2(initialScreenResolution.x, initialScreenResolution.y);
                var newScreenSize = new Vector2Int((int)(screenSize.x * v), (int)(screenSize.y * v));
                Screen.SetResolution(newScreenSize.x, newScreenSize.y, true);
            });
        }

        private void SetupSkybox()
        {
            var skyboxButton = Instantiate(perfomanceTesterButtonPrefab, holder);
            skyboxButton.Init("Skybox", () => RenderSettings.skybox != null, e => RenderSettings.skybox = e  ? skyboxMaterial : null);
        }

        private void SetupDPIFactor()
        {
            var dpiTesterSlider = Instantiate(perfomanceTesterSliderPrefab, holder);
            dpiTesterSlider.Init("DPI: ", "{0:0.0}", 1f, v =>
            {
                QualitySettings.resolutionScalingFixedDPIFactor = v;
            });
        }

        public void SetupVolume(Volume volume)
        {
            if (volume != null)
            {
                var postProcessingVolumeButton = Instantiate(perfomanceTesterButtonPrefab, holder);
                postProcessingVolumeButton.Init("Post Processing", () => volume.enabled, e => volume.enabled = e);

                var ppComponents = volume.profile.components;
                foreach (var component in ppComponents)
                {
                    if (!component.active)
                        continue;

                    var ppComponentButton = Instantiate(perfomanceTesterButtonPrefab, content);
                    var ppComponentName = component.name.Replace("(Clone)", "");
                    ppComponentButton.Init(ppComponentName, () => component.active, e => component.active = e);
                }
            }
        }

        public void SetupDirectionalLight(Light directionalLight)
        {
            if (directionalLight != null)
            {
                var shadowsButton = Instantiate(perfomanceTesterButtonPrefab, holder);
                shadowsButton.Init("Shadows", () => directionalLight.shadows != LightShadows.None, e => directionalLight.shadows = e ? LightShadows.Soft : LightShadows.None);
            }
        }
    }
}
