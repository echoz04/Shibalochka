using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Sources.UI.Services
{
    public class MenuView : UIViewBase
    {
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _creditsButton;
        [SerializeField] private Button _exitButton;

        public readonly string StartGameKey = "Start";
        public readonly string SettingsKey = "Settings";
        public readonly string CreditsKey = "Credits";
        public readonly string ExitKey = "Exit";

        [Inject]
        private void Construct()
        {
            _actions = new Dictionary<string, Action>
            {
                {StartGameKey, null},
                {SettingsKey, null},
                {CreditsKey, null},
                {ExitKey, null}
            };
        }

        public override void Show(Action onComplete = null)
        {
            Listen();
            base.Show(onComplete);
        }

        public override void Hide(Action onComplete = null)
        {
            Unlisten();
            base.Hide(onComplete);
        }

        void Listen()
        {
            _startGameButton.onClick.AddListener(() => _actions[StartGameKey].Invoke());
            _settingsButton.onClick.AddListener(() => _actions[SettingsKey].Invoke());
            _creditsButton.onClick.AddListener(() => _actions[CreditsKey].Invoke());
            _exitButton.onClick.AddListener(() => _actions[ExitKey].Invoke());
        }

        void Unlisten()
        {
            _startGameButton.onClick.RemoveAllListeners();
            _settingsButton.onClick.RemoveAllListeners();
            _creditsButton.onClick.RemoveAllListeners();
            _exitButton.onClick.RemoveAllListeners();
        }
    }
}