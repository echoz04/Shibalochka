using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Sources.Runtime.Gameplay.Configs;
using Sources.Runtime.Gameplay.Configs.Items;

namespace Sources.Editor
{
    public class ItemCellPointsGenerator : EditorWindow
    {
        private ProjectConfig _projectConfig;

        private float _minOverlapAreaRatio = 0.05f;
        private int _maxCellsCount = 100;
        private float _delaySeconds = 0.1f;

        private Queue<ItemConfig> _itemQueue;
        private double _nextStepTime;
        private int _processedCount;

        [MenuItem("Tools/Project/Item CellPoint Generator")]
        private static void ShowWindow()
        {
            GetWindow<ItemCellPointsGenerator>("CellPoint Generator");
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("CellPoint Generator", EditorStyles.boldLabel);

            _projectConfig = (ProjectConfig)EditorGUILayout.ObjectField("Project Config", _projectConfig, typeof(ProjectConfig), false);
            _minOverlapAreaRatio = EditorGUILayout.Slider("Min Overlap Area Ratio", _minOverlapAreaRatio, 0f, 1f);
            _maxCellsCount = EditorGUILayout.IntField("Max Cells Count (Per Item)", _maxCellsCount);
            _delaySeconds = EditorGUILayout.Slider("Delay Between Items (sec)", _delaySeconds, 0f, 2f);

            if (GUILayout.Button("Generate For All Items"))
            {
                if (_projectConfig == null || _projectConfig.ItemsConfig == null)
                {
                    Debug.LogError("❌ ProjectConfig or ItemsConfig is not assigned.");
                    return;
                }

                StartGenerationWithDelay();
            }
        }

        private void StartGenerationWithDelay()
        {
            var items = _projectConfig.ItemsConfig.Configs
                .Where(item => item != null && item.Icon != null && item.ItemCellPointPrefab != null)
                .ToList();

            if (items.Count == 0)
            {
                Debug.LogWarning("No valid items to process.");
                return;
            }

            _itemQueue = new Queue<ItemConfig>(items);
            _processedCount = 0;
            _nextStepTime = EditorApplication.timeSinceStartup + _delaySeconds;

            EditorApplication.update += ProcessQueueStep;
        }

        private void ProcessQueueStep()
        {
            if (_itemQueue == null || _itemQueue.Count == 0)
            {
                EditorApplication.update -= ProcessQueueStep;
                Debug.Log($"✅ Completed generation for {_processedCount} items.");
                return;
            }

            if (EditorApplication.timeSinceStartup < _nextStepTime)
                return;

            _nextStepTime = EditorApplication.timeSinceStartup + _delaySeconds;

            ItemConfig currentItem = _itemQueue.Dequeue();
            List<Vector3> points = GeneratePoints(currentItem);
            SetCellPoints(currentItem, points);

            Debug.Log($"✅ {currentItem.name} — {points.Count} cells");
            _processedCount++;
        }

        // ... (все вспомогательные методы GeneratePoints, IsValidCell, etc. без изменений)

        private List<Vector3> GeneratePoints(ItemConfig itemConfig)
        {
            List<Vector3> cellPoints = new();

            Sprite sprite = itemConfig.Icon;
            Texture2D texture = sprite.texture;
            Rect spriteRect = sprite.rect;
            Vector2 pivot = sprite.pivot;
            int spriteWidth = Mathf.RoundToInt(spriteRect.width);
            int spriteHeight = Mathf.RoundToInt(spriteRect.height);

            float cellSize = _projectConfig.InventoryConfig.InventoryCellSize;
            float spacing = _projectConfig.InventoryConfig.InventorySpacing;
            float effectiveSize = cellSize + spacing;

            int countX = Mathf.CeilToInt((spriteWidth - 0.01f) / effectiveSize);
            int countY = Mathf.CeilToInt((spriteHeight - 0.01f) / effectiveSize);

            float offsetX = (spriteWidth - (countX * effectiveSize - spacing)) / 2f;
            float offsetY = (spriteHeight - (countY * effectiveSize - spacing)) / 2f;

            for (int y = 0; y < countY; y++)
            {
                for (int x = 0; x < countX; x++)
                {
                    if (cellPoints.Count >= _maxCellsCount)
                        break;

                    float left = offsetX + x * effectiveSize;
                    float bottom = offsetY + y * effectiveSize;
                    Rect cell = new(left, bottom, cellSize, cellSize);

                    if (IsValidCell(cell, spriteWidth, spriteHeight, cellSize) &&
                        IsCellOpaque(cell, sprite, texture, pivot))
                    {
                        float cx = cell.x + cellSize / 2f - spriteWidth / 2f;
                        float cy = cell.y + cellSize / 2f - spriteHeight / 2f;
                        cellPoints.Add(new Vector3(cx, cy, 0));
                    }
                }
            }

            return cellPoints;
        }

        private bool IsValidCell(Rect cell, float texWidth, float texHeight, float cellSize)
        {
            Rect imageRect = new(0, 0, texWidth, texHeight);
            Rect intersection = GetIntersectionRect(cell, imageRect);

            float intersectionArea = intersection.width * intersection.height;
            float cellArea = cellSize * cellSize;

            return intersectionArea > 0.001f && cellArea > 0.001f &&
                   (intersectionArea / cellArea) >= _minOverlapAreaRatio;
        }

        private Rect GetIntersectionRect(Rect a, Rect b)
        {
            float xMin = Mathf.Max(a.xMin, b.xMin);
            float yMin = Mathf.Max(a.yMin, b.yMin);
            float xMax = Mathf.Min(a.xMax, b.xMax);
            float yMax = Mathf.Min(a.yMax, b.yMax);

            if (xMax < xMin || yMax < yMin)
                return Rect.zero;

            return Rect.MinMaxRect(xMin, yMin, xMax, yMax);
        }

        private bool IsCellOpaque(Rect cell, Sprite sprite, Texture2D texture, Vector2 pivot)
        {
            Rect spriteRect = sprite.rect;
            int samples = 5;
            int opaqueCount = 0;
            int total = 0;

            for (int y = 0; y < samples; y++)
            {
                for (int x = 0; x < samples; x++)
                {
                    float px = Mathf.Lerp(cell.xMin, cell.xMax, x / (float)(samples - 1));
                    float py = Mathf.Lerp(cell.yMin, cell.yMax, y / (float)(samples - 1));

                    int texX = Mathf.RoundToInt(spriteRect.x + px);
                    int texY = Mathf.RoundToInt(spriteRect.y + py);

                    if (texX >= 0 && texX < texture.width && texY >= 0 && texY < texture.height)
                    {
                        if (texture.GetPixel(texX, texY).a > 0.1f)
                            opaqueCount++;
                        total++;
                    }
                }
            }

            return total > 0 && (opaqueCount / (float)total) > 0.2f;
        }

        private void SetCellPoints(ItemConfig config, List<Vector3> points)
        {
            var so = new SerializedObject(config);
            var property = so.FindProperty("_cellPointsPosition");
            property.ClearArray();

            for (int i = 0; i < points.Count; i++)
            {
                property.InsertArrayElementAtIndex(i);
                property.GetArrayElementAtIndex(i).vector3Value = points[i];
            }

            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(config);
            AssetDatabase.SaveAssets();
        }
    }
}
