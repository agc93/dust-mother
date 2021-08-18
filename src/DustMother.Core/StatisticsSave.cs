using System.Collections.ObjectModel;
using System.Linq;
using UnSave;
using UnSave.Extensions;
using UnSave.Types;

namespace DustMother.Core
{
    public partial class StatisticsSave : WingmanSave
    {
        public StatisticsSave(GvasSaveData rawSaveData) : base(rawSaveData)
        {
            var missionList = rawSaveData.FindProperty<UEArrayProperty>("MissionList");
            if (missionList != null)
            {
                Missions = missionList
                    .Items<UEStructProperty>()
                    .Select(sp => new MissionCompletion(sp))
                    .ToList()
                    .AsReadOnly();
            }
        }

        [ValueProperty(ValuePropertyName = "AircraftKills")]
        private UEIntProperty AirKill => RawSaveData.FindProperty<UEIntProperty>("STAT_AirKill");

        [ValueProperty(ValuePropertyName = "GroundTargetKills")]
        private UEIntProperty GroundKill => RawSaveData.FindProperty<UEIntProperty>("STAT_GroundKill");

        [ValueProperty(ValuePropertyName = "VesselKills")]
        private UEIntProperty VesselKill => RawSaveData.FindProperty<UEIntProperty>("STAT_VesselKill");

        [ValueProperty(ValuePropertyName = "TotalKills")]
        private UEIntProperty Kills => RawSaveData.FindProperty<UEIntProperty>("STAT_Kills");

        public ReadOnlyCollection<MissionCompletion>? Missions {get;}

        public override string FileName => "stat";
    }
}