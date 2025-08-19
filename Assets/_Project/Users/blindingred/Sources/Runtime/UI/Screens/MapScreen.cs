using Sources.UI.Services;
using VContainer;

namespace Sources
{
    public class MapScreen : UIScreenBase
    {
        [Inject]
        public MapScreen(MapService mapService)
        {

        }
    }
}