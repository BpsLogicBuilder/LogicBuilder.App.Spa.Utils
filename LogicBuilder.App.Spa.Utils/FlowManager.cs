using LogicBuilder.App.Spa.Business.Cache;
using LogicBuilder.App.Spa.Business.Cache.Interfaces;
using LogicBuilder.App.Spa.Business.Requests;
using LogicBuilder.App.Spa.Business.ScreenSettings;
using LogicBuilder.App.Spa.Business.ScreenSettings.Views;
using LogicBuilder.App.Spa.Utils.Dialogs;
using LogicBuilder.App.Spa.Utils.Factories;
using LogicBuilder.App.Spa.Utils.Interfaces;
using LogicBuilder.RulesDirector;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace LogicBuilder.App.Spa.Utils
{
    public class FlowManager : IFlowManager
    {
        private readonly ILogger<FlowManager> _logger;

        public FlowManager(
            ILogger<FlowManager> logger,
            IFlowDataCache flowDataCache,
            IFlowFactory flowFactory,
            Progress progress,
            IRulesCache rulesCache,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            FlowDataCache = flowDataCache;
            Progress = progress;
            RulesCache = rulesCache;
            ServiceProvider = serviceProvider;
            Director = flowFactory.GetDirector(this);
            FlowActivity = flowFactory.GetFlowActivity(this);
        }

        public DirectorBase Director { get; }

        public IFlowDataCache FlowDataCache { get; }

        public Progress Progress { get; }

        public IFlowActivity FlowActivity { get; }

        public IRulesCache RulesCache { get; }

        public IServiceProvider ServiceProvider { get; }

        private FlowSettings FlowSettings
           => new
           (
               GetPersistentFlowSetting(),
               ((Director)this.Director).FlowState,
               FlowDataCache.NavigationBar,
               FlowDataCache.ScreenSettings ?? throw new ArgumentException($"{nameof(FlowDataCache.ScreenSettings)}: {{60B6AFD1-2247-4775-BE99-F3F650A15B0F}}")
           );

        public void FlowComplete()
        {
            if (_logger.IsEnabled(LogLevel.Information))
                _logger.LogInformation("FlowComplete {Progress}", JsonSerializer.Serialize(this.Progress));
            FlowDataCache.ScreenSettings = new ScreenSettings<object>(null!, [], ViewType.FlowComplete);
        }

        public FlowSettings NavStart(NavBarRequest navBarRequest)
        {
            try
            {
                foreach (KeyValuePair<string, object> kvp in navBarRequest.PersistentFlowItems)
                    FlowDataCache.Items[kvp.Key] = kvp.Value;

                FlowDataCache.PersistentKeys = [.. navBarRequest.PersistentFlowItems.Keys];

                FlowDataCache.RequestedFlowStage = new RequestedFlowStage
                {
                    InitialModule = navBarRequest.InitialModuleName ?? throw new ArgumentException($"{nameof(navBarRequest.InitialModuleName)}: {{91027670-3D9A-444C-A1C9-03B19BC53C19}}"),
                    TargetModule = navBarRequest.TargetModule
                };

                this.Director.StartInitialFlow(FlowDataCache.RequestedFlowStage.InitialModule);

                return this.FlowSettings;
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                    _logger.LogInformation("NavStart {Progress}", JsonSerializer.Serialize(this.Progress));
                _logger.LogError(ex, "NavStart Exception: {Message}", ex.Message);
                return this.GetFlowSettings(ex);
            }
        }

        public FlowSettings Next(RequestBase request)
        {
            try
            {
                IDialogHandler handler = BaseDialogHandler.Create(request);

                handler.Complete(this, request);
                this.Director.ExecuteRulesEngine();

                return this.FlowSettings;
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                    _logger.LogInformation("Progress Next {Progress}", JsonSerializer.Serialize(this.Progress));
                _logger.LogError(ex, "Next Exception: {Message}", ex.Message);
                return this.GetFlowSettings(ex);
            }
        }

        public void RunFlow(string flowName)
        {
            try
            {
                this.Director.StartInitialFlow(flowName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: {Message}", ex.Message);
                throw new InvalidOperationException($"{flowName} falied.", ex);
            }
        }

        public void SetCurrentBusinessBackupData()
        {
        }

        public FlowSettings Start(string module, int stage)
        {
            try
            {
                FlowDataCache.RequestedFlowStage = new RequestedFlowStage { InitialModule = module, TargetModule = stage };
                this.Director.StartInitialFlow(module);
                return this.FlowSettings;
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                    _logger.LogInformation("Progress Start{Progress}", JsonSerializer.Serialize(this.Progress));
                _logger.LogError(ex, "Exception: {Message}", ex.Message);
                return this.GetFlowSettings(ex);
            }
        }

        public void Terminate()
        {
            throw new NotImplementedException();
        }

        private FlowSettings GetFlowSettings(Exception ex)
            => new
            (
                GetPersistentFlowSetting(),
                ((Director)this.Director).FlowState,
                FlowDataCache.NavigationBar,
                new ScreenSettings<ExceptionView>
                (
                    new ExceptionView { Message = ex.Message },
                    [],
                    ViewType.Exception
                )
            );

        private Dictionary<string, object> GetPersistentFlowSetting()
            => FlowDataCache.PersistentKeys.Aggregate
            (
                new Dictionary<string, object>(), 
                (dictionary, key) =>
                {
                    if (FlowDataCache.Items.TryGetValue(key, out object? value))
                        dictionary.Add(key, value);
                    return dictionary;
                }
            );
    }
}
