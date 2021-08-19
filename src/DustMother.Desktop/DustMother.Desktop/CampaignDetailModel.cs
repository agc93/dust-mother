using DustMother.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml;

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
            if (data.UnlockedAircraft != null)
            {
                foreach (var unlock in data.UnlockedAircraft.Where(a => a.Available == true && a.Unlocked == true))
                {
                    UnlockedAircraft.Add(unlock);
                }
            }
            OnPropertyChanged(nameof(Credits));
            OnPropertyChanged(nameof(CampaignCompleted));
        }

        public override async Task Refresh()
        {
            await RefreshSaveAsync(r => r.GetCampaignData(), c => c?.CampaignActive != null);
        }

        public void UpdateCredits(int? value)
        {
            //if (value != null)
            //{
            //    SaveData.Credits = value;

            //}
            OnPropertyChanged("SaveData.Credits");
            OnPropertyChanged("Credits");
        }

        public int Credits
        {
            get => SaveData?.Credits ?? 0; set
            {
                if (SaveData?.Credits != null)
                {
                    SaveData.Credits = value;
                    PendingChanges = true;
                    OnPropertyChanged();
                }
            }
        }

        public bool CampaignCompleted
        {
            get => SaveData?.CampaignCompleted ?? false; set
            {
                if (SaveData?.CampaignCompleted != null)
                {
                    SaveData.CampaignCompleted = value;
                    PendingChanges = true;
                    OnPropertyChanged();
                }
            }
        }

        public int? CurrentMission
        {
            get
            {
                var currentMission = SaveData?.CurrentCampaign?.CurrentMission ?? null;
                return currentMission == null ? null : int.TryParse(currentMission.Split('_').Last(), out var missionNum) ? missionNum : null;
            } set
            {
                if (SaveData?.CurrentCampaign?.CurrentMission != null && value != null)
                {
                    SaveData.UpdateCurrentCampaign($"campaign_{value}");
                    PendingChanges = true;
                    OnPropertyChanged();
                }
            }
        }

    }
}
