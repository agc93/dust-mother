using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnSave;

namespace DustMother.Core
{
    public class SaveFileReader
    {
        private readonly SaveSerializer _serializer;

        public SaveFileReader()
        {
            _serializer = new SaveSerializerBuilder().AddDefaultSerializers().Build();
        }

        public SaveFileReader(SaveSerializer serializer)
        {
            _serializer = serializer;
        }

        public CampaignSave GetCampaignData(FileInfo filePath)
        {
            var data = _serializer.ReadFile(filePath);
            var campaign = new CampaignSave(data);
            return campaign;
        }

        public ConquestSave GetConquestData(FileInfo filePath)
        {
            var data = _serializer.ReadFile(filePath);
            var conquest = new ConquestSave(data);
            return conquest;
        }

        public StatisticsSave GetStatisticsData(FileInfo filePath)
        {
            var data = _serializer.ReadFile(filePath);
            var stats = new StatisticsSave(data);
            return stats;
        }

        public SettingsSave GetSettingsData(FileInfo filePath)
        {
            var data = _serializer.ReadFile(filePath);
            var settings = new SettingsSave(data);
            return settings;
        }

        public FileInfo GetSaveFile(string saveName, string basePath = null)
        {
            saveName = Path.GetFileNameWithoutExtension(saveName);
            string rootPath;
            if (!string.IsNullOrWhiteSpace(basePath))
            {
                //TODO: handle this;
                rootPath = new DirectoryInfo(basePath).Name == "SaveGames"
                    ? basePath
                    : Path.Combine(basePath, "Saved", "SaveGames");
            } else
            {
                var localAppData = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                rootPath = Path.Combine(localAppData, "ProjectWingman", "Saved", "SaveGames");
            }
            return new FileInfo(Path.Combine(rootPath, $"{saveName}.sav"));
        }

        public FileInfo WriteSave(WingmanSave save) {

            var savePath = string.IsNullOrWhiteSpace(save?.FileName) && new FileInfo(save?.FileName).Exists
                ? save switch {
                    CampaignSave {} => GetSaveFile("Campaign"),
                    ConquestSave {} => GetSaveFile("Conquest"),
                    StatisticsSave {} => GetSaveFile("stat"),
                    SettingsSave {} => GetSaveFile("savegame"),
                    _ => throw new ArgumentException("Unknown save type")
                }
                : GetSaveFile(save.FileName);
            _serializer.WriteToFile(save, savePath);
            return savePath;
        }

    }
}
