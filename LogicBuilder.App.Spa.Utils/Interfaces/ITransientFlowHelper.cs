using LogicBuilder.App.Spa.Business.Requests.TransientFlows;
using LogicBuilder.App.Spa.Business.Responses.TransientFlows;

namespace LogicBuilder.App.Spa.Utils.Interfaces
{
    public interface ITransientFlowHelper
    {
        BaseFlowResponse RunSelectorFlow(SelectorFlowRequest selectorFlowRequest);
    }
}
