using UnSave;
using UnSave.Extensions;
using UnSave.Types;

namespace DustMother.Core
{
    public partial class CampaignDetails {
        private GvasSaveData _saveData;

        public CampaignDetails(GvasSaveData saveData)
        {
            _saveData = saveData;
        }

        [ValueProperty(ValuePropertyName = "CurrentMission")]
        private UEStringProperty? CurrentCampaignMission => _saveData.Properties.FindProperty<UEStringProperty>(p => p.Name == "CurrentCampaignMission");
        //public string CurrentMission { get; internal set; }

        [ValueProperty(ValuePropertyName = "Difficulty")]
        private UEIntProperty? CurrentCampaignDifficulty => _saveData.Properties.FindProperty<UEIntProperty>(p => p.Name == "CurrentCampaignDifficulty");
    }
}