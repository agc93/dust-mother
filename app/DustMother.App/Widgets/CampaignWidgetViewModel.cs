using DustMother.Core;
using DustMother.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace DustMother.App.Widgets
{
    public class CampaignWidgetViewModel : SaveViewModel<CampaignSave>
    {
        public CampaignWidgetViewModel()
        {
        }

        public override async Task Refresh()
        {
            await RefreshSaveAsync(r => r.GetCampaignData(), c => c?.CampaignActive != null);
        }
    }
}
