using UnityEngine;

namespace Sources
{
    [CreateAssetMenu(fileName = "LightPreset", menuName = "Scriptable Objects/LightPreset")]
    public class LightPreset : ScriptableObject
    {
        public Gradient ambientColor;
        public Gradient directionalColor;
        public Gradient fogColor;
    }
}
