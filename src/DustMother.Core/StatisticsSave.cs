using System.Collections.ObjectModel;
using System.Linq;
using UnSave;
using UnSave.Types;

namespace DustMother.Core
{
    public class StatisticsSave : WingmanSave
    {
        public StatisticsSave(GvasSaveData rawSaveData) : base(rawSaveData)
        {
            AircraftKills = rawSaveData.FindProperty<UEIntProperty>("STAT_AirKill")?.Value;
            GroundTargetKills = rawSaveData.FindProperty<UEIntProperty>("STAT_GroundKill")?.Value;
            VesselKills = rawSaveData.FindProperty<UEIntProperty>("STAT_VesselKill")?.Value;
            TotalKills = rawSaveData.FindProperty<UEIntProperty>("STAT_Kills")?.Value;
            Missions = rawSaveData
                .FindProperty<UEArrayProperty>("MissionList")
                .Items<UEStructProperty>()
                .Select(sp => new MissionCompletion(sp))
                .ToList()
                .AsReadOnly();
        }

        public int? AircraftKills {get;}
        public int? GroundTargetKills {get;}
        public int? VesselKills {get;}
        /// <summary>
        /// Unknown value.
        /// </summary>
        /// <remarks>
        ///     Despite what you'd think, this value is not just the sum of the other kill stats. 
        ///     No idea why.
        /// </remarks>
        /// <value></value>
        public int? TotalKills {get;}
        public ReadOnlyCollection<MissionCompletion> Missions {get;}
    }
}