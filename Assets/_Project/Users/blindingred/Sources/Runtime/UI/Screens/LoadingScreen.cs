using System.Collections.Generic;
using Sources.UI.Services;
using Sources.UI.Services.Loading;
using VContainer;

namespace Sources
{
    public class LoadingScreen:UIScreenBase
    {
        [Inject]
        private void Construct(
            LoadingService loadingService
            )
        {
            UIServices = new List<IUIService>
            {
               loadingService
            };
        }
    }
}