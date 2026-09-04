using AutoMapper;
using LogicBuilder.App.Spa.Business.Cache.Interfaces;
using LogicBuilder.App.Spa.Business.ScreenSettings.Views;
using LogicBuilder.App.Spa.Forms.Configuration;
using LogicBuilder.App.Spa.Forms.Configuration.Common;
using LogicBuilder.App.Spa.Forms.Parameters.Common;
using LogicBuilder.App.Spa.Utils.Interfaces;
using LogicBuilder.Forms.Parameters;
using System;
using System.Collections.Generic;

namespace LogicBuilder.App.Spa.Utils
{
    public class CustomDialogs(IMapper mapper, IFlowDataCache flowDataCache) : ICustomDialogs
    {

        #region Fields
        private readonly IFlowDataCache flowDataCache = flowDataCache;
        private readonly IMapper mapper = mapper;
        #endregion Fields

        public void DisplayGrid(GridSettingsParameters setting, ICollection<ConnectorParameters> buttons)
            => this.flowDataCache.ScreenSettings = new ScreenSettings<GridSettingsDescriptor>
            (
                mapper.Map<GridSettingsDescriptor>(setting),
                mapper.Map<IEnumerable<ConnectorParameters>, IEnumerable<CommandButtonDescriptor>>(buttons),
                ViewType.Grid
            );

        public void DisplayEditForm(EditFormSettingsParameters setting, ViewType viewType, ICollection<ConnectorParameters> buttons)
        {
            this.flowDataCache.ScreenSettings = viewType switch
            {
                ViewType.Edit or ViewType.Create => new ScreenSettings<EditFormSettingsDescriptor>
                (
                    mapper.Map<EditFormSettingsDescriptor>(setting),
                    mapper.Map<IEnumerable<ConnectorParameters>, IEnumerable<CommandButtonDescriptor>>(buttons),
                    viewType
                ),
                _ => throw new ArgumentException($"{nameof(viewType)}: {{B3B3788E-738A-4E6B-8B95-C6DDC2C4AA5D}}"),
            };
        }

        public void DisplayDetailForm(DetailFormSettingsParameters setting, ViewType viewType, ICollection<ConnectorParameters> buttons)
        {
            this.flowDataCache.ScreenSettings = viewType switch
            {
                ViewType.Detail or ViewType.Delete => new ScreenSettings<DetailFormSettingsDescriptor>
                (
                    mapper.Map<DetailFormSettingsDescriptor>(setting),
                    mapper.Map<IEnumerable<ConnectorParameters>, IEnumerable<CommandButtonDescriptor>>(buttons),
                    viewType
                ),
                _ => throw new ArgumentException($"{nameof(viewType)}: {{097C0872-CC75-4772-9570-64D9868DF970}}"),
            };
        }

        public void DisplayHtmlForm(HtmlPageSettingsParameters setting, ICollection<ConnectorParameters> buttons)
           => this.flowDataCache.ScreenSettings = new ScreenSettings<HtmlPageSettingsDescriptor>
           (
               mapper.Map<HtmlPageSettingsDescriptor>(setting),
               mapper.Map<IEnumerable<ConnectorParameters>, IEnumerable<CommandButtonDescriptor>>(buttons),
               ViewType.Html
           );

        public void DisplayListForm(ListFormSettingsParameters setting, ICollection<ConnectorParameters> buttons)
           => this.flowDataCache.ScreenSettings = new ScreenSettings<ListFormSettingsDescriptor>
           (
               mapper.Map<ListFormSettingsDescriptor>(setting),
               mapper.Map<IEnumerable<ConnectorParameters>, IEnumerable<CommandButtonDescriptor>>(buttons),
               ViewType.List
           );
    }
}
