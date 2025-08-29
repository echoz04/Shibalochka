using System.Collections;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

namespace Sources
{
    public class WeatherManager : MonoBehaviour
    {
        [Header("Rain Settings")]
        [SerializeField] private GameObject rainParticles;
        [SerializeField] private EventReference rainEvent;
        [SerializeField] private float minDelayBetweenRains = 1f;
        [SerializeField] private float maxDelayBetweenRains = 3f;
        [SerializeField] private float minRainDuration = 0.5f;
        [SerializeField] private float maxRainDuration = 2f;
        [SerializeField] private float fadeOutTime = 3f;

        private EventInstance rainInstance;
        private Coroutine rainRoutine;

        private void Start()
        {
            rainRoutine = StartCoroutine(RainCycle());
        }

        private IEnumerator RainCycle()
        {
            while (true)
            {
                float waitTime = Random.Range(minDelayBetweenRains, maxDelayBetweenRains);
                yield return new WaitForSeconds(waitTime);
                StartRain();

                float rainTime = Random.Range(minRainDuration, maxRainDuration) * 60f;
                yield return new WaitForSeconds(rainTime);
                yield return StopRain();
            }
        }

        private void StartRain()
        {
            if (rainParticles != null)
                rainParticles.SetActive(true);

            rainInstance = RuntimeManager.CreateInstance(rainEvent);
            rainInstance.start();

            Debug.Log("Rain started");
        }

        private IEnumerator StopRain()
        {
            if (rainParticles != null)
                rainParticles.SetActive(false);

            float timer = 0f;
            while (timer < fadeOutTime)
            {
                timer += Time.deltaTime;
                float volume = Mathf.Lerp(1f, 0f, timer / fadeOutTime);
                rainInstance.setParameterByName("RainVolume", volume);
                yield return null;
            }

            rainInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            rainInstance.release();

            Debug.Log("Rain stopped");

            yield break;
        }

    }
}
