using UnityEngine;
using Sirenix.OdinInspector;

namespace Sources.Runtime.Gameplay.Configs
{
    [System.Serializable]
    public class DiscordConfig
    {
        [Title("Discord Overlay Settings")]

        [SerializeField, LabelText("Overlay Title")] private string _overlayTitle = "Chilling";
        public string OverlayTitle => _overlayTitle;
    }
}
