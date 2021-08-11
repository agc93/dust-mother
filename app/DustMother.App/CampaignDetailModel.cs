using DustMother.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace DustMother.App
{
    public class CampaignDetailModel : SaveViewModel<CampaignSave>
    {
        public ObservableCollection<AircraftUnlock> UnlockedAircraft { get; set; } = new ObservableCollection<AircraftUnlock>();

        public CampaignDetailModel()
        {
            base.OnSaveLoad += SaveDataLoaded;
        }

        private void SaveDataLoaded(object sender, CampaignSave data)
        {
            UnlockedAircraft.Clear();
            foreach (var unlock in data.UnlockedAircraft.Where(a => a.Available == true && a.Unlocked == true))
            {
                UnlockedAircraft.Add(unlock);
            }
        }

        public override async Task Refresh()
        {
            await RefreshSaveAsync(r => r.GetCampaignData(), c => c?.CampaignActive != null);
        }
    }
}
