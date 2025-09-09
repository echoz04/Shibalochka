using Sources.SceneManagement;
using Sources.Signals;
using UnityEngine;
using VContainer;

namespace Sources.UI.Services
{
    public class MenuService : IUIService
    {
        private IAddressableSceneLoader _sceneLoader;
        private ScenesData _scenesData;
        
        private ISignalBus _signalBus;
        
        private MenuView _view;
        public IUIView View => _view;

        [Inject]
        private void Construct(
            IAddressableSceneLoader sceneLoader,
            ScenesData scenesData,
            MenuView menuView,
            ISignalBus signalBus)
        {
            _sceneLoader = sceneLoader;
            _scenesData = scenesData;
            _view = menuView;
            _signalBus = signalBus;
        }

        public void Enable()
        {
            Subscribe();
            _view.Show();
        }

        public void Disable()
        {
            Unsubscribe();
            _view.Hide();
        }

        private void Subscribe()
        {
            _view.Subscribe(_view.StartGameKey, OnStartGame);
            _view.Subscribe(_view.SettingsKey, OnSettings);
            _view.Subscribe(_view.CreditsKey, OnCredits);
            _view.Subscribe(_view.ExitKey, OnExitGame);
        }

        private void Unsubscribe()
        {
            _view.Unsubscribe(_view.StartGameKey, OnStartGame);
            _view.Unsubscribe(_view.SettingsKey, OnSettings);
            _view.Unsubscribe(_view.CreditsKey, OnCredits);
            _view.Unsubscribe(_view.ExitKey, OnExitGame);
        }

        private void OnStartGame()
        {
            var gameplayScene = _scenesData.GetSceneBindingByKey(SceneKey.IslandTropical).Scene;
            
            _signalBus.Fire(new LoadingSceneSignal(gameplayScene));
            _signalBus.Fire(new ScreenChangeSignal(typeof(LoadingScreen)));   
        }

        private void OnSettings()
        {
            Debug.Log("Settings");
        }

        private void OnCredits()
        {
            Debug.Log("Credits");
        }

        private void OnExitGame()
        {
            Application.Quit();
        }
    }
}