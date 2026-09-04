using LogicBuilder.App.Spa.Business.ScreenSettings.Navigation;
using LogicBuilder.Attributes;

namespace LogicBuilder.App.Spa.Utils.Interfaces
{
    public interface ICustomActions
    {
        [AlsoKnownAs("SetupNavigationMenu")]
        void UpdateNavigationBar(NavigationBar navBar);
    }
}
