using AutoMapper;
using LogicBuilder.App.Spa.AutoMapperProfiles;
using LogicBuilder.App.Spa.Business.ScreenSettings.Views;
using LogicBuilder.App.Spa.Utils.Interfaces;
using LogicBuilder.EntityFrameworkCore.Mapping;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace LogicBuilder.App.Spa.Utils.Tests
{
    public class TerminateTest
    {
        static TerminateTest()
        {
            InitializeMapperConfiguration();
            Initialize();
        }
        #region Fields
        private static MapperConfiguration MapperConfiguration;
        private static IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public async Task CallTerminateFlow()
        {
            //arrange
            IFlowManager flowManager = serviceProvider!.GetRequiredService<IFlowManager>();

            //act
            var result = flowManager.Start("callterminateflow", 0);

            //assert
            Assert.Equal("Enrollment with MyUniqueValue appended.", flowManager.FlowDataCache.NavigationBar.BrandText);
            Assert.Equal(ViewType.Exception, result.ScreenSettings.ViewType);
        }

        #region Helpers
        [MemberNotNull(nameof(MapperConfiguration))]
        private static void InitializeMapperConfiguration()
        {
            MapperConfiguration ??= new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<BaseClassMappings>();
                cfg.AddProfile<ConnectorProfile>();
                cfg.AddProfile<ParameterToDescriptorProfile>();
                cfg.AddProfile<ExpressionParameterToDescriptorMappingProfile>();
                cfg.AddProfile<ExpansionParameterToDescriptorMappingProfile>();
            }, new NullLoggerFactory());
            MapperConfiguration.AssertConfigurationIsValid();
        }

        [MemberNotNull(nameof(serviceProvider))]
        private static void Initialize()
        {
            serviceProvider ??= new ServiceCollection()
                .AddSingleton<AutoMapper.IConfigurationProvider>
                (
                    MapperConfiguration
                )
                .AddTransient<IMapper>(sp => new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService))
                .AddLogging()
                .AddSpaFlowServices()
                .BuildServiceProvider();
        }
        #endregion Helpers
    }
}
