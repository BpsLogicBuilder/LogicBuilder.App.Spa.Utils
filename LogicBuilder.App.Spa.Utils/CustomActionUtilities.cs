using LogicBuilder.App.Spa.Business.ScreenSettings.Navigation;
using LogicBuilder.App.Spa.Utils.Interfaces;
using LogicBuilder.Attributes;

namespace LogicBuilder.App.Spa.Utils
{
    public static class CustomActionUtilities
    {
        [AlsoKnownAs("SetupNavigationMenu")]
        public static void UpdateNavigationBar(ICustomActions customActions, NavigationBar navBar)
        {
            customActions.UpdateNavigationBar(navBar);
        }
    }
}
