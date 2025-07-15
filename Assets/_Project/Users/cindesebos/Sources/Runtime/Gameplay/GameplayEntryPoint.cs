using Sources.Runtime.Gameplay.MiniGames.Fishing;
using UnityEngine;
using Zenject;

namespace Sources.Runtime.Gameplay
{
    public class GameplayEntryPoint : MonoBehaviour
    {
        private CharacterInput _characterInput;

        [Inject]
        private void Construct(CharacterInput characterInput, FishingMiniGameBootstrapper fishingMiniGameBootstrapper)
        {
            _characterInput = characterInput;
        }

        private void Start()
        {
            _characterInput.Enable();
        }

        private void OnDestroy()
        {
            _characterInput.Disable();
        }
    }
}