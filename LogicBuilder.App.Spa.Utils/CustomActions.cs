using LogicBuilder.App.Spa.Business.Cache.Interfaces;
using LogicBuilder.App.Spa.Business.ScreenSettings.Navigation;
using LogicBuilder.App.Spa.Utils.Interfaces;

namespace LogicBuilder.App.Spa.Utils
{
    public class CustomActions(IFlowDataCache flowDataCache) : ICustomActions
    {
        private readonly IFlowDataCache flowDataCache = flowDataCache;

        public void UpdateNavigationBar(NavigationBar navBar)
        {
            this.flowDataCache.NavigationBar = navBar;
        }
    }
}
