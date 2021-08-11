using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace DustMother.Runtime
{
    public sealed class CampaignSaveSummary
    {
        public int? Credits { get; set; }
        public bool? CampaignActive { get; set; }
        public bool? CampaignCompleted { get; set; }
        public string FurthestMission { get; set; }
        public string CurrentMission { get; set; }
        public string CurrentDifficulty { get; set; }

        public IReadOnlyDictionary<string, UnlockStatus> UnlockedAircraft { get; set; } = new ReadOnlyDictionary<string, UnlockStatus>(new Dictionary<string, UnlockStatus>());
    }
}
