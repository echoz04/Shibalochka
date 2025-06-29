using UnityEngine;

namespace Sources.Runtime.Gameplay.Configs
{
    [System.Serializable]
    public class CameraConfig
    {
        [Header("Camera")]
        [field: SerializeField] public float Sensitivity { get; private set; } = 30f;
        [field: SerializeField] public float MinVerticalAngle { get; private set; } = -25f;
        [field: SerializeField] public float MaxVerticalAngle { get; private set; } = 25f;
    }
}
