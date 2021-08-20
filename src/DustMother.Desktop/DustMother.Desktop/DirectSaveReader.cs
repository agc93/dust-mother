using DustMother.Core;
using System;
using System.IO;
using System.Threading.Tasks;
using UnSave;

namespace DustMother.App
{
    public class DirectSaveReader : ISaveReader
    {
        private readonly SaveSerializer _serializer;

        public DirectSaveReader()
        {
            _serializer = new SaveSerializerBuilder().AddDefaultSerializers().Build();
        }

        public DirectSaveReader(SaveSerializer serializer)
        {
            _serializer = serializer;
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

        protected async Task<FileInfo?> GetFile(string path)
        {
            var file = new FileInfo(path);
            return file.Exists ? file : null;
        }

        protected async Task<DirectoryInfo?> GetFolder(string path)
        {
            var di = new DirectoryInfo(path);

            return di.Exists ? di : null;
        }

        public async Task<bool?> CheckFileAccessAsync()
        {
            try
            {
                var file = await GetFile(GetSavePath("campaign"));
                return file != null &&  file.Exists && file.Length > 0;
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
            var data = _serializer.Read(file.OpenRead());
            var campaign = new CampaignSave(data);
            return campaign;
        }

        public async Task<ConquestSave> GetConquestDataAsync()
        {
            var file = await GetFile(GetSavePath("conquest"));
            var data = _serializer.Read(file.OpenRead());
            var conquest = new ConquestSave(data);
            return conquest;
        }

        public async Task<SettingsSave> GetSettingsDataAsync()
        {
            var file = await GetFile(GetSavePath("savegame"));
            var data = _serializer.Read(file.OpenRead());
            var stats = new SettingsSave(data);
            return stats;
        }

        public async Task<StatisticsSave> GetStatisticsDataAsync()
        {
            try
            {
                var file = await GetFile(GetSavePath("stat"));
                var data = _serializer.Read(file.OpenRead());
                var stats = new StatisticsSave(data);
                return stats;
            } catch (Exception e)
            {
                File.WriteAllText(Path.Combine(Path.GetTempPath(), $"ex_{DateTime.UtcNow.Ticks}.txt"), string.Join(Environment.NewLine, new[] { e.ToString(), e.StackTrace?.ToString() }));
                throw;
            }
        }

        public async Task<bool> WriteSaveAsync(WingmanSave save)
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
            var tmpFilePath = Path.GetTempFileName();
            var folderPath = GetFolderPath();
            var folder = await GetFolder(folderPath);
            var tmpFile = new FileInfo(tmpFilePath);
            try
            {
                var outStream = tmpFile.Open(FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
                _serializer.Write(outStream, save);
                await outStream.FlushAsync();
                outStream.Dispose();
            }
            catch
            {
                //TODO: handle this
            }
            tmpFile.Refresh();
            if (tmpFile.Length > 0)
            {
                var targetFile = await GetFile(savePath);
                tmpFile.CopyTo(targetFile.FullName, true);
                tmpFile.Delete();
                return true;
            }

            return false;
        }
    }
}
