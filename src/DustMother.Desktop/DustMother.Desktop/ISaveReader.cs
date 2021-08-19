using DustMother.Core;
using System.Threading.Tasks;

namespace DustMother.App
{
    public interface ISaveReader
    {
        Task<bool?> CheckFileAccessAsync();
        Task<CampaignSave> GetCampaignData();
        Task<ConquestSave> GetConquestDataAsync();
        Task<SettingsSave> GetSettingsDataAsync();
        Task<StatisticsSave> GetStatisticsDataAsync();
        Task<bool> WriteSaveAsync(WingmanSave save);
    }
}
