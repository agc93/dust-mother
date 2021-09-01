using System;
using System.ComponentModel;
using System.IO;
using Spectre.Console;
using Spectre.Console.Cli;

namespace DustMother
{
    public class SaveCommandSettings : OutputSettings
    {
        private string _defaultFileName;

        public SaveCommandSettings(string defaultFileName)
        {
            _defaultFileName = Path.ChangeExtension(defaultFileName, ".sav");
        }

        [CommandArgument(0, "[filePath]")]
        [Description("Path to the save file to read")]
        public string SaveFilePath { get; set; }

        public override ValidationResult Validate()
        {
            if (string.IsNullOrWhiteSpace(SaveFilePath) && !string.IsNullOrWhiteSpace(_defaultFileName))
            {
                
                var savesDirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ProjectWingman", "Saved", "SaveGames");
                var defaultSavePath = Path.Combine(savesDirPath, _defaultFileName);
                if (File.Exists(defaultSavePath))
                {
                    AnsiConsole.MarkupLine($"[grey]Using default save file location: {defaultSavePath}[/]");
                    SaveFilePath = defaultSavePath;
                }
            }
            return base.Validate();
        }
    }
}