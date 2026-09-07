using AutoMapper;
using Enrollment.Domain.Entities;
using Enrollment.Spa.Flow.Cache;
using LogicBuilder.App.Spa.AutoMapperProfiles;
using LogicBuilder.App.Spa.Business.Requests;
using LogicBuilder.App.Spa.Business.ScreenSettings.Views;
using LogicBuilder.App.Spa.Forms.Configuration.Common;
using LogicBuilder.App.Spa.Utils.Interfaces;
using LogicBuilder.EntityFrameworkCore.Mapping;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace LogicBuilder.App.Spa.Utils.Tests
{
    public class NavStartTest
    {
        static NavStartTest()
        {
            InitializeMapperConfiguration();
        }

        public NavStartTest()
        {
            Initialize();
        }

        #region Fields
        private static MapperConfiguration MapperConfiguration;
        private IServiceProvider serviceProvider;
        private const string initialFlow = "initial";
        #endregion Fields

        [Fact]
        public void FlowWithUnrecognizedTarget_EndsWithViewTypeComplete()
        {
            //arrange
            IFlowManager flowManager = serviceProvider!.GetRequiredService<IFlowManager>();

            //act
            var result = flowManager.NavStart(new NavBarRequest { InitialModuleName = initialFlow, TargetModule = 10 });

            //assert
            Assert.Equal(ViewType.FlowComplete, result.ScreenSettings.ViewType);
        }

        [Fact]
        public void FlowWithHomeTarget_StopsAtHomeScreen()
        {
            //arrange
            IFlowManager flowManager = serviceProvider!.GetRequiredService<IFlowManager>();

            //act
            var result = flowManager.NavStart(new NavBarRequest { InitialModuleName = initialFlow, TargetModule = TargetModules.Home });

            //assert
            var screenSettings = Assert.IsType<ScreenSettings<HtmlPageSettingsDescriptor>>(result.ScreenSettings);
            Assert.Equal(ViewType.Html, result.ScreenSettings.ViewType);
            Assert.Equal("Charlotte School of Science", screenSettings.Settings.ContentTemplate?.Title);
        }

        [Fact]
        public void FlowWithAcademicTarget_StopsAtAcademicScreen()
        {
            //arrange
            IFlowManager flowManager = serviceProvider!.GetRequiredService<IFlowManager>();
            flowManager.FlowDataCache.Items["UserId"] = 1;

            //act
            var result = flowManager.NavStart(new NavBarRequest { InitialModuleName = initialFlow, TargetModule = TargetModules.Academic });

            //assert
            var screenSettings = Assert.IsType<ScreenSettings<EditFormSettingsDescriptor>>(result.ScreenSettings);
            Assert.Equal(ViewType.Edit, result.ScreenSettings.ViewType);
            Assert.Equal("Academic", screenSettings.Settings.Title);
        }

        [Fact]
        public void PersistentFlowItemsFronNavBarRequest_AreTransferredToTheFlowDataCache_andFlowSettings()
        {
            //arrange
            IFlowManager flowManager = serviceProvider!.GetRequiredService<IFlowManager>();
            var persistentFlowItems = new Dictionary<string, object> { ["UserId"] = 1, ["UserName"] = "Smith101", ["UserRating"] = 9.5 };

            //act
            var result = flowManager.NavStart
            (
                new NavBarRequest 
                { 
                    PersistentFlowItems = persistentFlowItems,
                    InitialModuleName = initialFlow, 
                    TargetModule = TargetModules.Academic 
                }
            );

            //assert
            Assert.Equal(1, (int)flowManager.FlowDataCache.Items["UserId"]);
            Assert.Equal("Smith101", (string)flowManager.FlowDataCache.Items["UserName"]);
            Assert.Equal(9.5, (double)flowManager.FlowDataCache.Items["UserRating"]);

            Assert.Equal(1, (int)result.PersistentFlowItems["UserId"]);
            Assert.False(result.PersistentFlowItems.ContainsKey("UserName"));
            Assert.False(result.PersistentFlowItems.ContainsKey("UserRating"));
            var screenSettings = Assert.IsType<ScreenSettings<EditFormSettingsDescriptor>>(result.ScreenSettings);
            Assert.Equal(ViewType.Edit, result.ScreenSettings.ViewType);
            Assert.Equal("Academic", screenSettings.Settings.Title);
        }

        [Fact]
        public void FlowWithAdminTarget_CanVisitCreateScreen()
        {
            //arrange
            IFlowManager flowManager = serviceProvider!.GetRequiredService<IFlowManager>();
            flowManager.FlowDataCache.Items["UserId"] = 1;

            //act
            var firstResult = flowManager.NavStart(new NavBarRequest { InitialModuleName = initialFlow, TargetModule = TargetModules.Admin });
            var firstScreenSettings = (ScreenSettings<GridSettingsDescriptor>)firstResult.ScreenSettings;
            var selectedButton = firstScreenSettings.CommandButtons!.First(b => b.ShortString == "admin_CREATE");
            var secondResult = flowManager.Next
            (
                new GridRequest
                {
                    CommandButtonRequest = new CommandButtonRequest { NewSelection = selectedButton.ShortString },
                    FlowState = ((Director)flowManager.Director).FlowState,
                    ViewType = ViewType.Grid
                }
            );

            //assert
            Assert.Equal(1, (int)secondResult.PersistentFlowItems["UserId"]);
            Assert.False(secondResult.PersistentFlowItems.ContainsKey("UserName"));
            Assert.False(secondResult.PersistentFlowItems.ContainsKey("UserRating"));
            var secondScreenSettings = Assert.IsType<ScreenSettings<EditFormSettingsDescriptor>>(secondResult.ScreenSettings);
            Assert.Equal(ViewType.Create, secondScreenSettings.ViewType);
            Assert.Equal("Title", secondScreenSettings.Settings.Title);
        }

        [Fact]
        public void PersistentFlowItemsFromRequestBase_AreReturnedInFlowSettings()
        {
            //arrange
            IFlowManager flowManager = serviceProvider!.GetRequiredService<IFlowManager>();
            flowManager.FlowDataCache.Items["UserId"] = 1;

            //act
            var firstResult = flowManager.NavStart(new NavBarRequest { InitialModuleName = initialFlow, TargetModule = TargetModules.Admin });
            var firstScreenSettings = (ScreenSettings<GridSettingsDescriptor>)firstResult.ScreenSettings;
            var selectedButton = firstScreenSettings.CommandButtons!.First(b => b.ShortString == "admin_CREATE");
            var secondResult = flowManager.Next
            (
                new GridRequest
                {
                    PersistentFlowItems = new Dictionary<string, object> { ["UserId"] = 1, ["UserName"] = "Smith101", ["UserRating"] = 9.5 },
                    CommandButtonRequest = new CommandButtonRequest { NewSelection = selectedButton.ShortString },
                    FlowState = ((Director)flowManager.Director).FlowState,
                    ViewType = ViewType.Grid
                }
            );

            //assert
            var secondScreenSettings = Assert.IsType<ScreenSettings<EditFormSettingsDescriptor>>(secondResult.ScreenSettings);
            Assert.Equal(ViewType.Create, secondScreenSettings.ViewType);
            Assert.Equal("Title", secondScreenSettings.Settings.Title);
        }

        [Fact]
        public void FlowWithAdminTarget_CanVisitEditScreen()
        {
            //arrange
            IFlowManager flowManager = serviceProvider!.GetRequiredService<IFlowManager>();
            flowManager.FlowDataCache.Items["UserId"] = 1;

            //act
            var firstResult = flowManager.NavStart(new NavBarRequest { InitialModuleName = initialFlow, TargetModule = TargetModules.Admin });
            var firstScreenSettings = (ScreenSettings<GridSettingsDescriptor>)firstResult.ScreenSettings;
            var selectedButton = firstScreenSettings.CommandButtons!.First(b => b.ShortString == "admin_EDIT");
            var secondResult = flowManager.Next
            (
                new GridRequest
                {
                    CommandButtonRequest = new CommandButtonRequest { NewSelection = selectedButton.ShortString },
                    Entity = new UserModel { UserId = 1 },
                    FlowState = ((Director)flowManager.Director).FlowState,
                    ViewType = ViewType.Grid
                }
            );

            //assert
            var secondScreenSettings = Assert.IsType<ScreenSettings<EditFormSettingsDescriptor>>(secondResult.ScreenSettings);
            Assert.Equal(ViewType.Edit, secondScreenSettings.ViewType);
            Assert.Equal("Title", secondScreenSettings.Settings.Title);
        }

        [Fact]
        public void FlowWitAdminTarget_CanVisitDetailScreen()
        {
            //arrange
            IFlowManager flowManager = serviceProvider!.GetRequiredService<IFlowManager>();
            flowManager.FlowDataCache.Items["UserId"] = 1;

            //act
            var firstResult = flowManager.NavStart(new NavBarRequest { InitialModuleName = initialFlow, TargetModule = TargetModules.Admin });
            var firstScreenSettings = (ScreenSettings<GridSettingsDescriptor>)firstResult.ScreenSettings;
            var selectedButton = firstScreenSettings.CommandButtons!.First(b => b.ShortString == "admin_DETAIL");
            var secondResult = flowManager.Next
            (
                new GridRequest
                {
                    CommandButtonRequest = new CommandButtonRequest { NewSelection = selectedButton.ShortString },
                    Entity = new UserModel { UserId = 1 },
                    FlowState = ((Director)flowManager.Director).FlowState,
                    ViewType = ViewType.Grid
                }
            );

            //assert
            var secondScreenSettings = Assert.IsType<ScreenSettings<DetailFormSettingsDescriptor>>(secondResult.ScreenSettings);
            Assert.Equal(ViewType.Detail, secondScreenSettings.ViewType);
            Assert.Equal("Student", secondScreenSettings.Settings.Title);
        }

        [Fact]
        public void FlowWithAdminTarget_CanVisitDeleteScreen()
        {
            //arrange
            IFlowManager flowManager = serviceProvider!.GetRequiredService<IFlowManager>();
            flowManager.FlowDataCache.Items["UserId"] = 1;

            //act
            var firstResult = flowManager.NavStart(new NavBarRequest { InitialModuleName = initialFlow, TargetModule = TargetModules.Admin });
            var firstScreenSettings = (ScreenSettings<GridSettingsDescriptor>)firstResult.ScreenSettings;
            var selectedButton = firstScreenSettings.CommandButtons!.First(b => b.ShortString == "admin_DELETE");
            var secondResult = flowManager.Next
            (
                new GridRequest
                {
                    CommandButtonRequest = new CommandButtonRequest { NewSelection = selectedButton.ShortString },
                    Entity = new UserModel { UserId = 1 },
                    FlowState = ((Director)flowManager.Director).FlowState,
                    ViewType = ViewType.Grid
                }
            );

            //assert
            var secondScreenSettings = Assert.IsType<ScreenSettings<DetailFormSettingsDescriptor>>(secondResult.ScreenSettings);
            Assert.Equal(ViewType.Delete, secondScreenSettings.ViewType);
            Assert.Equal("Student", secondScreenSettings.Settings.Title);
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
        private void Initialize()
        {
            serviceProvider = new ServiceCollection()
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
