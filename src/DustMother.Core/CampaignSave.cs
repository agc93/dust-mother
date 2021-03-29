using System.Collections.ObjectModel;
using System.Linq;
using UnSave;
using UnSave.Types;

namespace DustMother.Core
{
    public class CampaignSave
    {
        public CampaignSave(GvasSaveData saveData)
        {
            RawSaveData = saveData;
            Credits = saveData.Properties.FindProperty<UEIntProperty>(p => p.Name == "CampaignCredits")?.Value;
            CampaignActive = saveData.Properties.FindProperty<UEBoolProperty>(p => p.Name == "HasACampaign")?.Value;
            CampaignCompleted = saveData.Properties.FindProperty<UEBoolProperty>(p => p.Name == "HasFinishedCampaignOnce")?.Value;
            FurthestMission = saveData.Properties.FindProperty<UEStringProperty>(p => p.Name == "FurthestCampaignMission")?.Value;
            CurrentCampaign = new CampaignDetails {
                CurrentMission = saveData.Properties.FindProperty<UEStringProperty>(p => p.Name == "CurrentCampaignMission")?.Value,
                Difficulty = saveData.Properties.FindProperty<UEIntProperty>(p => p.Name == "CurrentCampaignDifficulty")?.Value
            };
            UnlockedAircraft = saveData.Properties
                .FindProperty<UEArrayProperty>(p => p.Name == "CD_AircraftUnlock")?
                .Items
                .Cast<UEStructProperty>()
                .Select(gs => new AircraftUnlock(gs))
                .ToList()
                .AsReadOnly();
        }

        public int? Credits {get; private set;}
        public bool? CampaignActive {get; private set;}
        public bool? CampaignCompleted {get; private set;}
        public string FurthestMission {get; private set;}
        public CampaignDetails? CurrentCampaign {get; private set;}
        public ReadOnlyCollection<AircraftUnlock> UnlockedAircraft {get; private set;}
        private GvasSaveData RawSaveData {get; set;}
        
    }
}