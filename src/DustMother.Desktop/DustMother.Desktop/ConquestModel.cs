using DustMother.App.TypeHelpers;
using DustMother.Core;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace DustMother.App
{
    public class ConquestModel : SaveViewModel<ConquestSave>
    {
        private string alertStatus = string.Empty;

        public ObservableCollection<AircraftUnlock> UnlockedAircraft { get; set; } = new ObservableCollection<AircraftUnlock>();
        public ObservableCollection<ConquestRegion> Regions { get; set; } = new ObservableCollection<ConquestRegion>();
        public ObservableDictionary<string, int> Allies { get; set; } = new ObservableDictionary<string, int>();
        public string AlertStatus { get => alertStatus; set => SetProperty(ref alertStatus, value); }
        public ConquestModel()
        {
            base.OnSaveLoad += SaveDataLoaded;
        }

        private void SaveDataLoaded(object sender, ConquestSave data)
        {
            UnlockedAircraft.Clear();
            if (data.Aircraft != null && data.Aircraft.Any())
            {
                foreach (var unlock in data.Aircraft.Where(a => a.Available == true))
                {
                    UnlockedAircraft.Add(unlock);
                }
            }
            Regions.Clear();
            if (data.Regions != null && data.Regions.Any())
            {
                foreach (var region in data.Regions.Where(r => !string.IsNullOrWhiteSpace(r?.Name)))
                {
                    Regions.Add(region);
                }
            }
            Allies.Clear();
            if (data.Allies != null && data.Allies.Any())
            {
                foreach (var alliesEntry in data.Allies.Where(a => !string.IsNullOrWhiteSpace(a.Key)))
                {
                    Allies.Add(alliesEntry.Key, alliesEntry.Value);
                }
            }
            if (data.AlertLevel != null && data.AlertLevelProgress != null)
            {
                AlertStatus = $"{data.AlertLevel} ({Math.Round((decimal)(data.AlertLevelProgress * 100), 1)}%)";
            } else
            {
                AlertStatus = "None";
            }
            OnPropertyChanged(nameof(Prestige));
            OnPropertyChanged(nameof(CordiumEngines));
            OnPropertyChanged(nameof(Credits));
            OnPropertyChanged(nameof(AlertLevel));
            

        }

        public static string ToStatusText(int? alertLevel, float? alertLevelProgress)
        {
            if (alertLevel != null && alertLevelProgress != null)
            {
                return $"{alertLevel} ({Math.Round((decimal)(alertLevelProgress * 100), 1)}%)";
            }
            else
            {
                return "None";
            }
        }

        public override async Task Refresh()
        {
            await RefreshSaveAsync(r => r.GetConquestDataAsync(), c => c?.AlertLevelProgress != null);
        }

        public int? Prestige
        {
            get => SaveData?.Prestige ?? null; set
            {
                if (SaveData?.Prestige!= null && value != null && value != SaveData.Prestige)
                {
                    SaveData.Prestige = value;
                    PendingChanges = true;
                    OnPropertyChanged();
                }
            }
        }

        public int? CordiumEngines
        {
            get => SaveData?.CordiumEngines ?? null; set
            {
                if (SaveData?.CordiumEngines != null && value != null && value != SaveData.CordiumEngines)
                {
                    SaveData.CordiumEngines = value;
                    PendingChanges = true;
                    OnPropertyChanged();
                }
            }
        }

        public int? Credits
        {
            get => SaveData?.Credits?? null; set
            {
                if (SaveData?.Credits != null && value != null && value != SaveData.Credits)
                {
                    SaveData.Credits = value;
                    PendingChanges = true;
                    OnPropertyChanged();
                }
            }
        }

        public int? AlertLevel
        {
            get => SaveData?.AlertLevel ?? null;
            set
            {
                if (SaveData?.AlertLevel != null && value is int intVal && intVal != SaveData.AlertLevel)
                {
                    SaveData.AlertLevel = intVal;
                    PendingChanges = true;
                    OnPropertyChanged();
                }
            }
        }

    }
}
