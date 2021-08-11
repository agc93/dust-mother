using UnSave.Types;

namespace DustMother.Core
{
    public class ConquestRegion {
        public ConquestRegion(UEStructProperty structProp)
        {
            Property = structProp;
            var prop = structProp as UEGenericStructProperty;
            if (prop != null) {
                Name = prop.Properties.FindProperty<UETextProperty>(p => p.Name.StartsWith("TerritoryName_"))?.Value;
                Team = prop.Properties.FindProperty<UEIntProperty>(p => p.Name.StartsWith("Team_"))?.Value;
                CordiumDeposits = prop.Properties.FindProperty<UEIntProperty>(p => p.Name.StartsWith("CordiumDeposit_"))?.Value;
                RawObjectiveType = prop.Properties.FindProperty<UEEnumProperty>(p => p.Name.StartsWith("ObjType"))?.Value;
                var unlockType = prop.Properties.FindProperty<UEEnumProperty>(p => p.Name.StartsWith("UnlockType"))?.Value;
            }
            else {
                throw new System.Exception("Wrong struct type I guess?");
            }
        }

        public string? Name {get;set;}
        public int? Team {get;set;}
        public int? CordiumDeposits {get;set;}
        public string? RawObjectiveType { get; set; }
        private UEStructProperty Property {get;}
    }
}