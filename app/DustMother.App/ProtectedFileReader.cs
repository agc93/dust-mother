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
    public class ProtectedSaveReader
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
            } catch (UnauthorizedAccessException)
            {
                return false;
            } catch
            {
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
    }
}
