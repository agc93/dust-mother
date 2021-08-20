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
            OnPropertyChanged(nameof(CurrentMission));
            OnPropertyChanged(nameof(Difficulty));
        }

        public override async Task Refresh()
        {
            await RefreshSaveAsync(r => r.GetCampaignData(), c => c?.CampaignActive != null);
        }

        public int Credits
        {
            get => SaveData?.Credits ?? 0; set
            {
                if (SaveData?.Credits != null && SaveData.Credits != value)
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
                if (SaveData?.CampaignCompleted != null && value != SaveData.CampaignCompleted)
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
                if (SaveData?.CurrentCampaign?.CurrentMission != null && value is int intVal && $"campaign_{intVal:D2}" != SaveData.CurrentCampaign.CurrentMission)
                {
                    SaveData.UpdateCurrentCampaign($"campaign_{intVal:D2}");
                    PendingChanges = true;
                    OnPropertyChanged();
                }
            }
        }

        public string? Difficulty
        {
            get
            {
                var currentMission = SaveData?.CurrentCampaign?.Difficulty ?? null;
                return currentMission == null ? null : PropertyValueConverter.Difficulty.FromValue(currentMission);
            }
            set
            {
                var dValue = PropertyValueConverter.Difficulty.ToValue(value);
                if (SaveData?.CurrentCampaign?.Difficulty != null && dValue != null && dValue != SaveData.CurrentCampaign.Difficulty)
                {
                    SaveData.CurrentCampaign.Difficulty = dValue;
                    PendingChanges = true;
                    OnPropertyChanged();
                }
            }
        }

    }
}
