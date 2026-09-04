using LogicBuilder.App.Spa.Business.ScreenSettings.Views;
using LogicBuilder.App.Spa.Forms.Parameters.Common;
using System;

namespace LogicBuilder.App.Spa.Utils.Tests
{
    public class CustomDialogsTest
    {
        [Fact]
        public void DisplayEditFormThrowsArgumentException_ForInvalidViewType()
        {
            //arrange
            EditFormSettingsParameters setting = new(
                "",
                "",
                new FormRequestDetailsParameters("", "", "", "", "", ""),
                [],
                []
            );
            CustomDialogs customDialogs = new(null!, null!);

            //act && assert
            Assert.Throws<ArgumentException>(() => customDialogs.DisplayEditForm(setting, ViewType.Grid, []));
        }

        [Fact]
        public void DisplayDetailFormThrowsArgumentException_ForInvalidViewType()
        {
            //arrange
            DetailFormSettingsParameters setting = new(
                "",
                "",
                new FormRequestDetailsParameters("", "", "", "", "", ""),
                []
            );
            CustomDialogs customDialogs = new(null!, null!);

            //act && assert
            Assert.Throws<ArgumentException>(() => customDialogs.DisplayDetailForm(setting, ViewType.Grid, []));
        }
    }
}
