using DustMother.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DustMother.App
{
    public class MainPageViewModel : SaveViewModel<SettingsSave>
    {
        
        public MainPageViewModel()
        {
            
        }
        public override async Task Refresh()
        {
            await RefreshSaveAsync(r => r.GetSettingsDataAsync(), t => t?.ResolutionScale != null);
        }
    }
}
