using System.Collections.Generic;
using Sources.UI.Services;
using VContainer;

namespace Sources
{
    public class MenuScreen : UIScreenBase
    {
        [Inject]
        private void Construct(MenuService menuService)
        {
            UIServices = new List<IUIService>
            {
                menuService
            };
        }
     }
}
