#if UNITY_EDITOR

using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sources.Runtime.Gameplay.Configs;
using UnityEditor;
using UnityEngine;

namespace Sources.Editor
{
    public class ProjectConfigEditorWindow : OdinEditorWindow
    {
        private const string WINDOW_TITLE = "Project Config Editor";

        [MenuItem("Tools/Project/Open Project Config Editor")]
        private static void OpenWindow()
        {
            var window = GetWindow<ProjectConfigEditorWindow>();
            window.titleContent = new GUIContent(WINDOW_TITLE);
            window.Show();
        }

        [SerializeField, HideInInspector]
        private ProjectConfig _projectConfig;

        [ShowIf(nameof(_projectConfig)), InlineProperty, PropertyOrder(1)]
        [TabGroup("Configs", "Camera"), LabelText("Camera Config")]
        [SerializeField] private CameraConfig _cameraConfig;

        [ShowIf(nameof(_projectConfig)), InlineProperty, PropertyOrder(1)]
        [TabGroup("Configs", "UI"), LabelText("UI Config")]
        [SerializeField] private UIConfig _uiConfig;

        [ShowIf(nameof(_projectConfig)), InlineProperty, PropertyOrder(1)]
        [TabGroup("Configs", "Inventory"), LabelText("Inventory Config")]
        [SerializeField] private InventoryConfig _inventoryConfig;

        [ShowIf(nameof(_projectConfig)), InlineProperty, PropertyOrder(1)]
        [TabGroup("Configs", "Discord"), LabelText("Discord Config")]
        [SerializeField] private DiscordConfig _discordConfig;

        protected override void OnEnable()
        {
            base.OnEnable();

            if (_projectConfig == null)
            {
                string[] guids = AssetDatabase.FindAssets("t:ProjectConfig");

                if (guids.Length > 0)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    _projectConfig = AssetDatabase.LoadAssetAtPath<ProjectConfig>(path);
                }
                else
                {
                    Debug.LogWarning("⚠️ ProjectConfig asset not found in project.");

                    return;
                }
            }

            _cameraConfig = _projectConfig.CameraConfig;
            _uiConfig = _projectConfig.UIConfig;
            _inventoryConfig = _projectConfig.InventoryConfig;
            _discordConfig = _projectConfig.DiscordConfig;
        }
    }
}

#endif
