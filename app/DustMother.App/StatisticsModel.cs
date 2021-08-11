using DustMother.App.TypeHelpers;
using DustMother.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DustMother.App
{
    public class StatisticsModel : SaveViewModel<StatisticsSave>
    {
        public ObservableCollection<MissionRecord> Missions { get; set; } = new ObservableCollection<MissionRecord>();
        public StatisticsModel()
        {
            base.OnSaveLoad += SaveDataLoaded;
        }

        private void SaveDataLoaded(object sender, StatisticsSave data)
        {
            Missions.Clear();
            if (data.Missions != null && data.Missions.Any())
            {
                for (int i = 0; i < data.Missions.Count; i++)
                {
                    Missions.Add(new MissionRecord { MissionName = $"Mission {i+1}", Completion = data.Missions[i] });
                }
            }
        }

        public override async Task Refresh()
        {
            await RefreshSaveAsync(r => r.GetStatisticsDataAsync(), c => c?.TotalKills != null);
        }
    }
}
