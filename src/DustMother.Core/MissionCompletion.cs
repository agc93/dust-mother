using UnSave.Types;

namespace DustMother.Core
{
    public class MissionCompletion {
        public MissionCompletion(UEStructProperty structProp)
        {
            if (structProp is UEGenericStructProperty prop) {
                MissionName = prop.Properties.FindProperty<UEStringProperty>(p => p.Name.StartsWith("missionID_"))?.Value;
                CompletedEasy = prop.Properties.FindProperty<UEBoolProperty>(p => p.Name.StartsWith("easy_"))?.Value;
                CompletedNormal = prop.Properties.FindProperty<UEBoolProperty>(p => p.Name.StartsWith("normal_"))?.Value;
                CompletedHard = prop.Properties.FindProperty<UEBoolProperty>(p => p.Name.StartsWith("hard_"))?.Value;
                CompletedMercenary = prop.Properties.FindProperty<UEBoolProperty>(p => p.Name.StartsWith("mercenary_"))?.Value;
            }
        }

        public string? MissionName {get;}
        public bool? CompletedEasy {get;}
        public bool? CompletedNormal {get;}
        public bool? CompletedHard {get;}
        public bool? CompletedMercenary {get;}
    }
}