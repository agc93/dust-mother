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
            TelemetryFirstPass = rawSaveData.FindProperty<UEBoolProperty>("TelemetryFirstPass")?.Value;
            TelemetryAccepted = rawSaveData.FindProperty<UEBoolProperty>("TelemetryAccepted")?.Value;
            CockpitFOV = rawSaveData.FindProperty<UEFloatProperty>("CockpitFOV")?.Value;
            ThirdPersonFOV = rawSaveData.FindProperty<UEFloatProperty>("ThirdPersonFOV")?.Value;
            AudioSettings = new AudioSettings
            {
                MasterLevel = rawSaveData.FindProperty<UEFloatProperty>("MasterAudio")?.Value,
                MusicLevel = rawSaveData.FindProperty<UEFloatProperty>("MusicAudio")?.Value,
                EffectsLevel = rawSaveData.FindProperty<UEFloatProperty>("SFXAudio")?.Value
            };
            SubtitlesOnTop = rawSaveData.FindProperty<UEBoolProperty>("SubtitleTop")?.Value;
            InterfaceScale = rawSaveData.FindProperty<UEFloatProperty>("UI_Scale")?.Value;

        }

        public float? ResolutionScale {get;}
        public float? InterfaceScale { get; }
        public int? FrameLimit {get;}
        public int? CloudResolution {get;}
        public bool? ToggleAoA {get;}
        public bool? TelemetryFirstPass {get;}
        public bool? TelemetryAccepted { get; }
        public float? CockpitFOV { get; }
        public float? ThirdPersonFOV { get; }
        public AudioSettings? AudioSettings { get; }
        public bool? SubtitlesOnTop { get; }

    }

    public class AudioSettings
    {
        public float? MasterLevel { get; internal set; }
        public float? EffectsLevel { get; internal set; }
        public float? MusicLevel { get; internal set; }
    }
}