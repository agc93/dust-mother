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
            }
            else {
                throw new System.Exception("Wrong struct type I guess?");
            }
        }

        public string? Name {get;set;}
        public int? Team {get;set;}
        public int? CordiumDeposits {get;set;}
        private UEStructProperty Property {get;}
    }
}