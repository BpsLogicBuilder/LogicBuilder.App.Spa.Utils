using LogicBuilder.App.Spa.Business.Requests;
using LogicBuilder.App.Spa.Utils.Interfaces;

namespace LogicBuilder.App.Spa.Utils.Dialogs
{
    public interface IDialogHandler
    {
        void Complete(IFlowManager flowManager, RequestBase request);
    }
}
