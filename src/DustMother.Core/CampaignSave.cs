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
            //CampaignCompleted = saveData.Properties.FindProperty<UEBoolProperty>(p => p.Name == "HasFinishedCampaignOnce")?.Value;
            // FurthestMission = saveData.Properties.FindProperty<UEStringProperty>(p => p.Name == "FurthestCampaignMission")?.Value;
            CurrentCampaign = new CampaignDetails(saveData);
            //CurrentCampaign = new CampaignDetails {
            //    CurrentMission = saveData.Properties.FindProperty<UEStringProperty>(p => p.Name == "CurrentCampaignMission")?.Value,
            //    Difficulty = saveData.Properties.FindProperty<UEIntProperty>(p => p.Name == "CurrentCampaignDifficulty")?.Value
            //};
            var aircraftUnlock = saveData.Properties
                .FindProperty<UEArrayProperty>(p => p.Name == "CD_AircraftUnlock");
            if (aircraftUnlock != null)
            {
                UnlockedAircraft = aircraftUnlock
                    .Items<UEStructProperty>()
                    .Select(gs => new AircraftUnlock(gs))
                    .ToList()
                    .AsReadOnly();
            }
            
        }

        [ValueProperty(ValuePropertyName = "Credits")]
        private UEIntProperty CampaignCredits =>
            RawSaveData.Properties.FindProperty<UEIntProperty>(p => p.Name == "CampaignCredits");
        
        [ValueProperty(ValuePropertyName = "CampaignActive")]
        private UEBoolProperty HasACampaign => RawSaveData.FindProperty<UEBoolProperty>(p => p.Name == "HasACampaign");
        
        [ValueProperty(ValuePropertyName = "FurthestMission")]
        private UEStringProperty FurthestCampaignMission => RawSaveData.Properties.FindProperty<UEStringProperty>(p => p.Name == "FurthestCampaignMission");

        [ValueProperty(ValuePropertyName = "CampaignCompleted")]
        private UEBoolProperty HasFinishedCampaignOnce => RawSaveData.FindProperty<UEBoolProperty>(p => p.Name == "HasFinishedCampaignOnce");

        public CampaignDetails? CurrentCampaign {get; private set;}
        public ReadOnlyCollection<AircraftUnlock>? UnlockedAircraft {get; private set;}

        private UEStringProperty CurrentCampaignMission => RawSaveData.Properties.FindProperty<UEStringProperty>(p => p.Name == "CurrentCampaignMission");

        public override string FileName => "Campaign";

        public CampaignSave UpdateCurrentCampaign(string currentMission, int? currentDifficulty = null)
        {
            if (this.CurrentCampaign != null)
            {
                //TODO: validate this
                CurrentCampaign.CurrentMission = currentMission;

                if (currentDifficulty != null)
                {
                    CurrentCampaign.Difficulty = currentDifficulty;
                }
            }
            return this;
        }
        
    }
}