using DustMother.Core;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnSave;
using Windows.Storage;
using Windows.Storage.Streams;

namespace DustMother.App
{
    public class ProtectedSaveReader : ISaveReader
    {
        private readonly SaveSerializer _serializer;

        public ProtectedSaveReader()
        {
            _serializer = new SaveSerializerBuilder().AddDefaultSerializers().Build();
        }

        public ProtectedSaveReader(SaveSerializer serializer)
        {
            _serializer = serializer;
        }

        public async Task<bool?> CheckFileAccessAsync()
        {
            try
            {
                var file = await GetFile(GetSavePath("campaign"));
                return file.IsAvailable;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
            catch
            {
#if DEBUG
                System.Console.WriteLine("Unhandled file access error");
#endif
                return null;
            }
        }

        public async Task<CampaignSave> GetCampaignData()
        {
            var file = await GetFile(GetSavePath("campaign"));
            var data = _serializer.Read(await file.OpenStreamForReadAsync());
            var campaign = new CampaignSave(data);
            return campaign;
        }

        public async Task<ConquestSave> GetConquestDataAsync()
        {
            var file = await GetFile(GetSavePath("conquest"));
            var data = _serializer.Read(await file.OpenStreamForReadAsync());
            var conquest = new ConquestSave(data);
            return conquest;
        }

        public async Task<StatisticsSave> GetStatisticsDataAsync()
        {
            var file = await GetFile(GetSavePath("stat"));
            var data = _serializer.Read(await file.OpenStreamForReadAsync());
            var stats = new StatisticsSave(data);
            return stats;
        }

        public async Task<SettingsSave> GetSettingsDataAsync()
        {
            var file = await GetFile(GetSavePath("savegame"));
            var data = _serializer.Read(await file.OpenStreamForReadAsync());
            var settings = new SettingsSave(data);
            return settings;
        }

        protected async Task<StorageFile> GetFile(string path)
        {
            var file = await StorageFile.GetFileFromPathAsync(path);
            return file;
        }

        protected async Task<StorageFolder> GetFolder(string path)
        {
            var file = await StorageFolder.GetFolderFromPathAsync(path);
            return file;
        }

        protected string GetSavePath(string saveName, string basePath = null)
        {
            saveName = Path.GetFileNameWithoutExtension(saveName);
            var rootPath = GetFolderPath(basePath);
            return Path.Combine(rootPath, $"{saveName}.sav");
        }

        protected string GetFolderPath(string basePath = null)
        {
            string rootPath;
            if (!string.IsNullOrWhiteSpace(basePath))
            {
                //TODO: handle this;
                rootPath = new DirectoryInfo(basePath).Name == "SaveGames"
                    ? basePath
                    : Path.Combine(basePath, "Saved", "SaveGames");
            }
            else
            {
                //var localAppData = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                var localAppData = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AppData", "Local");
                rootPath = Path.Combine(localAppData, "ProjectWingman", "Saved", "SaveGames");
            }
            return rootPath;
        }

        public virtual async Task<bool> WriteSaveAsync(WingmanSave save)
        {

            var savePath = string.IsNullOrWhiteSpace(save?.FileName) && new FileInfo(save?.FileName).Exists
                ? save switch
                {
                    CampaignSave { } => GetSavePath("Campaign"),
                    ConquestSave { } => GetSavePath("Conquest"),
                    StatisticsSave { } => GetSavePath("stat"),
                    SettingsSave { } => GetSavePath("savegame"),
                    _ => throw new ArgumentException("Unknown save type")
                }
                : GetSavePath(save.FileName);
            var tmpFilePath = Path.ChangeExtension(savePath, ".tmp");
            var folderPath = GetFolderPath();
            var folder = await GetFolder(folderPath);
            var tmpFile = await folder.CreateFileAsync(Path.GetFileName(tmpFilePath), CreationCollisionOption.ReplaceExisting);
            try
            {
                var outStream = await tmpFile.OpenAsync(FileAccessMode.ReadWrite, StorageOpenOptions.AllowOnlyReaders);
                _serializer.Write(outStream.AsStreamForWrite(), save);
                await outStream.FlushAsync();
                outStream.Dispose();
            }
            catch
            {
                //TODO: handle this
            }
            var props = await tmpFile.GetBasicPropertiesAsync();
            if (props.Size > 0)
            {
                var targetFile = await GetFile(savePath);
                await tmpFile.CopyAndReplaceAsync(targetFile);
                return true;
            }

            return false;

            //_serializer.WriteToFile(save, savePath);
            //return Task.CompletedTask;
        }
    }
}
