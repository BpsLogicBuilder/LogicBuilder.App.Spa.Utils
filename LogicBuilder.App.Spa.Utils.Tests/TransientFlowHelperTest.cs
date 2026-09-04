using Enrollment.Domain.Entities;
using LogicBuilder.App.Spa.Business.Requests.TransientFlows;
using LogicBuilder.App.Spa.Business.Responses.TransientFlows;
using LogicBuilder.App.Spa.Utils.Interfaces;
using LogicBuilder.Expressions.Utils.ExpressionDescriptors;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;

namespace LogicBuilder.App.Spa.Utils.Tests
{
    public class TransientFlowHelperTest
    {
        public TransientFlowHelperTest()
        {
            Initialize();
        }
        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void TransientFlowH_CreatesSelector_WhenProgramIsValid()
        {
            //arrange
            ITransientFlowHelper flowHelper = serviceProvider!.GetRequiredService<ITransientFlowHelper>();

            //act
            var result = flowHelper.RunSelectorFlow(new SelectorFlowRequest
            {
                Entity = new AdmissionsModel { ProgramType = "degreePrograms" },
                ReloadItemsFlowName = "admissionsprogramselector"
            });

            //assert
            Assert.NotNull(result);
            var response = Assert.IsType<SelectorFlowResponse>(result);
            Assert.NotNull(response.Selector);
            Assert.IsType<SelectorLambdaDescriptor>(response.Selector);
        }

        [Fact]
        public void TransientFlowH_ThrowsInvalidOperationException_ForInValidProgramType()
        {
            //arrange
            ITransientFlowHelper flowHelper = serviceProvider!.GetRequiredService<ITransientFlowHelper>();

            //act & assert
            var exception = Assert.Throws<InvalidOperationException>(() => flowHelper.RunSelectorFlow(new SelectorFlowRequest
            {
                Entity = new AdmissionsModel { ProgramType = "invalidProgramType" },
                ReloadItemsFlowName = "admissionsprogramselector"
            }));
            Assert.Equal("Selector is null.", exception.Message);
        }

        #region Helpers
        [MemberNotNull(nameof(serviceProvider))]
        private void Initialize()
        {
            serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSpaFlowServices()
                .AddAutoMapperServices()
                .BuildServiceProvider();
        }
        #endregion Helpers
    }
}
