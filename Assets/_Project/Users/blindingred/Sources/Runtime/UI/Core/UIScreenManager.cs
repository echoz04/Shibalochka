using System;
using System.Collections.Generic;
using Sources.Signals;
using VContainer;
using VContainer.Unity;

namespace Sources
{
    public class UIScreenManager:IInitializable, IDisposable
    {
        private IUIScreen _currentScreen;
        
        private Dictionary<string, IUIScreen> _screens;
        
        private ISignalBus _signalBus;

        [Inject]
        public UIScreenManager(IReadOnlyList<IUIScreen> uiScreens, ISignalBus signalBus)
        {
            _screens = new Dictionary<string, IUIScreen>();
            foreach (var uiScreen in uiScreens)
            {
                var typeName = uiScreen.GetType().Name;
                _screens[typeName] = uiScreen;
            }		
            
            _signalBus = signalBus;
        }

        public void Initialize()
        {
           _signalBus.Subscribe<ScreenChangeSignal>(OpenUIScreen, false);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<ScreenChangeSignal>(OpenUIScreen);
        }

        public void OpenUIScreen(ScreenChangeSignal signal)
        {
            if (!typeof(IUIScreen).IsAssignableFrom(signal.ScreenType))
                throw new ArgumentException($"{signal.ScreenType.Name} is not an IUIScreen");
            
            var screen = _screens[signal.ScreenType.Name];
            
            OpenUIScreen(screen);
        }

        private void OpenUIScreen(IUIScreen screen)
        {
            _currentScreen?.Close();
            _currentScreen = screen;
            _currentScreen?.Open();
        }
    }
}