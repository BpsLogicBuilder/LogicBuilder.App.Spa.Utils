using LogicBuilder.App.Spa.Utils.Interfaces;
using LogicBuilder.RulesDirector;

namespace LogicBuilder.App.Spa.Utils.Factories
{
    public interface IFlowFactory
    {
        DirectorBase GetDirector(IFlowManager flowManager);
        IFlowActivity GetFlowActivity(IFlowManager flowManager);
    }
}
