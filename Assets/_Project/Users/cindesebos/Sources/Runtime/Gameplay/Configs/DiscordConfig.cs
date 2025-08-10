using UnityEngine;
using Sirenix.OdinInspector;

namespace Sources.Runtime.Gameplay.Configs
{
    [System.Serializable]
    public class DiscordConfig
    {
        [Title("Discord Overlay Settings")]

        [SerializeField, LabelText("Enable Overlay")] private bool _isOverlayEnabled;
        public bool IsOverlayEnabled => _isOverlayEnabled;

        [SerializeField, LabelText("Overlay Title"), ShowIf(nameof(_isOverlayEnabled))] private string _overlayTitle;
        public string OverlayTitle => _overlayTitle;
    }
}
