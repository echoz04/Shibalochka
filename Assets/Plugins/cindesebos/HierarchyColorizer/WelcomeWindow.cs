#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace cindesebos.HierarchyCustomizer
{
    [InitializeOnLoad]
    public static class WelcomeWindowOnImport
    {
        private const string WelcomeShownKey = "Hierarchy Colorizer";

        static WelcomeWindowOnImport()
        {
            if (!EditorPrefs.GetBool(WelcomeShownKey, false))
            {
                EditorApplication.delayCall += () =>
                {
                    WelcomeWindow.ShowWindow();

                    EditorPrefs.SetBool(WelcomeShownKey, true);
                };
            }
        }
    }

    public class WelcomeWindow : EditorWindow
    {
        private Texture2D _screenshot;
        
        public static void ShowWindow()
        {
            var window = GetWindow<WelcomeWindow>("Hierarchy Colorizer");
            window.minSize = new Vector2(350, 150);
        }

        private void OnEnable() => _screenshot = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Plugins/cindesebos/HierarchyColorizer/screen.png");

        private void OnGUI()
        {
            GUILayout.Space(10);

            var centeredStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 15
            };

            GUILayout.Label("🚀Welcome!🚀", centeredStyle);

            if (_screenshot != null) GUI.DrawTexture(new Rect(10, 50, 350, 150), _screenshot, ScaleMode.ScaleToFit);
            else EditorGUILayout.HelpBox("Картинка не найдена", MessageType.Warning);

            GUILayout.Label("Customize in Tools → cindesebos → Editor Auto Saver", EditorStyles.miniBoldLabel);

            GUILayout.Space(135);

            if (GUILayout.Button("Kiss you, bro🫡")) Close();
        }
    }
}

#endif