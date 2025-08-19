using System;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.UI.Services
{
    public class HUDView: UIViewBase
    {
        [SerializeField] private Button _inventoryButton;
        [SerializeField] private Button _settingsButton;
        
        public readonly string InventoryKey = "Inventory";
        public readonly string SettingsKey = "Settings";
        
        public override void Show(Action onComplete = null)
        {
            Listen();
            onComplete?.Invoke();
        }

        public override void Hide(Action onComplete = null)
        {
            Unlisten();
            onComplete?.Invoke();
        }
        
        void Listen()
        {
            _inventoryButton.onClick.AddListener(() => _actions[InventoryKey].Invoke());
            _settingsButton.onClick.AddListener(() => _actions[SettingsKey].Invoke());
        }

        void Unlisten()
        {
            _inventoryButton.onClick.RemoveAllListeners();
            _settingsButton.onClick.RemoveAllListeners();
        }
    }
}