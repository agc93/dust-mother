using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnSave;
using UnSave.Types;

namespace DustMother.Core
{
    public class ConquestSave : WingmanSave
    {
        public ConquestSave(GvasSaveData rawSaveData) : base(rawSaveData)
        {
            Credits = rawSaveData.FindProperty<UEIntProperty>(p => p.Name == "Cash")?.Value;
            TotalScore = rawSaveData.FindProperty<UEIntProperty>("TotalScore")?.Value;
            AlertLevel = rawSaveData.FindProperty<UEIntProperty>("AlertLevel")?.Value;
            AlertLevelProgress = rawSaveData.FindProperty<UEFloatProperty>("AlertProgress")?.Value;
            Prestige = rawSaveData.FindProperty<UEIntProperty>("PersistentCash")?.Value;
            CordiumEngines = rawSaveData.FindProperty<UEIntProperty>("ThermalEngines")?.Value;
            Regions = rawSaveData.Properties
                .FindProperty<UEArrayProperty>("CQ_RegionSaves")
                .Items
                .Cast<UEStructProperty>()
                .Select(sp => new ConquestRegion(sp))
                .ToList()
                .AsReadOnly();
            Aircraft = rawSaveData.Properties
                .FindProperty<UEArrayProperty>("CQ_AircraftUnlock")
                .Items
                .Cast<UEStructProperty>()
                .Select(sp => new ConquestAircraftUnlock(sp))
                .ToList()
                .AsReadOnly();
            var playerData = rawSaveData.FindProperty<UEGenericStructProperty>("CQPlayerData");
            var alliesData = rawSaveData.FindProperty<UEGenericStructProperty>("AlliedCount");
            if (alliesData != null) {
                var alliesDict = new Dictionary<string, int>();
                try
                {
                    alliesDict = alliesData
                    .Properties
                    .Where(p => p is UEIntProperty)
                    .ToDictionary(k => k.Name.Split('_').First(), v => (v as UEIntProperty).Value);
                }
                catch (System.Exception)
                {
                    //ignored
                } finally {
                    Allies = new ReadOnlyDictionary<string, int>(alliesDict);
                }
            }
            var modifiersList = rawSaveData.FindProperty<UEGenericStructProperty>("ModifiersList");
            if (modifiersList != null && modifiersList.Properties.Count > 0)
            {
                ModifiersList = new Modifiers
                {
                    Difficulty = modifiersList.Properties.FindProperty<UEIntProperty>(p => p.Name.StartsWith("Difficulty"))?.Value,
                    AlertModifier = modifiersList.Properties.FindProperty<UEFloatProperty>(p => p.Name.StartsWith("AlertMod"))?.Value
                };
            }
            CurrentLoadout = new Loadout {
                AircraftName = playerData
                    .Properties
                    .FindProperty<UEGenericStructProperty>(p => p.Name.StartsWith("CurrentAircraft"))
                    .Properties
                    .FindProperty<UETextProperty>(p => p.Name.StartsWith("IndicatorName"))
                    .Value,
                Weapons = playerData
                    .Properties
                    .FindProperty<UEArrayProperty>(p => p.Name.StartsWith("WeaponStruct"))
                    .Items
                    .Cast<UEGenericStructProperty>()
                    .Select(gs => gs.Properties.FindProperty<UETextProperty>(p => p.Name.StartsWith("WeaponUIName_")).Value)
                    .ToList()
            };
            
        }

        public int? Credits {get;}
        public int? TotalScore {get;}
        public Loadout CurrentLoadout {get;}
        public ReadOnlyCollection<ConquestAircraftUnlock> Aircraft {get;}
        public ReadOnlyCollection<ConquestRegion> Regions {get;}
        // public ReadOnlyCollection<Modifier> Modifiers {get;}
        public int? AlertLevel {get;set;}
        public float? AlertLevelProgress {get;set;}
        public int? Prestige {get;set;}
        public int? CordiumEngines { get; set; }
        public ReadOnlyDictionary<string, int> Allies {get;set;}
        public Modifiers ModifiersList { get;  }

    }

    public class Loadout {
        public string AircraftName { get; internal set; }
        public List<string> Weapons {get;internal set;}
    }

    public class Modifiers {
        public int? Difficulty { get; set; }
        public float? AlertModifier { get; set; }
    }
}