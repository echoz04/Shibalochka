using UnityEngine;
using UnityEngine.UI;

namespace Sources.Runtime.Utilities
{
    public static class Extensions
    {
        public static float MapWorldPositionToSliderValue(Vector3 worldPosition, Slider slider)
        {
            RectTransform sliderRect = slider.GetComponent<RectTransform>();

            Vector2 localPoint = sliderRect.InverseTransformPoint(worldPosition);

            float normalized = Mathf.InverseLerp(sliderRect.rect.xMin, sliderRect.rect.xMax, localPoint.x);

            return Mathf.Lerp(slider.minValue, slider.maxValue, normalized);
        }
    }
}