using DustMother.Core;
using DustMother.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DustMother.App
{
    [ViewModelProperty(nameof(SettingsSave.CockpitFOV), PendingChanges = nameof(PendingChanges))]
    [ViewModelProperty(nameof(SettingsSave.ThirdPersonFOV), PendingChanges = nameof(PendingChanges))]
    [ViewModelProperty(nameof(SettingsSave.MetricHUDUnits), PendingChanges = nameof(PendingChanges))]
    [ViewModelProperty(nameof(SettingsSave.ToggleAoA), PendingChanges = nameof(PendingChanges))]
    //[ViewModelProperty(nameof(SettingsSave.ResolutionScale), PendingChanges = nameof(PendingChanges))]
    public partial class GameSettingsViewModel : SaveViewModel<SettingsSave>
    {
        public GameSettingsViewModel()
        {
            base.OnSaveLoad += SaveDataLoaded;
        }

        private void SaveDataLoaded(object sender, SettingsSave data)
        {
            OnPropertyChanged(nameof(CockpitFOV));
            OnPropertyChanged(nameof(ThirdPersonFOV));
            OnPropertyChanged(nameof(ResolutionScale));
            OnPropertyChanged(nameof(MetricHUDUnits));
            OnPropertyChanged(nameof(ToggleAoA));
        }

        public override async Task Refresh()
        {
            await RefreshSaveAsync(r => r.GetSettingsDataAsync(), c => c?.TelemetryFirstPass != null);
        }

        public float? ResolutionScale
        {
            get => SaveData?.ResolutionScale == null ? null : SaveData.ResolutionScale * 100;
            set
            {
                if (SaveData?.ResolutionScale != null && value is float flValue && flValue is not default(float) && Math.Truncate(flValue) != Math.Truncate((float)SaveData.ResolutionScale*100))
                {
                    var rawValue = flValue / 100;
                    SaveData.ResolutionScale = rawValue;
                    PendingChanges = true;
                    OnPropertyChanged();
                }
            }
        }

        public float? InterfaceScale
        {
            get => SaveData?.InterfaceScale == null ? null : SaveData.InterfaceScale * 100;
            set
            {
                if (SaveData?.InterfaceScale != null && value is float flValue && flValue is not default(float) && Math.Truncate(flValue) != Math.Truncate((float)SaveData.InterfaceScale * 100))
                {
                    var rawValue = flValue / 100;
                    SaveData.InterfaceScale = rawValue;
                    PendingChanges = true;
                    OnPropertyChanged();
                }
            }
        }


    }
}
