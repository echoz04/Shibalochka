#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sources.Runtime.Gameplay.Configs;
using Sources.Runtime.Gameplay.Configs.Items;
using UnityEditor;
using UnityEngine;

namespace Sources.Editor
{
    public class ProjectConfigEditorWindow : OdinEditorWindow
    {
        [SerializeField, HideInInspector]
        private ProjectConfig _projectConfig;

        [Title("Global Configs"), PropertyOrder(0)]
        [ShowIf(nameof(_projectConfig)), InlineProperty]
        [TabGroup("Configs", "Camera")] public CameraConfig _cameraConfig;

        [ShowIf(nameof(_projectConfig)), InlineProperty]
        [TabGroup("Configs", "UI")] public UIConfig _uiConfig;

        [ShowIf(nameof(_projectConfig)), InlineProperty]
        [TabGroup("Configs", "Inventory")] public InventoryConfig _inventoryConfig;

        [ShowIf(nameof(_projectConfig)), InlineProperty]
        [TabGroup("Configs", "Items")] public ItemsConfig _itemsConfig;

        [ShowIf(nameof(_projectConfig)), InlineProperty]
        [TabGroup("Configs", "Discord")] public DiscordConfig _discordConfig;

        [Title("Edit ItemConfig"), PropertyOrder(10), Space(10)]
        [ShowIf(nameof(_projectConfig))]
        [TabGroup("Configs", "Items")]
        [ValueDropdown(nameof(GetAllItemConfigs)), LabelText("Select Item")]
        public ItemConfig _selectedItemConfig;

        [ShowIf(nameof(_selectedItemConfig)), PropertyOrder(11)]
        [TabGroup("Configs", "Items")]
        [InlineEditor(InlineEditorModes.GUIOnly)]
        public ItemConfig _inlineItemEditor;

        [MenuItem(ProjectConfigEditorWindowConstants.MENU_ITEM_PATH)]
        private static void OpenWindow()
        {
            var window = GetWindow<ProjectConfigEditorWindow>();
            window.titleContent = new GUIContent(ProjectConfigEditorWindowConstants.WINDOW_TITLE);
            window.Show();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (_projectConfig == null)
            {
                string[] guids = AssetDatabase.FindAssets(ProjectConfigEditorWindowConstants.ASSET_PATH);

                if (guids.Length > 0)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    _projectConfig = AssetDatabase.LoadAssetAtPath<ProjectConfig>(path);
                }
                else
                {
                    Debug.LogWarning(ProjectConfigEditorWindowConstants.NO_PROJECT_CONFIG_FOUND_WARNING);
                    return;
                }
            }

            _cameraConfig = _projectConfig.CameraConfig;
            _uiConfig = _projectConfig.UIConfig;
            _inventoryConfig = _projectConfig.InventoryConfig;
            _discordConfig = _projectConfig.DiscordConfig;
            _itemsConfig = _projectConfig.ItemsConfig;

            if (_itemsConfig.Configs is List<ItemConfig> configs && configs.Count > 0)
            {
                _selectedItemConfig = configs[0];
                _inlineItemEditor = _selectedItemConfig;
            }
        }

        private IEnumerable<ItemConfig> GetAllItemConfigs()
        {
            if (_itemsConfig?.Configs is List<ItemConfig> configs)
            {
                foreach (var config in configs)
                    yield return config;
            }
        }

        private void OnValidate()
        {
            _inlineItemEditor = _selectedItemConfig;
        }

        [Button(ProjectConfigEditorWindowConstants.ADD_ITEM_BUTTON_TITLE), PropertyOrder(12)]
        [TabGroup("Configs", "Items")]
        private void AddNewItem()
        {
            var newItem = ScriptableObject.CreateInstance<ItemConfig>();
            newItem.name = $"TempItem_{_itemsConfig.Configs.Count}";

            string assetPath = ProjectConfigEditorWindowConstants.NEW_ITEM_PATH;
            if (!AssetDatabase.IsValidFolder(assetPath.TrimEnd('/')))
            {
                Debug.LogError($"Folder does not exist: {assetPath}");
                return;
            }

            string tempPath = AssetDatabase.GenerateUniqueAssetPath($"{assetPath}{newItem.name}.asset");
            AssetDatabase.CreateAsset(newItem, tempPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Selection.activeObject = newItem;
            EditorUtility.FocusProjectWindow();
            EditorGUIUtility.PingObject(newItem);

            _itemsConfig.Configs.Add(newItem);
            EditorUtility.SetDirty(_projectConfig);
        }

        [Button(ProjectConfigEditorWindowConstants.CHECK_FOR_DUPLICATE_BUTTON_TITLE), PropertyOrder(13)]
        [TabGroup("Configs", "Items")]
        private void CheckForDuplicateTypeIds()
        {
            var typeIdSet = new HashSet<string>();
            var duplicates = new List<string>();

            foreach (var config in _itemsConfig.Configs)
            {
                if (config == null || string.IsNullOrWhiteSpace(config.TypeId))
                    continue;

                if (!typeIdSet.Add(config.TypeId))
                {
                    duplicates.Add(config.TypeId);
                }
            }

            if (duplicates.Count == 0)
                Debug.Log(ProjectConfigEditorWindowConstants.ALL_TYPES_UNIQUE_MESSAGE);
            else
                Debug.LogWarning($"{ProjectConfigEditorWindowConstants.DUPLICATE_TYPEIDS_FOUND_WARNING}: {string.Join(", ", duplicates.Distinct())}");
        }
    }
}

#endif