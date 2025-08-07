using System;
using System.Linq;
using Sirenix.OdinInspector;
using Sources.Runtime.Gameplay.Configs;
using Sources.Runtime.Gameplay.Inventory;
using Sources.Runtime.Gameplay.Wallet;
using Sources.Runtime.Services.SceneLoader;
using UnityEngine;

namespace Sources.Runtime.Project
{
    public class ContentManagementSystem : MonoBehaviour
    {
        [ShowInInspector, ReadOnly] public static string PrefabPath { get; private set; } = "CMS Prefab";

        private static ContentManagementSystem _owner;

        public static ContentManagementSystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<ContentManagementSystem>();

                    if (_instance == null)
                    {
                        _owner = Resources.Load<ContentManagementSystem>(PrefabPath);

                        var instance = new GameObject("ContentManagementSystem");

                        DontDestroyOnLoad(instance);

                        _instance = instance.AddComponent<ContentManagementSystem>();

                        _instance.InitializeInstance();
                    }
                }
                return _instance;
            }
        }

        private static ContentManagementSystem _instance;

        // [field: SerializeField] public bool InstanceHasProjectConfigLoader => Instance != null && Instance.ProjectConfigLoader != null;
        [field: SerializeField] public bool InstanceHasProjectConfig => Instance != null && Instance.ProjectConfig != null;

        // public IProjectConfigLoader ProjectConfigLoader { get; set; }
        
        public ProjectConfig ProjectConfig { get; set; }

        public void InitializeInstance()
        {
            _instance.SceneToLoad = _owner.SceneToLoad;
        }

        [field: SerializeField, ValueDropdown(nameof(GetFilteredScenes))] public Scene SceneToLoad { get; private set; } = Scene.Gameplay;

        private Scene[] GetFilteredScenes()
        {
            return Enum.GetValues(typeof(Scene))
                .Cast<Scene>()
                .Where(s => s != Scene.Bootstrap)
                .ToArray();
        }


        #region Add Item

        public InventoryRoot InventoryRoot { get; set; }

        [field: SerializeField] public bool InstanceHasInventoryRoot => Instance != null && Instance.InventoryRoot != null;

        [Space]
        [SerializeField, FoldoutGroup("Add Item"), LabelText("Item ID")] private string _targetItemId;

        [FoldoutGroup("Add Item"), Button("Add", ButtonSizes.Medium)]
        private void AddItemToInventory()
        {
            if (Application.isPlaying == false)
                return;

            var configs = Instance.ProjectConfig?.ItemsConfig?.Configs;

            var item = configs
                .Where(i => i != null)
                .FirstOrDefault(i => i.TypeId == _targetItemId);

            if (item == null)
            {
                Debug.Log($"Item with ID '{_targetItemId}' not found.");
                return;
            }

            if (Instance.InventoryRoot.TryAddItem(item))
            {
                Debug.Log($"Added item '{_targetItemId}' to inventory.");
                _targetItemId = string.Empty;
            }
        }

        private bool CanShowInventoryControls =>
            !Application.isPlaying || InstanceHasInventoryRoot;

        #endregion

        #region Wallet

        public WalletRoot WalletRoot { get; set; }

        [field: SerializeField] public bool InstanceHasWallet => Instance != null && Instance.WalletRoot != null;

        [Space]
        [SerializeField, FoldoutGroup("Add Money"), LabelText("Money Amount")] private int _targetMoneyAmount;

        [FoldoutGroup("Add Money"), Button("Add", ButtonSizes.Medium)]
        private void AddMoneyToWallet()
        {
            if (Application.isPlaying == false)
                return;

            Instance.WalletRoot.Money.TryAdd(_targetMoneyAmount);
            _targetMoneyAmount = 0;
        }

        #endregion
    }
}
