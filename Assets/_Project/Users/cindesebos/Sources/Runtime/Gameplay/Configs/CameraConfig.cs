using UnityEngine;
using Sirenix.OdinInspector;

namespace Sources.Runtime.Gameplay.Configs
{
    [System.Serializable]
    public class CameraConfig
    {
        [Title("Camera")]

        [SerializeField, MinValue(1f), MaxValue(100f), LabelText("Sensitivity")] private float _sensitivity = 2f;
        public float Sensitivity => _sensitivity;

        [SerializeField, LabelText("Min Vertical Angle")] private float _minVerticalAngle = -25f;
        public float MinVerticalAngle => _minVerticalAngle;

        [SerializeField, LabelText("Max Vertical Angle")] private float _maxVerticalAngle = 25f;
        public float MaxVerticalAngle => _maxVerticalAngle;
    }
}
