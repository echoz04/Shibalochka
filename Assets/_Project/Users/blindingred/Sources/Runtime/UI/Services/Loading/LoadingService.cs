using System;
using Cysharp.Threading.Tasks;
using Sources.SceneManagement;
using Sources.Signals;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace Sources.UI.Services.Loading
{
    public class LoadingService:IUIService, IInitializable, IDisposable
    {
        private ISignalBus _signalBus;
        private FadeService _fadeService;
        private IAddressableSceneLoader _sceneLoader;

        private AssetReference _sceneToLoad;
        
        private LoadingView _view;
        public IUIView View { get; }
        
        [Inject]
        private void Construct(
            LoadingView view,
            ISignalBus signalBus,
            FadeService fadeService,
            IAddressableSceneLoader sceneLoader
            )
        {
            _view = view;
            _signalBus = signalBus;
            _fadeService = fadeService;
            _sceneLoader = sceneLoader;
        }

        public void Initialize()
        {
            _view.Hide();
            _signalBus.Subscribe<LoadingSceneSignal>(SetSceneToLoad, false);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<LoadingSceneSignal>(SetSceneToLoad);
        }

        public void Enable()
        {
            _fadeService.FadeIn(() =>
            {
                _view.Show();
                StartLoading();
            });
        }

        public void Disable()
        {
            _view.Hide();
        }

        private void SetSceneToLoad(LoadingSceneSignal signal)
        {
            _sceneToLoad = signal.SceneToLoad;
        }

        private async void StartLoading()
        {
            try
            {
                await ProcessLoading();
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        private async UniTask ProcessLoading()
        {
            await UniTask.Yield(PlayerLoopTiming.Update);
            
            var currentScene = SceneManager.GetActiveScene();
            
            await _sceneLoader.LoadScene(
                _sceneToLoad, 
                LoadSceneMode.Additive,
                () =>
                {
                    Debug.Log($"Scene loaded : {_view.gameObject.activeSelf}");
                    _sceneLoader.ActivateScene(_sceneToLoad);
                    _sceneLoader.UnloadScene(currentScene);
                    
                    _view.Hide();
                    _fadeService.FadeOut(
                        () => _signalBus.Fire(new ScreenChangeSignal(typeof(MainScreen)))
                    );
                });
        }
    }
}