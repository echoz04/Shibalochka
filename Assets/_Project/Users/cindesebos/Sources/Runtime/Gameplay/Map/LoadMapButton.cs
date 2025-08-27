using Sources.Runtime.Gameplay.MiniGames.Fishing;
using Sources.Runtime.Services.SceneLoader;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace Sources.Runtime.Gameplay.Map
{
    public class LoadMapButton : MonoBehaviour
    {
        [SerializeField] private GameObject _button;

        private ISceneLoader _sceneLoader;
        private Sources.Runtime.Services.SceneLoader.Scene _scene;

        [Inject]
        private void Construct(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        private void Start()
        {
            BaseMapSelectorButton.IslandSelected += ShowButton;
            BaseMapSelectorButton.IslandUnselected += HideButton;
        }

        private void OnDestroy()
        {
            BaseMapSelectorButton.IslandSelected -= ShowButton;
            BaseMapSelectorButton.IslandUnselected -= HideButton;
        }

        private void ShowButton(Sources.Runtime.Services.SceneLoader.Scene scene)
        {
            _button.SetActive(true);

            _scene = scene;
        }

        private void HideButton()
        {
            _button.SetActive(false);
        }

        public void Load()
        {
            if (IsCurrentScene(_scene.ToString()) == false)
                _sceneLoader.LoadScene(_scene);
        }

        private bool IsCurrentScene(string sceneName)
        {
            var currentSceneName = SceneManager.GetActiveScene().name;
            var sceneToLoadName = sceneName;

            Debug.Log($"currentSceneName: {currentSceneName}  sceneToLoadName: {sceneToLoadName}");

            return currentSceneName == sceneToLoadName;
        }
    }
}
