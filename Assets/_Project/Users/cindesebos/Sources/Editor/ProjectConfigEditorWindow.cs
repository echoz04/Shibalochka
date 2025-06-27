using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEditor;
using UnityEngine;
using Sources.Runtime.Gameplay.Configs.Fish;
using Sources.Runtime.Gameplay.Configs;

namespace Sources.Editor
{
    public class ProjectConfigEditorWindow : EditorWindow
    {
        private const string WindowTitle = "Project Config";
        private const string FishTabTitle = "Fish Configs";
        private const string MenuPath = "Tools/Edit Project Config";
        private const string FishAssetFolderPath = "Assets/Resources/Configs/FishConfigs";
        private const string FishRarityAssetFolderPath = "Assets/Resources/Configs/FishConfigs/Rarity";
        private const string DefaultFishTitle = "Shibiriba";
        private const string DefaultFishIdPrefix = "F_";

        private ProjectConfig _projectConfig;
        private Vector2 _scroll;
        private Vector2 _rarityScroll;

        [MenuItem(MenuPath)]
        public static void OpenWindow()
        {
            var window = GetWindow<ProjectConfigEditorWindow>(FishTabTitle);

            window.minSize = new Vector2(800, 800);
        }

        private void OnGUI()
        {
            minSize = new Vector2(800, 800);

            _scroll = EditorGUILayout.BeginScrollView(_scroll);

            GUILayout.Space(5);
            DrawProjectConfigField();

            if (_projectConfig == null)
            {
                EditorGUILayout.HelpBox("Select ProjectConfig", MessageType.Info);
                EditorGUILayout.EndScrollView();
                return;
            }

            GUILayout.Space(10);
            DrawMainPanels();
            GUILayout.Space(10);
            DrawSaveButton();

            EditorGUILayout.EndScrollView();
        }

        #region ProjectConfig

        private void DrawProjectConfigField()
        {
            _projectConfig = (ProjectConfig)EditorGUILayout.ObjectField(WindowTitle, _projectConfig, typeof(ProjectConfig), false);
        }

        private void DrawSaveButton()
        {
            if (GUILayout.Button("💾 Save"))
            {
                EditorUtility.SetDirty(_projectConfig);
                AssetDatabase.SaveAssets();
            }
        }

        private void DrawMainPanels()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            DrawFishTable();
            EditorGUILayout.EndVertical();

            DrawVerticalSeparator();

            EditorGUILayout.BeginVertical(GUILayout.Width(250));
            DrawRarityConfigs();
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
        }

        #endregion

        #region FishConfigs

        private void DrawFishTable()
        {
            GUILayout.Label("Fish Configs", EditorStyles.boldLabel);
            _scroll = EditorGUILayout.BeginScrollView(_scroll);

            DrawFishTableHeader();
            DrawFishList();

            EditorGUILayout.EndScrollView();

            GUILayout.Space(5);
            DrawFishTableButtons();
        }

