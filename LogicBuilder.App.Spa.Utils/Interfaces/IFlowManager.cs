using LogicBuilder.App.Spa.Business.Cache.Interfaces;
using LogicBuilder.App.Spa.Business.Requests;
using LogicBuilder.App.Spa.Business.ScreenSettings;
using LogicBuilder.RulesDirector;
using System;

namespace LogicBuilder.App.Spa.Utils.Interfaces
{
    public interface IFlowManager
    {
        DirectorBase Director { get; }
        IFlowDataCache FlowDataCache { get; }
        Progress Progress { get; }
        IFlowActivity FlowActivity { get; }
        IRulesCache RulesCache { get; }
        IServiceProvider ServiceProvider { get; }

        FlowSettings Start(string module, int stage);
        FlowSettings Next(RequestBase request);
        FlowSettings NavStart(NavBarRequest navBarRequest);
        void RunFlow(string flowName);
        void FlowComplete();
        void Terminate();
        void SetCurrentBusinessBackupData();
    }
}
