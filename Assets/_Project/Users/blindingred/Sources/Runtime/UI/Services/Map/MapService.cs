using System;
using Sources.SceneManagement;
using Sources.Signals;
using VContainer;
using VContainer.Unity;

namespace Sources.UI.Services
{
    public class MapService:IUIService, IInitializable, IDisposable
    {
        private CharacterInput _characterInput;
        private ISignalBus _signalBus;
        private UIScreenManager _uiScreenManager;
        private ScenesData _scenesData;

        private MapView _view;
        
        public IUIView View { get; }
        
        [Inject]
        private void Construct(
            MapView hudView,
            CharacterInput characterInput,
            ISignalBus signalBus,
            ScenesData scenesData
            )
        {
            _characterInput = characterInput;
            _view = hudView;
            _signalBus = signalBus;
            _scenesData = scenesData;
        }

        public void Initialize()
        {
            _view.Subscribe(_view.ShopIslandKey, OnShopIsland);
            _view.Subscribe(_view.TropicalIslandKey, OnTropicalIsland);
            _view.Subscribe(_view.WinterIslandKey, OnWinterIsland);
            _view.Subscribe(_view.GoToIslandKey, OnGoToIsland);
        }

        public void Dispose()
        {
            _view.Unsubscribe(_view.ShopIslandKey, OnShopIsland);
            _view.Unsubscribe(_view.TropicalIslandKey, OnTropicalIsland);
            _view.Unsubscribe(_view.WinterIslandKey, OnWinterIsland);
            _view.Unsubscribe(_view.GoToIslandKey, OnGoToIsland);
        }

        public void Enable()
        {
            _view.Show();
            _characterInput.Map.Enable();
        }

        public void Disable()
        {
            _view.Hide();
            _characterInput.Map.Disable();
        }

        private void OnShopIsland()
        {
            var assetReference = _scenesData.GetSceneBindingByKey(SceneKey.Shop).Scene;
            _signalBus.Fire(new LoadingSceneSignal(assetReference));
        } 

        private void OnTropicalIsland()
        {
            var assetReference = _scenesData.GetSceneBindingByKey(SceneKey.IslandTropical).Scene;
            _signalBus.Fire(new LoadingSceneSignal(assetReference));
        }

        private void OnWinterIsland()
        {
            var assetReference = _scenesData.GetSceneBindingByKey(SceneKey.IslandWinter).Scene;
            _signalBus.Fire(new LoadingSceneSignal(assetReference));
        }

        private void OnGoToIsland()
        {
            _signalBus.Fire(new ScreenChangeSignal(typeof(LoadingScreen)));
        }
    }
}