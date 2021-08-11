using DustMother.Core;
using System.Collections.ObjectModel;
using System.Linq;

namespace DustMother.Runtime
{
    internal static class SummaryBuilder
    {
        internal static CampaignSaveSummary CreateCampaignSummary(CampaignSave save)
        {
            var summary = new CampaignSaveSummary
            {
                CampaignActive = save.CampaignActive,
                CampaignCompleted = save.CampaignCompleted,
                Credits = save.Credits,
                CurrentDifficulty = save.CurrentCampaign?.Difficulty.ToDifficulty(),
                CurrentMission = save.CurrentCampaign?.CurrentMission,
                FurthestMission = save.FurthestMission
            };
            if (save.UnlockedAircraft.Any())
            {
                var dict = save.UnlockedAircraft
                    .Where(kvp => !string.IsNullOrWhiteSpace(kvp?.Id))
                    .ToDictionary(
                    k => k.Id,
                    v => new UnlockStatus { Available = v.Available.Value, Purchased = v.Purchased.Value, Unlocked = v.Unlocked.Value });
                summary.UnlockedAircraft = new ReadOnlyDictionary<string, UnlockStatus>(dict);
            }
            return summary;
        }

        internal static string ToDifficulty(this int? difficulty)
        {
            return difficulty switch
            {
                0 => "Easy",
                1 => "Normal",
                2 => "Hard",
                3 => "Mercenary",
                _ => "Unknown"
            };
        }
    }
}
