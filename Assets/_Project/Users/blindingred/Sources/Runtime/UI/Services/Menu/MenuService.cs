using Cysharp.Threading.Tasks;
using Sources.SceneManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using VContainer;

namespace Sources.UI.Services
{
    public class MenuService : IUIService
    {
        private IAddressableSceneLoader _sceneLoader;
        private AssetReference[] _scenesToLoad;
        
        private MenuView _view;
        public IUIView View => _view;

        [Inject]
        private void Construct(
            IAddressableSceneLoader sceneLoader,
            [Key("scene_island")] AssetReference gameplayScene,
            MenuView menuView)
        {
            _sceneLoader = sceneLoader;
            _scenesToLoad = new[]
            {
                gameplayScene
            };
            
            _view = menuView;
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
            _sceneLoader.LoadScenes(
                _scenesToLoad, LoadSceneMode.Additive,
                () => _sceneLoader.ActivateAllScenes(false)
            ).Forget();
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