        private void DrawFishTableHeader()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.Label("TypeId", GUILayout.Width(100));
            GUILayout.Label("Title", GUILayout.Width(150));
            GUILayout.Label("Icon", GUILayout.Width(90));
            GUILayout.Label("Rarity", GUILayout.Width(85));
            GUILayout.Label("Delete", GUILayout.Width(45));
            EditorGUILayout.EndHorizontal();
        }

        private void DrawFishList()
        {
            var fishConfigsList = _projectConfig.FishConfigs.ToList();

            for (int i = 0; i < fishConfigsList.Count; i++)
            {
                var fish = fishConfigsList[i];
                if (fish == null) continue;

                EditorGUILayout.BeginHorizontal("box");

                fish.TypeId = EditorGUILayout.TextField(fish.TypeId, GUILayout.Width(100));
                fish.Title = EditorGUILayout.TextField(fish.Title, GUILayout.Width(150));
                fish.Icon = (Sprite)EditorGUILayout.ObjectField(fish.Icon, typeof(Sprite), false, GUILayout.Width(90));
                fish.Rarity = (Rarity)EditorGUILayout.EnumPopup(fish.Rarity, GUILayout.Width(85));

                if (GUILayout.Button("❌", GUILayout.Width(45)))
                {
                    _projectConfig.FishConfigs.RemoveAt(i);
                    EditorUtility.SetDirty(_projectConfig);
                    break;
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        private void DrawFishTableButtons()
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("➕ Add Fish", GUILayout.Height(25)))
                AddNewFish();

            if (GUILayout.Button("✅ Check For Duplicated TypeIds", GUILayout.Height(25)))
                CheckDuplicateIds();

            EditorGUILayout.EndHorizontal();
        }

        private void AddNewFish()
        {
            var newFish = CreateInstance<FishConfig>();

            var existingIds = _projectConfig.FishConfigs
                .Where(f => f != null)
                .Select(f => f.TypeId)
                .ToHashSet();

            int index = 1;
            string newId;
            do
            {
                newId = $"{DefaultFishIdPrefix}{index:D2}";
                index++;
            } while (existingIds.Contains(newId));

            newFish.TypeId = newId;
            newFish.Title = DefaultFishTitle;
            newFish.Rarity = Rarity.Common;

            EnsureDirectoryExists(FishAssetFolderPath);
            string path = AssetDatabase.GenerateUniqueAssetPath($"{FishAssetFolderPath}/{newId}.asset");

            AssetDatabase.CreateAsset(newFish, path);
            AssetDatabase.SaveAssets();

            _projectConfig.FishConfigs.Add(newFish);
            EditorUtility.SetDirty(_projectConfig);
        }

        private void CheckDuplicateIds()
        {
            var ids = new HashSet<string>();
            bool hasDuplicates = false;

            foreach (var fish in _projectConfig.FishConfigs)
            {
                if (fish == null || string.IsNullOrEmpty(fish.TypeId)) continue;

                if (!ids.Add(fish.TypeId))
                {
                    Debug.LogWarning($"Дубликат ID найден: {fish.TypeId}", fish);
                    hasDuplicates = true;
                }
            }

            if (!hasDuplicates)
                Debug.Log("Все ID уникальны!");
        }

        #endregion

        #region RarityConfigs

        private void DrawRarityConfigs()
        {
            GUILayout.Label("Rarity Configs", EditorStyles.boldLabel);

            DrawRarityTableHeader();
            _rarityScroll = EditorGUILayout.BeginScrollView(_rarityScroll);

            var rarityValues = System.Enum.GetValues(typeof(Rarity)).Cast<Rarity>().ToList();

            EnsureAllRarityConfigsExist(rarityValues);
            CleanupInvalidRarityConfigs(rarityValues);

            DrawRarityConfigList();

            EditorGUILayout.EndScrollView();
        }

        private void DrawRarityTableHeader()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.Label("Rarity", GUILayout.Width(100));
            GUILayout.Label("Chance (%)", GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();
        }

        private void EnsureAllRarityConfigsExist(List<Rarity> rarities)
        {
            foreach (var rarity in rarities)
            {
                if (_projectConfig.RarityConfigs.Any(rc => rc != null && rc.Rarity == rarity)) continue;

                var newConfig = CreateInstance<RarityConfig>();
                newConfig.Rarity = rarity;
                newConfig.Chance = 0;

                EnsureDirectoryExists(FishRarityAssetFolderPath);
                string assetPath = AssetDatabase.GenerateUniqueAssetPath($"{FishRarityAssetFolderPath}/{rarity}.asset");

                AssetDatabase.CreateAsset(newConfig, assetPath);
                AssetDatabase.SaveAssets();

                _projectConfig.RarityConfigs.Add(newConfig);
                EditorUtility.SetDirty(_projectConfig);
            }
        }

        private void CleanupInvalidRarityConfigs(List<Rarity> validRarities)
        {
            _projectConfig.RarityConfigs.RemoveAll(rc => rc == null || !validRarities.Contains(rc.Rarity));
            EditorUtility.SetDirty(_projectConfig);
        }

        private void DrawRarityConfigList()
        {
            foreach (var rarityConfig in _projectConfig.RarityConfigs)
            {
                if (rarityConfig == null) continue;

                EditorGUILayout.BeginHorizontal("box");
                rarityConfig.Rarity = (Rarity)EditorGUILayout.EnumPopup(rarityConfig.Rarity, GUILayout.Width(100));
                rarityConfig.Chance = EditorGUILayout.IntField(rarityConfig.Chance, GUILayout.Width(100));
                EditorGUILayout.EndHorizontal();

                EditorUtility.SetDirty(rarityConfig);
            }
        }

        private void DrawVerticalSeparator()
        {
            var rect = GUILayoutUtility.GetRect(1, float.MaxValue, GUILayout.Width(1), GUILayout.ExpandHeight(true));

            EditorGUI.DrawRect(rect, new Color(0.3f, 0.3f, 0.3f, 1f));
        }

        #endregion

        #region Utilities

        private void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                AssetDatabase.Refresh();
            }
        }

        #endregion
    }
}