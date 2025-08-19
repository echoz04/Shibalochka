using System.Collections.Generic;
using VContainer;

namespace Sources
{
    public class UIScreenManager
    {
        private IUIScreen _currentScreen;
        
        private Dictionary<string, IUIScreen> _screens;

        [Inject]
        public UIScreenManager(IReadOnlyList<IUIScreen> uiScreens)
        {
            _screens = new Dictionary<string, IUIScreen>();
            foreach (var uiScreen in uiScreens)
            {
                var typeName = uiScreen.GetType().Name;
                _screens[typeName] = uiScreen;
            }		
        }
        
        public void OpenUIScreen<T>() where T : IUIScreen
        {
            _currentScreen?.Close();
            var screen = _screens[typeof(T).Name];
            _currentScreen = screen;
            _currentScreen?.Open();
        }
    }
}