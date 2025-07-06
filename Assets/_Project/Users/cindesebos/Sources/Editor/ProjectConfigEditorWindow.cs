using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Sources.Runtime.Gameplay.Configs;

namespace Sources.Editor
{
    public class ProjectConfigEditorWindow : EditorWindow
    {
        private readonly string[] _tabs = { "All Configs", "Item Configs" };
        private int _selectedTab;
        private ProjectConfig _config;
        private Vector2 _scrollAll;
        private Vector2 _scrollItems;

        [MenuItem(ProjectConfigConstants.PROJECT_CONFIG_EDITOR_PATH)]
        public static void OpenWindow()
        {
            var window = GetWindow<ProjectConfigEditorWindow>();
            window.titleContent = new GUIContent(ProjectConfigConstants.PROJECT_CONFIG_EDITOR_WINDOW_TITLE);
            window.minSize = new Vector2(800, 800);
        }

        private void OnGUI()
        {
            _selectedTab = GUILayout.Toolbar(_selectedTab, _tabs);
            EditorGUILayout.Space();

            switch (_selectedTab)
            {
                case 0:
                    DrawAllConfigsTab();
                    break;
                case 1:
                    DrawItemConfigsTab();
                    break;
            }
        }

        private void DrawAllConfigsTab()
        {
            _config = (ProjectConfig)EditorGUILayout.ObjectField("Project Config", _config, typeof(ProjectConfig), false);
            if (_config == null) return;

            EditorGUILayout.Space();
            _scrollAll = EditorGUILayout.BeginScrollView(_scrollAll);

            var serialized = new SerializedObject(_config);
            serialized.Update();
            SerializedProperty prop = serialized.GetIterator();
            bool enterChildren = true;
            while (prop.NextVisible(enterChildren))
            {
                EditorGUILayout.PropertyField(prop, true);
                enterChildren = false;
            }
            serialized.ApplyModifiedProperties();

            EditorGUILayout.EndScrollView();
        }

        private void DrawItemConfigsTab()
        {
            _config = (ProjectConfig)EditorGUILayout.ObjectField("Project Config", _config, typeof(ProjectConfig), false);
            if (_config == null) return;

            EditorGUILayout.Space();
            _scrollItems = EditorGUILayout.BeginScrollView(_scrollItems);

            GUILayout.Label("Item Configs", EditorStyles.boldLabel);
            DrawItemTableHeader();
            DrawItemList();

            EditorGUILayout.EndScrollView();
            GUILayout.Space(5);
            DrawItemTableButtons();
        }

        private void DrawItemTableHeader()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.Label("TypeId", GUILayout.Width(100));
            GUILayout.Label("TitleLid", GUILayout.Width(150));
            GUILayout.Label("CellOffSets", GUILayout.Width(150));
            GUILayout.Label("Icon", GUILayout.Width(90));
            GUILayout.Label("Delete", GUILayout.Width(45));
            EditorGUILayout.EndHorizontal();
        }

        private void DrawItemList()
        {
            var list = _config.ItemConfigs.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                if (item == null) continue;
                EditorGUILayout.BeginHorizontal("box");

                EditorGUI.BeginChangeCheck();
                string newId = EditorGUILayout.TextField(item.TypeId, GUILayout.Width(100));
                string newTitle = EditorGUILayout.TextField(item.TitleLid, GUILayout.Width(150));

                // Редактирование CellOffSets в виде строки "x1,y1;x2,y2"
                string offsetsStr = string.Join(";", item.CellOffSets.Select(o => $"{o.x},{o.y}"));
                string newOffsetsStr = EditorGUILayout.TextField(offsetsStr, GUILayout.Width(150));
                Vector2Int[] newOffsets = item.CellOffSets;
                if (newOffsetsStr != offsetsStr)
                {
                    var parts = newOffsetsStr.Split(new[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries);
                    var listOffsets = new List<Vector2Int>();
                    foreach (var part in parts)
                    {
                        var coords = part.Split(',');
                        if (coords.Length == 2 && int.TryParse(coords[0], out var x) && int.TryParse(coords[1], out var y))
                            listOffsets.Add(new Vector2Int(x, y));
                    }
                    newOffsets = listOffsets.ToArray();
                }

                Sprite newIcon = (Sprite)EditorGUILayout.ObjectField(item.Icon, typeof(Sprite), false, GUILayout.Width(90));

                if (GUILayout.Button("❌", GUILayout.Width(45)))
                {
                    _config.ItemConfigs.RemoveAt(i);
                    EditorUtility.SetDirty(_config);
                    break;
                }

                if (EditorGUI.EndChangeCheck())
                {
#if UNITY_EDITOR
                    item.SetValues(newId, newTitle, newOffsets, newIcon);
                    EditorUtility.SetDirty(item);
                    EditorUtility.SetDirty(_config);
#endif
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        private void DrawItemTableButtons()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("➕ Add Item", GUILayout.Height(25)))
                AddNewItem();
            if (GUILayout.Button("✅ Check Duplicates", GUILayout.Height(25)))
                CheckDuplicateIds();
            EditorGUILayout.EndHorizontal();
        }

        private void AddNewItem()
        {
#if UNITY_EDITOR
            var newItem = CreateInstance<ItemConfig>();
            string baseId = "Item_";
            var existing = _config.ItemConfigs.Where(x => x != null).Select(x => x.TypeId).ToHashSet();
            int idx = 1;
            string id;
            do { id = baseId + idx; idx++; } while (existing.Contains(id));
            newItem.SetValues(id, "NewTitle", new Vector2Int[0], null);

            string folder = "Assets/Resources/Configs/ItemConfigs";
            if (!System.IO.Directory.Exists(folder))
                System.IO.Directory.CreateDirectory(folder);
            string path = AssetDatabase.GenerateUniqueAssetPath($"{folder}/{id}.asset");
            AssetDatabase.CreateAsset(newItem, path);
            AssetDatabase.SaveAssets();

            _config.ItemConfigs.Add(newItem);
            EditorUtility.SetDirty(_config);
#endif
        }

        private void CheckDuplicateIds()
        {
            var ids = new HashSet<string>(); bool dup = false;
            foreach (var item in _config.ItemConfigs)
            {
                if (item == null) continue;
                if (!ids.Add(item.TypeId))
                {
                    Debug.LogWarning($"Duplicate TypeId: {item.TypeId}", item);
                    dup = true;
                }
            }
            if (!dup) Debug.Log("All TypeIds are unique.");
        }
    }
}