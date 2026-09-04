using LogicBuilder.App.Spa.Business.ScreenSettings.Views;
using LogicBuilder.App.Spa.Forms.Parameters.Common;
using LogicBuilder.App.Spa.Utils.Interfaces;
using LogicBuilder.Attributes;
using LogicBuilder.Forms.Parameters;
using System.Collections.Generic;

namespace LogicBuilder.App.Spa.Utils
{
    public static class CustomDialogUtilities
    {
        [AlsoKnownAs("DisplayGrid")]
        [FunctionGroup(FunctionGroup.DialogForm)]
        public static void DisplayGrid
        (
            ICustomDialogs customDialogs,

            GridSettingsParameters setting,

            [ListEditorControl(ListControlType.Connectors)]
            ICollection<ConnectorParameters> buttons
        )
        {
            customDialogs.DisplayGrid(setting, buttons);
        }

        [AlsoKnownAs("DisplayEditForm")]
        [FunctionGroup(FunctionGroup.DialogForm)]
        public static void DisplayEditForm
        (
            ICustomDialogs customDialogs,
            [Comments("Configuration details for the form.")]
            EditFormSettingsParameters setting,

            [Comments("Create or Edit")]
            ViewType viewType,

            [ListEditorControl(ListControlType.Connectors)]
            ICollection<ConnectorParameters> buttons
        )
        {
            customDialogs.DisplayEditForm(setting, viewType, buttons);
        }

        [AlsoKnownAs("DisplayDetail")]
        [FunctionGroup(FunctionGroup.DialogForm)]
        public static void DisplayDetailForm
        (
            ICustomDialogs customDialogs,

            [Comments("Configuration details for the form.")]
            DetailFormSettingsParameters setting,

            [Comments("Detail or Delete")]
            ViewType viewType,

            [ListEditorControl(ListControlType.Connectors)]
            ICollection<ConnectorParameters> buttons
        )
        {
            customDialogs.DisplayDetailForm(setting, viewType, buttons);
        }

        [AlsoKnownAs("DisplayHtmlContent")]
        [FunctionGroup(FunctionGroup.DialogForm)]
        public static void DisplayHtmlForm
        (
            ICustomDialogs customDialogs,

            [Comments("Configuration details for the form.")]
            HtmlPageSettingsParameters setting,

            [ListEditorControl(ListControlType.Connectors)]
            ICollection<ConnectorParameters> buttons
        )
        {
            customDialogs.DisplayHtmlForm(setting, buttons);
        }

        [AlsoKnownAs("DisplayListForm")]
        [FunctionGroup(FunctionGroup.DialogForm)]
        public static void DisplayListForm
        (
            ICustomDialogs customDialogs,

            [Comments("Configuration details for the form.")]
            ListFormSettingsParameters setting,

            [ListEditorControl(ListControlType.Connectors)]
            ICollection<ConnectorParameters> buttons
        )
        {
            customDialogs.DisplayListForm(setting, buttons);
        }
    }
}
