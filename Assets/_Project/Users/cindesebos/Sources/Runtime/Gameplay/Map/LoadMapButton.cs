using Sources.Runtime.Services.SceneLoader;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Sources.Runtime.Gameplay.Map
{
    public class LoadMapButton : MonoBehaviour
    {
        [SerializeField] private GameObject _button;

        private ISceneLoader _sceneLoader;
        private Scene _scene;

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

        private void ShowButton(Scene scene)
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
            if (!IsCurrentScene(_scene))
                _sceneLoader.LoadScene(_scene);
        }

        private bool IsCurrentScene(string sceneName) => SceneManager.GetActiveScene().name == sceneName;
    }
}
