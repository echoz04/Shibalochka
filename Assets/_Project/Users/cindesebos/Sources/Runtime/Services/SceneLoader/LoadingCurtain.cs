using UnityEngine;
using VContainer;

namespace Sources.Runtime.Services.SceneLoader
{
    public class LoadingCurtain : MonoBehaviour
    {
        [SerializeField] private GameObject _view;

        private ISceneLoader _sceneLoader;

        [Inject]
        private void Construct(ISceneLoader sceneLoader)
        {
            Debug.Log($"LoadingCurtain constructed with sceneLoader: {sceneLoader}");

            _sceneLoader = sceneLoader;

            _sceneLoader.OnLoadingStarted += Show;
            _sceneLoader.OnLoadingEnded += Hide;
        }

        private void Show()
        {
            _view.SetActive(true);
        }

        private void Hide()
        {
            _view.SetActive(false);
        }

        private void OnDestroy()
        {
            _sceneLoader.OnLoadingStarted -= Show;
            _sceneLoader.OnLoadingEnded -= Hide;
        }
    }
}
