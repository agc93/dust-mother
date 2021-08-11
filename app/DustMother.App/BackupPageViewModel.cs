using DustMother.App.TypeHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DustMother.App
{
    public class BackupPageViewModel : DispatcherBase
    {
        private readonly BackupManager _manager = new BackupManager();

        //public ObservableCollection<string> BackupTimestamps { get; set; } = new ObservableCollection<string>();

        public ObservableDictionary<string, IEnumerable<FileSummary>> AvailableBackups { get; set; } = new ObservableDictionary<string, IEnumerable<FileSummary>>();

        public BackupPageViewModel()
        {

        }

        public async Task BackupSave(string saveType)
        {
            if (saveType == "*")
            {
                await _manager.BackupAllSaves();
            }
            else
            {
                await _manager.BackupSaveFile(saveType);
            }
        }

        public async Task GetAvailableSaves()
        {
            var files = await _manager.GetAvailableSaves();
            AvailableBackups.Clear();
            foreach (var availableFile in files.GroupBy(f => Path.GetFileNameWithoutExtension(f.Name).Split('.').First()))
            {
                var fileList = new List<FileSummary>();
                foreach (var af in availableFile)
                {
                    var props = await af.GetBasicPropertiesAsync();
                    fileList.Add(new FileSummary(af, props));
                }
                AvailableBackups.Add(availableFile.Key, fileList);
            }
        }

        public async void HandleBackupAll(object sender)
        {
            await BackupSave("*");
        }
    }
}
