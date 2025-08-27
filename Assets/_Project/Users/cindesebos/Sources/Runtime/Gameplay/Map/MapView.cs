using Sources.Runtime.Gameplay.Camera;
using Sources.Runtime.Gameplay.Inventory;
using Sources.Runtime.Gameplay.MiniGames.Fishing;
using UnityEngine;
using VContainer;

namespace Sources.Runtime.Gameplay.Map
{
    public class MapView : MonoBehaviour
    {
        public bool IsVisible => _canvas.enabled;

        [SerializeField] private Canvas _canvas;

        private StaminaHandler _staminaHandler;
        private CharacterInput _characterInput;
        private InventoryRoot _inventoryRoot;
        private CameraRotator _cameraRotator;

        [Inject]
        private void Construct(StaminaHandler staminaHandler, CharacterInput characterInput, InventoryRoot inventoryRoot, CameraRotator cameraRotator)
        {
            _staminaHandler = staminaHandler;
            _characterInput = characterInput;
            _inventoryRoot = inventoryRoot;
            _cameraRotator = cameraRotator;
        }

        private void Awake()
        {
            // _characterInput.UI.ToggleMapVisibility.performed += ctx => ToggleVisibility();
        }

        private void OnDestroy()
        {
            // _characterInput.UI.ToggleMapVisibility.performed -= ctx => ToggleVisibility();
        }

        private void ToggleVisibility()
        {
            if (_staminaHandler.IsStarted == true || _inventoryRoot.IsVisible == true)
                return;

            _canvas.enabled = !_canvas.enabled;

            if (_canvas.enabled == true)
            {
                // _cameraRotator.Disable();
            }
            else
            {
                // _cameraRotator.Enable();
            }
        }
    }
}
