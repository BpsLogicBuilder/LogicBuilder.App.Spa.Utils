using LogicBuilder.App.Spa.Business.Cache;
using LogicBuilder.App.Spa.Business.Cache.Interfaces;
using LogicBuilder.App.Spa.Utils;
using LogicBuilder.App.Spa.Utils.Interfaces;
using LogicBuilder.RulesDirector;

#pragma warning disable IDE0130 //Microsoft recommended namespace for service registrations
namespace Microsoft.Extensions.DependencyInjection
#pragma warning restore IDE0130
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class SpaUtilsServiceRegistrations
    {
        public static IServiceCollection AddSpaUtilsServices(this IServiceCollection services)
        {
            return services
                .AddAppUtilsServices()
                .AddHttpClient()
                .AddTransient<ICustomActions, CustomActions>()
                .AddTransient<ICustomDialogs, CustomDialogs>()
                .AddTransient<IFlowManager, FlowManager>()
                .AddTransient<ITransientFlowHelper, TransientFlowHelper>()
                .AddScoped<IFlowDataCache, FlowDataCache>()
                .AddScoped<Progress>();
        }
    }
}
