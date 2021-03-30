using UnSave;
using UnSave.Types;

namespace DustMother.Core
{
    public class SettingsSave : WingmanSave
    {
        public SettingsSave(GvasSaveData rawSaveData) : base(rawSaveData)
        {
            ResolutionScale = rawSaveData.FindProperty<UEFloatProperty>("ResolutionScale")?.Value;
            FrameLimit = rawSaveData.FindProperty<UEIntProperty>("FrameLimit")?.Value;
            CloudResolution = rawSaveData.FindProperty<UEIntProperty>("CloudResolution")?.Value;
            ToggleAoA = rawSaveData.FindProperty<UEBoolProperty>("AoAToggle")?.Value;
            DisableTelemetry = rawSaveData.FindProperty<UEBoolProperty>("TelemetryFirstPass")?.Value;
        }

        public float? ResolutionScale {get;}
        public int? FrameLimit {get;}
        public int? CloudResolution {get;}
        public bool? ToggleAoA {get;}
        public bool? DisableTelemetry {get;}
    }
}