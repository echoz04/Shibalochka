using Sources.Runtime.Services.SceneLoader;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Sources.Runtime.Menu
{
    public class LoadGameplayButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Scene _gameplayScene;

        private void OnValidate()
        {
            _button ??= GetComponent<Button>();
        }

        [Inject]
        private void Construct(ISceneLoader sceneLoader)
        {
            _button.onClick.AddListener(delegate
            {
                sceneLoader.LoadScene(_gameplayScene);
            });
        }
    }
}
