using System;
using Sources.Signals;
using UnityEngine;
using Unity.Cinemachine;
using VContainer;
using VContainer.Unity;

namespace Sources.Runtime.Gameplay.Camera
{
    public class CameraRotator : MonoBehaviour, IInitializable, IDisposable
    {
        [SerializeField] private CinemachineInputAxisController _cinemachineInputAxisController;
        
        private ISignalBus _signalBus;

        [Inject]
        private void Construct(ISignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        void IInitializable.Initialize()
        {
            _signalBus.Subscribe<CameraRotatorStateSignal>(SetState, true);
        }
        
        void IDisposable.Dispose()
        {
            _signalBus.Unsubscribe<CameraRotatorStateSignal>(SetState);
        }

        private void SetState(CameraRotatorStateSignal signal)
        {
            SetState(signal.State);
        }

        public void SetState(bool state)
        {
            _cinemachineInputAxisController.enabled = state;
        }
    }
}
