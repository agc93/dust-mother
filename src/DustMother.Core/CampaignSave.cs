using System.Collections.ObjectModel;
using System.Linq;
using UnSave;
using UnSave.Extensions;
using UnSave.Types;

namespace DustMother.Core
{
    // [SaveProperty("FurthestCampaignMission", typeof(UEBoolProperty), IncludeValueProperty = true, PropertyName = "FurthestMission")]
    public partial class CampaignSave : WingmanSave
    {
        public CampaignSave(GvasSaveData saveData) : base(saveData)
        {
            // Credits = saveData.Properties.FindProperty<UEIntProperty>(p => p.Name == "CampaignCredits")?.Value;
            // CampaignActive = saveData.Properties.FindProperty<UEBoolProperty>(p => p.Name == "HasACampaign")?.Value;
            CampaignCompleted = saveData.Properties.FindProperty<UEBoolProperty>(p => p.Name == "HasFinishedCampaignOnce")?.Value;
            FurthestMission = saveData.Properties.FindProperty<UEStringProperty>(p => p.Name == "FurthestCampaignMission")?.Value;
            CurrentCampaign = new CampaignDetails {
                CurrentMission = saveData.Properties.FindProperty<UEStringProperty>(p => p.Name == "CurrentCampaignMission")?.Value,
                Difficulty = saveData.Properties.FindProperty<UEIntProperty>(p => p.Name == "CurrentCampaignDifficulty")?.Value
            };
            UnlockedAircraft = saveData.Properties
                .FindProperty<UEArrayProperty>(p => p.Name == "CD_AircraftUnlock")?
                .Items<UEStructProperty>()
                .Select(gs => new AircraftUnlock(gs))
                .ToList()
                .AsReadOnly();
        }

        [ValueProperty(ValuePropertyName = "Credits")]
        private UEIntProperty CampaignCredits =>
            RawSaveData.Properties.FindProperty<UEIntProperty>(p => p.Name == "CampaignCredits");
        
        [ValueProperty(ValuePropertyName = "CampaignActive")]
        private UEBoolProperty HasACampaign => RawSaveData.FindProperty<UEBoolProperty>(p => p.Name == "HasACampaign");
        
        [ValueProperty(ValuePropertyName = "FurthestMission")]
        private UEStringProperty FurthestCampaignMission => RawSaveData.Properties.FindProperty<UEStringProperty>(p => p.Name == "FurthestCampaignMission");

        // public int? Credits {get; private set;}
        // public bool? CampaignActive {get; private set;}
        public bool? CampaignCompleted {get; private set;}
        public CampaignDetails? CurrentCampaign {get; private set;}
        public ReadOnlyCollection<AircraftUnlock> UnlockedAircraft {get; private set;}
        
    }
}