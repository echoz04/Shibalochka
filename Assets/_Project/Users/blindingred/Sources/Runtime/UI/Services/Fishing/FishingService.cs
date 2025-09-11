using System;
using Sources.Signals;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace Sources.UI.Services.Fishing
{
    public class FishingService : IUIService, IInitializable, IDisposable, ITickable
    {
        private CharacterInput _characterInput;
        private ISignalBus _signalBus;
        private FishingGame _fishingGame;

        private FishingView _view;
        public IUIView View { get; }

        [Inject]
        private void Construct(
            FishingView view,
            CharacterInput characterInput,
            ISignalBus signalBus,
            FishingGame fishingGame)
        {
            _characterInput = characterInput;
            _view = view;
            _signalBus = signalBus;
            _fishingGame = fishingGame;
        }
        
        public void Initialize()
        {
            
        }
        
        public void Dispose()
        {
            
        }

        public void Enable()
        {
            Subscribe();
            _view.SetupFishingView(_fishingGame.Barriers.ToArray());
            _view.Show();
        }

        public void Disable()
        {
            Unsubscribe();
            _view.Hide();
        }

        void Subscribe()
        {
            _characterInput.Fishing.Enable();
            _characterInput.Fishing.Pull.performed += Pull;
            _characterInput.Fishing.Pull.canceled += CancelPull;
            _characterInput.Fishing.SmashBarrier.performed += SmashBarrier;
        }

        void Unsubscribe()
        {
            _characterInput.Fishing.Disable();
            _characterInput.Fishing.Pull.performed -= Pull;
            _characterInput.Fishing.Pull.canceled -= CancelPull;
            _characterInput.Fishing.SmashBarrier.performed -= SmashBarrier;
        }

        private void Pull(InputAction.CallbackContext obj)
        {
            _fishingGame.SetPullingFlag(true);
        }
        
        private void CancelPull(InputAction.CallbackContext obj)
        {
            _fishingGame.SetPullingFlag(false);
        }

        private void SmashBarrier(InputAction.CallbackContext obj)
        {
            _fishingGame.SmashBarrier();
        }

        public void Tick()
        {
            _view.SetPlayerBarValue(_fishingGame.PlayerValue);
            _view.SetFailBarValue(_fishingGame.FailValue);
        }
    }
}