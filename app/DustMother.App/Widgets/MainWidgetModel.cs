using DustMother.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace DustMother.App.Widgets
{
    //public class MainWidgetViewModel : WidgetViewModel<MainWidgetModel, SettingsSave>
    //{
    //    public MainWidgetViewModel(ISynchronizeInvoke sync) : base(new MainWidgetModel(), sync)
    //    {

    //    }
        
    //}

    public class MainWidgetModel : WidgetSaveViewModel<SettingsSave>
    {
        public MainWidgetModel(INotifyBase notify) : base(notify)
        {
        }

        public override async Task Refresh()
        {
            await RefreshSaveAsync(r => r.GetSettingsDataAsync(), c => c?.ResolutionScale != null);
        }
    }
}
