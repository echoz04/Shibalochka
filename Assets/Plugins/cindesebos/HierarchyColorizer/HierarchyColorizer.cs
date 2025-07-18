#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace cindesebos.HierarchyCustomizer
{
    [InitializeOnLoad]
    public static class HierarchyColorizer
    {
        static readonly Dictionary<string, Color> IconColorMap = new()
        {
            { "sv_label_0", new Color(0.65f, 0.65f, 0.65f, 0.3f) },
            { "sv_label_1", new Color(0.29f, 0.52f, 0.82f, 0.3f) },
            { "sv_label_2", new Color(0.16f, 0.74f, 0.66f, 0.3f) },
            { "sv_label_3", new Color(0.49f, 0.81f, 0.27f, 0.3f) },
            { "sv_label_4", new Color(0.96f, 0.81f, 0.17f, 0.3f) },
            { "sv_label_5", new Color(0.95f, 0.61f, 0.12f, 0.3f) },
            { "sv_label_6", new Color(0.93f, 0.3f, 0.3f, 0.3f) },
            { "sv_label_7", new Color(0.7f, 0.42f, 0.84f, 0.3f) }
        };

        static HierarchyColorizer()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
        }

        static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

            if (obj == null) return;

            GUIContent iconContent = EditorGUIUtility.ObjectContent(obj, typeof(GameObject));

            if (iconContent?.image == null) return;

            string iconName = iconContent.image.name;

            if (iconName.EndsWith("_on")) iconName = iconName.Replace("_on", "");

            if (IconColorMap.TryGetValue(iconName, out var bgColor))
            {
                EditorGUI.DrawRect(selectionRect, bgColor);

                Rect labelRect = new Rect(selectionRect.x + 17, selectionRect.y - 1, selectionRect.width, selectionRect.height);

                EditorGUI.LabelField(labelRect, obj.name, EditorStyles.label);
            }
        }
    }
}

#endif