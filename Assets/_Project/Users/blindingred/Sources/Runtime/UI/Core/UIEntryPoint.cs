using Sources.Signals;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Sources
{
    public class UIEntryPoint: IInitializable
    {
        private ISignalBus _signalBus;
        
        [Inject]
        private void Construct(ISignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            Debug.Log("UIEntryPoint::Initialize()");
            _signalBus.Fire(new ScreenChangeSignal(typeof(MenuScreen)));
        }
    }
}