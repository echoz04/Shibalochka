using System.Collections.Generic;
using Sources.UI.Services.Fishing;
using VContainer;

namespace Sources
{
    public class FishingScreen:UIScreenBase
    {
        [Inject]
        private void Construct(
            FishingService fishingService
        )
        {
            UIServices = new List<IUIService>
            {
                fishingService
            };
        }
    }
}