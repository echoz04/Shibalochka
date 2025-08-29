using FMODUnity;
using UnityEngine;

namespace Sources
{
    [ExecuteAlways]
    public class LightManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Light directionalLight;
        [SerializeField] private LightPreset preset;

        [Header("Time Settings")]
        [SerializeField, Range(0, 24)] private float timeOfDay = 12f;
        [SerializeField, Min(1f)] private float dayLengthInSeconds = 120f;

        private void Update()
        {
            if (preset == null)
                return;

            if (Application.isPlaying)
            {
                float timePerSecond = 24f / dayLengthInSeconds;
                timeOfDay += Time.deltaTime * timePerSecond;
                timeOfDay %= 24;
                UpdateLighting(timeOfDay / 24f);
            }
            else
            {
                UpdateLighting(timeOfDay / 24f);
            }
        }

        private void UpdateLighting(float timePercent)
        {
            RenderSettings.ambientLight = preset.ambientColor.Evaluate(timePercent);
            RenderSettings.fogColor = preset.fogColor.Evaluate(timePercent);

            if (directionalLight != null)
            {
                directionalLight.color = preset.directionalColor.Evaluate(timePercent);
                directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170, 0));
            }

            if (Application.isPlaying)
            {
                float value = timePercent * 24f;
                RuntimeManager.StudioSystem.setParameterByName("Time", value);
            }
        }

        private void OnValidate()
        {
            if (directionalLight != null)
                return;

            if (RenderSettings.sun != null)
            {
                directionalLight = RenderSettings.sun;
            }
            else
            {
                Light[] lights = GameObject.FindObjectsOfType<Light>();
                foreach (Light light in lights)
                {
                    if (light.type == LightType.Directional)
                    {
                        directionalLight = light;
                        return;
                    }
                }
            }
        }
    }
}
