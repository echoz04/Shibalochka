using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
namespace Sources.Runtime.Gameplay.MiniGames.Fishing
{
    [ExecuteAlways]
    public class FishingMiniGameGizmosDrawer : MonoBehaviour
    {
        [SerializeField] private FishingMiniGameBootstrapper _bootstrapper;

        [SerializeField] private UnityEngine.Camera _camera;
        [SerializeField] private Slider _pointerSlider;

        private void OnDrawGizmos()
        {
            if (_bootstrapper == null || _camera == null || _pointerSlider == null || _pointerSlider.gameObject.activeInHierarchy == false)
                return;

            var fishSlots = _bootstrapper.FishSlots;

            if (fishSlots == null)
                return;

            float pointerValue = _pointerSlider.value;
            Vector2 pointerScreen = MapSliderValueToScreenPosition(pointerValue);
            Vector3 pointerWorld = _camera.ScreenToWorldPoint(new Vector3(pointerScreen.x, pointerScreen.y, 10f));
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(pointerWorld, 0.05f);

            foreach (var slot in fishSlots)
            {
                if (slot.CurrentFish != null)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(slot.CurrentFish.transform.position, slot.CatchRange / 100f);
                }

                float catchMin = slot.CatchCenterValue - slot.CatchRange;
                float catchMax = slot.CatchCenterValue + slot.CatchRange;

                Vector2 minScreen = MapSliderValueToScreenPosition(catchMin);
                Vector2 maxScreen = MapSliderValueToScreenPosition(catchMax);

                Vector3 minWorld = _camera.ScreenToWorldPoint(new Vector3(minScreen.x, minScreen.y, 10f));
                Vector3 maxWorld = _camera.ScreenToWorldPoint(new Vector3(maxScreen.x, maxScreen.y, 10f));

                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(minWorld, maxWorld);
            }
        }

        private Vector2 MapSliderValueToScreenPosition(float sliderValue)
        {
            RectTransform sliderRect = _pointerSlider.GetComponent<RectTransform>();

            Vector3[] worldCorners = new Vector3[4];
            sliderRect.GetWorldCorners(worldCorners);

            float minX = RectTransformUtility.WorldToScreenPoint(_camera, worldCorners[0]).x;
            float maxX = RectTransformUtility.WorldToScreenPoint(_camera, worldCorners[3]).x;

            float t = Mathf.InverseLerp(_pointerSlider.minValue, _pointerSlider.maxValue, sliderValue);
            float screenX = Mathf.Lerp(minX, maxX, t);

            float screenY = RectTransformUtility.WorldToScreenPoint(_camera, sliderRect.position).y;

            return new Vector2(screenX, screenY);
        }
    }
}
#endif
