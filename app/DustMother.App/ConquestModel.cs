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
            Allies.Clear();
            foreach (var unlock in data.Aircraft.Where(a => a.Available == true))
            {
                UnlockedAircraft.Add(unlock);
            }
            Regions.Clear();
            foreach (var region in data.Regions.Where(r => !string.IsNullOrWhiteSpace(r?.Name)))
            {
                Regions.Add(region);
            }
            if (data.Allies != null && data.Allies.Any())
            {
                foreach (var alliesEntry in data.Allies.Where(a => !string.IsNullOrWhiteSpace(a.Key)))
                {
                    Allies.Add(alliesEntry.Key, alliesEntry.Value);
                }
            }
            AlertStatus = $"{data.AlertLevel} ({Math.Round((decimal)(data.AlertLevelProgress * 100), 1)}%)";

        }

        public override async Task Refresh()
        {
            await RefreshSaveAsync(r => r.GetConquestDataAsync(), c => c?.AlertLevelProgress != null);
        }
    }
}
