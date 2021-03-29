using System.Linq;
using UnSave.Types;

namespace DustMother.Core
{
    public class AircraftUnlock {
        public AircraftUnlock(UEStructProperty unlockItem)
        {
            if (unlockItem is UEGenericStructProperty structProp) {
                var idProp = structProp.Properties.FirstOrDefault(p => p.Name.StartsWith("ID_")) as UEStringProperty;
                Id = idProp?.Value;
                var unlockProp = structProp.Properties.FindProperty<UEBoolProperty>(p => p.Name.StartsWith("Unlocked"));
                Unlocked = unlockProp?.Value;
                var availProp = structProp.Properties.FindProperty<UEBoolProperty>(p => p.Name.StartsWith("Available_"));
                Available = availProp?.Value;
                var purchaseProp = structProp.Properties.FindProperty<UEBoolProperty>(p => p.Name.StartsWith("Purchased_"));
                Purchased = purchaseProp?.Value;
            }
        }
        public string Id {get;}
        public bool? Unlocked {get;}
        public bool? Available {get;}
        public bool? Purchased {get;}
    }
}