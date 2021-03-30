using System.Linq;
using UnSave.Types;

namespace DustMother.Core
{
    public class ConquestAircraftUnlock : AircraftUnlock
    {
        public ConquestAircraftUnlock(UEStructProperty unlockItem)
        {
            if (unlockItem is UEGenericStructProperty structProp) {
                var idProp = structProp.Properties.FirstOrDefault(p => p.Name.StartsWith("CQ_id_")) as UEStringProperty;
                Id = idProp?.Value;
                var unlockProp = structProp.Properties.FindProperty<UEBoolProperty>(p => p.Name.StartsWith("CQ_Unlocked"));
                Unlocked = unlockProp?.Value;
                var availProp = structProp.Properties.FindProperty<UEBoolProperty>(p => p.Name.StartsWith("CQ_Available_"));
                Available = availProp?.Value;
                var purchaseProp = structProp.Properties.FindProperty<UEBoolProperty>(p => p.Name.StartsWith("CQ_Purchased_"));
                Purchased = purchaseProp?.Value;
                Level = structProp.Properties.FindProperty<UEIntProperty>(p => p.Name.StartsWith("CQ_Level_"))?.Value;
                Price = structProp.Properties.FindProperty<UEIntProperty>(p => p.Name.StartsWith("CQ_Price_"))?.Value;
            }
        }
        public int? Level {get;}
        public int? Price {get;}
    }
}