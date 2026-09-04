using LogicBuilder.App.Spa.Business.ScreenSettings;
using LogicBuilder.App.Spa.Utils.Interfaces;
using LogicBuilder.RulesDirector;
using System.Linq;

namespace LogicBuilder.App.Spa.Utils
{
    public class Director(IFlowManager flowManager) : AppDirectorBase
    {
        private readonly IFlowManager _flowManager = flowManager;

        protected override IRulesCache RulesCache => _flowManager.RulesCache;
        protected override IFlowActivity FlowActivity => _flowManager.FlowActivity;
        protected override Progress Progress => _flowManager.Progress;

        public FlowState FlowState
        {
            get
            {
                return new FlowState
                {
                    Driver = this._driver,
                    Selection = this._selection,
                    CallingModuleDriverStack = [.. this._callingModuleDriverStack.OfType<string>()],
                    CallingModuleStack = [.. this._callingModuleStack.OfType<string>()],
                    ModuleBeginName = this._moduleBeginName,
                    ModuleEndName = this._moduleEndName
                };
            }
            set
            {
                this._driver = value.Driver;
                this._selection = value.Selection;
                this._callingModuleDriverStack = new System.Collections.Stack(value.CallingModuleDriverStack.Reverse<string>().ToList());
                this._callingModuleStack = new System.Collections.Stack(value.CallingModuleStack.Reverse<string>().ToList());
                this._moduleBeginName = value.ModuleBeginName;
                this._moduleEndName = value.ModuleEndName;
            }
        }

        public override void SetCurrentBusinessBackupData() => _flowManager.SetCurrentBusinessBackupData();
    }
}
