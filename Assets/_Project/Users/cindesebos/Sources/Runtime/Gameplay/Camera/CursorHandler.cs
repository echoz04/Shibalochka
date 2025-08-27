using System;
using Sources.Signals;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Sources.Runtime.Gameplay.Camera
{
    public class CursorHandler: IInitializable, IDisposable
    {
        private ISignalBus _signalBus;
        public bool IsVisible => Cursor.visible;

        [Inject]
        public CursorHandler(ISignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        void IInitializable.Initialize()
        {
            _signalBus.Subscribe<SwitchCursorStateSignal>(OnSwitchState, true);
        }

        void IDisposable.Dispose()
        {
            _signalBus.Unsubscribe<SwitchCursorStateSignal>(OnSwitchState);
        }

        private void OnSwitchState(SwitchCursorStateSignal signal)
        {
            SetState(signal.State);
        }
         
        public void SetState(bool isVisible)
        {
            switch (isVisible)
            {
                case true:
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    break;
                case false:
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    break;
            }
        }
    }
}
