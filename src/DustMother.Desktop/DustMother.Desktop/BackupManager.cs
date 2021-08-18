using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace DustMother.App
{
    public class BackupManager : ProtectedSaveReader
    {
        public async Task BackupSaveFile(string saveName)
        {
            saveName = Path.GetFileNameWithoutExtension(saveName);
            var saveFilePath = GetSavePath(saveName);
            var folder = await StorageFolder.GetFolderFromPathAsync(GetFolderPath());
            var d = DateTime.Now;
            var fileName = $"{saveName}.{d:yyyyMMddHHmm}.sav";
            var file = await GetFile(saveFilePath);
            await file.CopyAsync(folder, fileName, NameCollisionOption.GenerateUniqueName);
        }

        public async Task BackupAllSaves()
        {
            foreach (var saveFile in new[] { "Campaign", "Conquest", "stat", "savegame" })
            {
                await BackupSaveFile(saveFile);
            }
        }

        public string SaveLocation => GetFolderPath();

        public async Task<IEnumerable<StorageFile>> GetAvailableSaves()
        {
            var folder = await StorageFolder.GetFolderFromPathAsync(GetFolderPath());
            var files = await folder.GetFilesAsync(Windows.Storage.Search.CommonFileQuery.DefaultQuery);
            var matches = files.Where(f => f.Name.Split('.').Any(seg => seg.All(char.IsDigit)));
            return matches.ToList();
        }
    }
}
