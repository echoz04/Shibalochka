using VContainer;
using VContainer.Unity;

namespace Sources
{
    public class UIEntryPoint: IInitializable
    {
        private UIScreenManager _uiScreenManager;
        
        [Inject]
        private void Construct(UIScreenManager screenManager)
        {
           _uiScreenManager = screenManager;
        }

        public void Initialize()
        {
            _uiScreenManager.OpenUIScreen<MenuScreen>();
        }
    }
}