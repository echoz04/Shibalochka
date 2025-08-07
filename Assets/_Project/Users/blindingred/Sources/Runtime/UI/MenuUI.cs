using blindingred;
using Cysharp.Threading.Tasks;
using Sources._Project.Users.blindingred.Sources.Runtime.Interfaces;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;

namespace Sources
{
    public class MenuUI : MonoBehaviour, IUIView
    {
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _creditsButton;
        [SerializeField] private Button _exitButton;

        private IAddressableSceneLoader _sceneLoader;
        private AssetReference[] _scenes;

        [Inject]
        private void Construct(
            IAddressableSceneLoader sceneLoader,
            [Key("scene_ui")] AssetReference uiScene,
            [Key("scene_island")] AssetReference gameplayScene)
        {
            _sceneLoader = sceneLoader;
            _scenes = new[]
            {
                uiScene,
                gameplayScene
            };
        }
        
        private void OnEnable()
        {
            _startGameButton.onClick.AddListener(OnStartGame);
            _settingsButton.onClick.AddListener(OnSettings);
            _creditsButton.onClick.AddListener(OnCredits);
            _exitButton.onClick.AddListener(OnExit);
        }

        private void OnDisable()
        {
            _startGameButton.onClick.RemoveListener(OnStartGame);
            _settingsButton.onClick.RemoveListener(OnSettings);
            _creditsButton.onClick.RemoveListener(OnCredits);
            _exitButton.onClick.RemoveListener(OnExit);
        }

        void OnStartGame()
        {
            _sceneLoader.LoadScenes(
                _scenes, LoadSceneMode.Additive,
                () => _sceneLoader.ActivateAllScenes(true)
                ).Forget();
        }

        void OnSettings()
        {
            Debug.Log("Settings");
        }

        void OnCredits()
        {
            Debug.Log("Credits");
        }

        void OnExit()
        {
            Application.Quit();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
