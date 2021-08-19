using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnSave;
using UnSave.Extensions;
using UnSave.Types;

namespace DustMother.Core
{
    public partial class ConquestSave : WingmanSave
    {
        public ConquestSave(GvasSaveData rawSaveData) : base(rawSaveData)
        {
            var regionSaves = RawSaveData.FindProperty<UEArrayProperty>("CQ_RegionSaves");
            if (regionSaves != null)
            {
                Regions = regionSaves
                    .Items
                    .Cast<UEStructProperty>()
                    .Select(sp => new ConquestRegion(sp))
                    .ToList()
                    .AsReadOnly();
            }

            var aircraftUnlocks = RawSaveData.FindProperty<UEArrayProperty>("CQ_AircraftUnlock");
            if (aircraftUnlocks != null)
            {
                Aircraft = aircraftUnlocks
                    .Items
                    .Cast<UEStructProperty>()
                    .Select(sp => new ConquestAircraftUnlock(sp))
                    .ToList()
                    .AsReadOnly();
            }
            
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
                    .FindProperty<UEGenericStructProperty>(p => p.Name.StartsWith("CurrentAircraft"))?
                    .Properties
                    .FindProperty<UETextProperty>(p => p.Name.StartsWith("IndicatorName"))
                    .Value,
                Weapons = playerData
                    .Properties
                    .FindProperty<UEArrayProperty>(p => p.Name.StartsWith("WeaponStruct"))?
                    .Items
                    .Cast<UEGenericStructProperty>()
                    .Select(gs => gs.Properties.FindProperty<UETextProperty>(p => p.Name.StartsWith("WeaponUIName_")).Value)
                    .ToList()
            };
            
        }

        [ValueProperty(ValuePropertyName = "Credits")]
        private UEIntProperty? Cash => RawSaveData.FindProperty<UEIntProperty>(p => p.Name == "Cash");

        [ValueProperty(ValuePropertyName = "TotalScore")]
        private UEIntProperty Score => RawSaveData.FindProperty<UEIntProperty>("TotalScore");

        [ValueProperty(ValuePropertyName = "AlertLevel")]
        private UEIntProperty RawAlertLevel => RawSaveData.FindProperty<UEIntProperty>("AlertLevel");

        [ValueProperty(ValuePropertyName = "AlertLevelProgress")]
        private UEFloatProperty AlertProgress => RawSaveData.FindProperty<UEFloatProperty>("AlertProgress");

        [ValueProperty(ValuePropertyName = "Prestige")]
        private UEIntProperty PersistentCash => RawSaveData.FindProperty<UEIntProperty>("PersistentCash");

        //[ValueProperty(ValuePropertyName = "CordiumEngines")]
        private UEIntProperty ThermalEngines => RawSaveData.FindProperty<UEIntProperty>("ThermalEngines");

        public int? CordiumEngines
        {
            get { return ThermalEngines?.Value ?? 0; }
            set
            {
                if (ThermalEngines?.Value is not null && value is not null)
                {
                    ThermalEngines.Value = (int)value;
                }
                else if (ThermalEngines?.Value is null && value is int intVal && intVal > 0)
                {
                    //value is good, but property isn't
                    RawSaveData.Properties.Insert(RawSaveData.Properties.Count - 2, new UEIntProperty
                    {
                        Name = "ThermalEngines",
                        Value = (int)value,
                        ValueLength = 4
                    });
                }
            }
        }


        public Loadout CurrentLoadout {get;}
        public ReadOnlyCollection<ConquestAircraftUnlock>? Aircraft {get;}
        public ReadOnlyCollection<ConquestRegion>? Regions {get;}
        // public ReadOnlyCollection<Modifier> Modifiers {get;}
        public ReadOnlyDictionary<string, int>? Allies {get;set;}
        public Modifiers? ModifiersList { get;  }

        public override string FileName => "Conquest";

    }

    public class Loadout {
        public string? AircraftName { get; internal set; }
        public List<string>? Weapons {get;internal set;}
    }

    public class Modifiers {
        public int? Difficulty { get; set; }
        public float? AlertModifier { get; set; } = null;
    }
}