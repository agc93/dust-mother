using DustMother.App.TypeHelpers;
using Humanizer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.System;

namespace DustMother.App
{
    public class BackupPageViewModel : BindableBase
    {
        private readonly BackupManager _manager = new BackupManager();
        private bool isRunning;

        //public ObservableCollection<string> BackupTimestamps { get; set; } = new ObservableCollection<string>();

        public ObservableDictionary<string, IEnumerable<FileSummary>> AvailableBackups { get; set; } = new ObservableDictionary<string, IEnumerable<FileSummary>>();

        public ObservableCollection<BackupSummary> AllBackups { get; set; } = new();


        public BackupPageViewModel()
        {

        }

        public async Task BackupSave(string saveType)
        {
            IsRunning = true;
            if (saveType == "*")
            {
                await _manager.BackupAllSaves();
            }
            else
            {
                await _manager.BackupSaveFile(saveType);
            }
            await GetAvailableSaves();
            IsRunning = false;
        }

        public async Task GetAvailableSaves()
        {
            var files = await _manager.GetAvailableSaves();
            var backups = new List<BackupSummary>();
            AvailableBackups.Clear();
            AllBackups.Clear();
            foreach (var availableFile in files.GroupBy(f => Path.GetFileNameWithoutExtension(f.Name).Split('.').First()))
            {
                var fileList = new List<FileSummary>();
                foreach (var af in availableFile)
                {
                    var props = await af.GetBasicPropertiesAsync();
                    var fs = new BackupSummary(af, props);
                    fileList.Add(fs);
                    backups.Add(fs);
                }
                AvailableBackups.Add(availableFile.Key, fileList);
            }
            foreach (var backup in backups.OrderByDescending(b => b.Created))
            {
                AllBackups.Add(backup);
            }
            OnPropertyChanged(nameof(AvailableBackups));
            OnPropertyChanged(nameof(AllBackups));
        }

        public async void HandleBackupAll(object sender)
        {
            await BackupSave("*");
        }

        public async Task<bool> OpenSaveFolder()
        {
            return await Launcher.LaunchFolderPathAsync(_manager.SaveLocation);
        }

        public bool IsRunning { get => isRunning; set { isRunning = value; OnPropertyChanged(); } }

        public static string GetTimeLabel(BackupSummary bs) {
            return $"{bs.OriginalName} (created {bs.Created.Humanize(DateTimeOffset.Now)})";
        }
    }
}
