using DustMother.Core;
using System.Threading.Tasks;

namespace DustMother.App.Widgets
{
    public class StatisticsWidgetViewModel : SaveViewModel<StatisticsSave>
    {
        public override async Task Refresh()
        {
            await RefreshSaveAsync(r => r.GetStatisticsDataAsync(), c => c?.TotalKills != null);
        }
    }
}
