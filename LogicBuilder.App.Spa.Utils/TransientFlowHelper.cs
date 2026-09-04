using AutoMapper;
using LogicBuilder.App.Spa.Business.Requests.TransientFlows;
using LogicBuilder.App.Spa.Business.Responses.TransientFlows;
using LogicBuilder.App.Spa.Utils.Interfaces;
using LogicBuilder.Domain;
using LogicBuilder.Expressions.Utils.ExpressionDescriptors;
using LogicBuilder.Forms.Parameters.Expressions;
using System;

namespace LogicBuilder.App.Spa.Utils
{
    public class TransientFlowHelper(IFlowManager flowManager, IMapper mapper) : ITransientFlowHelper
    {
        private readonly IFlowManager _flowManager = flowManager;
        private readonly IMapper _mapper = mapper;

        public BaseFlowResponse RunSelectorFlow(SelectorFlowRequest selectorFlowRequest)
        {
            BaseModel entity = selectorFlowRequest.Entity ?? throw new ArgumentException($"{nameof(selectorFlowRequest)}: {{9B9FC188-D1EC-4E1A-B71C-26A4E2413ABB}}");
            string flowName = selectorFlowRequest.ReloadItemsFlowName ?? throw new ArgumentException($"{nameof(selectorFlowRequest)}: {{56C7E26E-C4FD-46B9-99E3-BF2B544A8D84}}");
            this._flowManager.FlowDataCache.Items[entity.GetType().FullName ?? throw new ArgumentException($"{nameof(selectorFlowRequest)}: {{27C817BC-1EC1-442F-9483-30AD6B2BCD7E}}")] = entity;

            this._flowManager.RunFlow(flowName);

            if (!_flowManager.FlowDataCache.Items.TryGetValue(typeof(SelectorLambdaOperatorParameters).FullName!, out object? selector))
                throw new InvalidOperationException("Selector is null.");

            return new SelectorFlowResponse
            {
                Success = true,
                Selector = _mapper.Map<SelectorLambdaDescriptor>
                (
                    selector
                )
            };
        }
    }
}
