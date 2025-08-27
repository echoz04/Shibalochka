using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Sources.UI.Services
{
    public class HUDView: UIViewBase
    {
        [SerializeField] private Button _inventoryButton;
        [SerializeField] private Button _settingsButton;
        
        public readonly string InventoryKey = "Inventory";
        public readonly string SettingsKey = "Settings";
        
        [Inject]
        private void Construct()
        {
            _actions = new Dictionary<string, Action>
            {
                {InventoryKey, null},
                {SettingsKey, null},
                
            };
        }
        
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