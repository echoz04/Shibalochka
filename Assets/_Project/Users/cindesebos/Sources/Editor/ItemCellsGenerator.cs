using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sources.Runtime.Gameplay.Configs.Items;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Sources.Editor
{
    public class ItemCellsGenerator
    {
        public static void GenerateCells(
            ItemConfig config,
            Sprite sprite,
            RectTransform imageRectTransform,
            RectTransform parentRectTransform,
            float cellSize,
            float spacing,
            float minOverlapAreaRatio)
        {
            if (sprite == null || config == null)
                return;

            Texture2D texture = sprite.texture;
            Rect spriteRect = sprite.rect;
            Vector2 pivot = sprite.pivot;

            var canvas = imageRectTransform.GetComponentInParent<Canvas>();
            var camera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

            Rect imageWorldRect = GetWorldRect(imageRectTransform);
            var cellRects = GetCellRects(imageWorldRect, cellSize, spacing);
            var cellPositions = new List<Vector3>();

            foreach (var cell in cellRects)
            {
                if (IsValidCell(cell, imageWorldRect, cellSize, minOverlapAreaRatio) &&
                    IsCellOpaque(cell, imageRectTransform, texture, spriteRect, pivot))
                {
                    Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(camera, GetCellCenter(cell, cellSize));

                    RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        parentRectTransform, screenPoint, camera, out Vector2 localPoint);

                    cellPositions.Add(localPoint);
                }
            }

#if UNITY_EDITOR
            config.SetCellPointsPosition(cellPositions);
            EditorUtility.SetDirty(config);
#endif
        }

        private static Rect GetWorldRect(RectTransform rectTransform)
        {
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            return new Rect(corners[0].x, corners[0].y,
                corners[2].x - corners[0].x, corners[2].y - corners[0].y);
        }

        private static Rect GetIntersectionRect(Rect a, Rect b)
        {
            float xMin = Mathf.Max(a.xMin, b.xMin);
            float yMin = Mathf.Max(a.yMin, b.yMin);
            float xMax = Mathf.Min(a.xMax, b.xMax);
            float yMax = Mathf.Min(a.yMax, b.yMax);

            if (xMax < xMin || yMax < yMin)
                return Rect.zero;

            return Rect.MinMaxRect(xMin, yMin, xMax, yMax);
        }

        private static bool IsValidCell(Rect cell, Rect imageWorldRect, float cellSize, float minOverlapRatio)
        {
            Rect intersection = GetIntersectionRect(cell, imageWorldRect);
            float intersectionArea = intersection.width * intersection.height;
            float cellArea = cellSize * cellSize;

            return intersectionArea > 0.001f &&
                   cellArea > 0.001f &&
                   (intersectionArea / cellArea) >= minOverlapRatio;
        }

        private static Vector3 GetCellCenter(Rect cell, float cellSize)
        {
            return new Vector3(cell.x + cellSize / 2f, cell.y + cellSize / 2f, 0);
        }

        private static IEnumerable<Rect> GetCellRects(Rect worldRect, float cellSize, float spacing)
        {
            float effectiveSize = cellSize + spacing;

            int countX = Mathf.CeilToInt((worldRect.width - 0.01f) / effectiveSize);
            int countY = Mathf.CeilToInt((worldRect.height - 0.01f) / effectiveSize);

            float totalWidth = countX * effectiveSize - spacing;
            float totalHeight = countY * effectiveSize - spacing;

            float offsetX = worldRect.xMin + (worldRect.width - totalWidth) / 2f;
            float offsetY = worldRect.yMin + (worldRect.height - totalHeight) / 2f;

            for (int y = 0; y < countY; y++)
            {
                for (int x = 0; x < countX; x++)
                {
                    float left = offsetX + x * effectiveSize;
                    float bottom = offsetY + y * effectiveSize;
                    yield return new Rect(left, bottom, cellSize, cellSize);
                }
            }
        }

        private static bool IsCellOpaque(Rect cellWorldRect, RectTransform imageRT, Texture2D texture, Rect spriteRect, Vector2 pivot)
        {
            Matrix4x4 worldToLocal = imageRT.worldToLocalMatrix;
            int samples = 5;
            int opaqueCount = 0;
            int total = 0;

            for (int y = 0; y < samples; y++)
            {
                for (int x = 0; x < samples; x++)
                {
                    float px = Mathf.Lerp(cellWorldRect.xMin, cellWorldRect.xMax, x / (float)(samples - 1));
                    float py = Mathf.Lerp(cellWorldRect.yMin, cellWorldRect.yMax, y / (float)(samples - 1));

                    Vector3 local = worldToLocal.MultiplyPoint3x4(new Vector3(px, py));
                    Vector2 pivoted = new(local.x + pivot.x, local.y + pivot.y);

                    int texX = Mathf.RoundToInt(spriteRect.x + pivoted.x);
                    int texY = Mathf.RoundToInt(spriteRect.y + pivoted.y);

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
    }
}
