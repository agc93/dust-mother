using UnSave;
using UnSave.Extensions;
using UnSave.Types;

namespace DustMother.Core
{
    public partial class SettingsSave : WingmanSave
    {
        public SettingsSave(GvasSaveData rawSaveData) : base(rawSaveData)
        {
            TelemetryFirstPass = rawSaveData.FindProperty<UEBoolProperty>("TelemetryFirstPass")?.Value;
            TelemetryAccepted = rawSaveData.FindProperty<UEBoolProperty>("TelemetryAccepted")?.Value;
            AudioSettings = new AudioSettings
            {
                MasterLevel = rawSaveData.FindProperty<UEFloatProperty>("MasterAudio")?.Value,
                MusicLevel = rawSaveData.FindProperty<UEFloatProperty>("MusicAudio")?.Value,
                EffectsLevel = rawSaveData.FindProperty<UEFloatProperty>("SFXAudio")?.Value
            };
            SubtitlesOnTop = rawSaveData.FindProperty<UEBoolProperty>("SubtitleTop")?.Value;

        }

        

        //public bool? ToggleAoA {get;}
        public bool? TelemetryFirstPass {get;}
        public bool? TelemetryAccepted { get; }
        //public float? CockpitFOV { get; }
        //public float? ThirdPersonFOV { get; }

        [ValueProperty(ValuePropertyName = "CockpitFOV", CreateProperty = "CockpitFOV")]
        private UEFloatProperty CockpitFOVProperty => RawSaveData.FindProperty<UEFloatProperty>("CockpitFOV");

        [ValueProperty(ValuePropertyName = "ThirdPersonFOV", CreateProperty = "ThirdPersonFOV")]
        private UEFloatProperty ThirdPersonFOVProperty => RawSaveData.FindProperty<UEFloatProperty>("ThirdPersonFOV");

        [ValueProperty(ValuePropertyName = "ResolutionScale", CreateProperty = "ResolutionScale")]
        private UEFloatProperty ResolutionScaleProperty => RawSaveData.FindProperty<UEFloatProperty>("ResolutionScale");

        [ValueProperty(ValuePropertyName = "FrameLimit", CreateProperty = "FrameLimit")]
        private UEIntProperty FrameLimitProperty => RawSaveData.FindProperty<UEIntProperty>("FrameLimit");

        [ValueProperty(ValuePropertyName = "CloudResolution", CreateProperty = "CloudResolution")]
        private UEIntProperty CloudResolutionProperty => RawSaveData.FindProperty<UEIntProperty>("CloudResolution");

        [ValueProperty(ValuePropertyName = "InterfaceScale", CreateProperty = "UI_Scale")]
        private UEFloatProperty InterfaceScaleProperty => RawSaveData.FindProperty<UEFloatProperty>("UI_Scale");

        [ValueProperty(ValuePropertyName = "MetricHUDUnits", CreateProperty = "HUDUnits")]
        private UEBoolProperty HUDUnits => RawSaveData.FindProperty<UEBoolProperty>("HUDUnits");

        [ValueProperty(ValuePropertyName = "ToggleAoA", CreateProperty = "AoAToggle")]
        private UEBoolProperty AoAToggle => RawSaveData.FindProperty<UEBoolProperty>("AoAToggle");

        public AudioSettings? AudioSettings { get; }
        public bool? SubtitlesOnTop { get; }

        public override string FileName => "savegame";

    }

    public class AudioSettings
    {
        public float? MasterLevel { get; internal set; }
        public float? EffectsLevel { get; internal set; }
        public float? MusicLevel { get; internal set; }
    }
}