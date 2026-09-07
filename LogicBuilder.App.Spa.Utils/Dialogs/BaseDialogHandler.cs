using LogicBuilder.App.Spa.Business.Requests;
using LogicBuilder.App.Spa.Business.ScreenSettings.Views;
using LogicBuilder.App.Spa.Utils.Interfaces;
using System;
using System.Collections.Generic;

namespace LogicBuilder.App.Spa.Utils.Dialogs
{
    abstract public class BaseDialogHandler : IDialogHandler
    {
        public virtual void Complete(IFlowManager flowManager, RequestBase request)
        {
            foreach (KeyValuePair<string, object> kvp in request.PersistentFlowItems)
                flowManager.FlowDataCache.Items[kvp.Key] = kvp.Value;

            ((Director)flowManager.Director).FlowState = request.FlowState ?? throw new ArgumentException($"{nameof(request.FlowState)}: {{3C808121-4B83-4633-A469-16AE4CF059C1}}");
            flowManager.Director.SetSelection(request.CommandButtonRequest?.NewSelection ?? throw new ArgumentException($"{nameof(request.CommandButtonRequest)}: {{F5B2A906-2E70-4A30-A114-12C9B544EB3F}}"));
        }

        public static IDialogHandler Create(RequestBase response)
        {
            return response.ViewType switch
            {
                ViewType.Grid or ViewType.Detail => (BaseDialogHandler)
                (
                    Activator.CreateInstance
                    (
                        typeof(BaseDialogHandler).Assembly.GetType
                        (
                            $"LogicBuilder.App.Spa.Utils.Dialogs.{Enum.GetName(typeof(ViewType), response.ViewType)}DialogHandler"
                        ) ?? throw new ArgumentException($"{nameof(response.ViewType)}: {{AEAC5486-CADF-43BB-89D4-7A7999B1148D}}")
                    ) ?? throw new ArgumentException($"{nameof(response.ViewType)}: {{27A956EB-4BD3-4ABC-B437-038835844091}}")
                ),
                _ => new DefaultDialogHandler(),
            };
        }
    }
}
