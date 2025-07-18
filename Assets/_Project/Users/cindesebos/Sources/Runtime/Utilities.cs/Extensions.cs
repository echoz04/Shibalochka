using UnityEngine;
using UnityEngine.UI;

namespace Sources.Runtime.Utilities
{
    public static class Extensions
    {
        public static float MapScreenPositionToSliderValue(float screenX, Slider slider, Camera camera)
        {
            RectTransform sliderRect = slider.GetComponent<RectTransform>();

            Vector3[] worldCorners = new Vector3[4];
            sliderRect.GetWorldCorners(worldCorners);

            float minX = RectTransformUtility.WorldToScreenPoint(camera, worldCorners[0]).x;
            float maxX = RectTransformUtility.WorldToScreenPoint(camera, worldCorners[3]).x;

            float t = Mathf.InverseLerp(minX, maxX, screenX);
            float value = Mathf.Lerp(slider.minValue, slider.maxValue, t);

            return value;
        }
    }
}