using System.Collections.Generic;
using Sources.UI.Services;
using VContainer;

namespace Sources
{
    public class MapScreen : UIScreenBase
    {
        [Inject]
        public void Construct(MapService mapService)
        {
            UIServices = new List<IUIService>
            {
                mapService
            };
        }
    }
}