using VContainer;

namespace Sources
{
    public class InputHandler
    {
        private CharacterInput _characterInput;

        [Inject]
        private void Construct(CharacterInput characterInput)
        {
            _characterInput = characterInput;
        }
        
        public void SetInputScheme(InputState inputState)
        {
            _characterInput.Disable();
            switch (inputState)
            {
                case InputState.Main: 
                    _characterInput.Game.Enable();
                    _characterInput.Camera.Enable();
                    break;
                case InputState.Fishing:
                    _characterInput.Fishing.Enable();
                    break;
                case InputState.Inventory:
                    _characterInput.Inventory.Enable();
                    break;
            }
        }
    }
}