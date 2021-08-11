using System.Linq;
using UnSave.Types;

namespace DustMother.Core
{
    public class AircraftUnlock {
        internal AircraftUnlock(UEStructProperty unlockItem)
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
        protected AircraftUnlock() {}
        public string Id {get;protected set;}
        public bool? Unlocked {get;protected set;}
        public bool? Available {get; protected set;}
        public bool? Purchased {get; protected set;}
    }
}