using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Text.Json;
using DustMother.Core;
using Spectre.Console;
using Spectre.Console.Cli;
using UnSave;

namespace DustMother
{
    public class GameSettingsCommand : Command<GameSettingsCommand.Settings>
    {
        private readonly SaveSerializer _serializer;
        private readonly IAnsiConsole _console;

        public GameSettingsCommand(SaveSerializer saveSerializer, IAnsiConsole console)
        {
            _serializer = saveSerializer;
            _console = console;
        }
        
        public class Settings : OutputSettings {
            [CommandArgument(0, "[filePath]")]
            [Description("Path to a statistics file to read")]
            public string SettingsFilePath {get;set;}
        }

        public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
        {
            var fi = new FileInfo(settings.SettingsFilePath);
            var isInteractive = settings.OutputMode == OutputMode.None;
            if (!fi.Exists) {
                throw new FileNotFoundException($"Provided statistics file '{fi.Name}' was not found!");
            }
            try {
                var data = _serializer.ReadFile(fi);
                var stats = new SettingsSave(data);
                if (isInteractive) {
                    _console.MarkupLine($"[bold green]Success![/] Loaded save data from [italic]{fi.Name}[/]");
                }
                switch (settings.OutputMode)
                {
                    case OutputMode.Json:
                        var opts = new JsonSerializerOptions {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                            WriteIndented = true
                        };
                        var json = JsonSerializer.Serialize<SettingsSave>(stats, opts);
                        _console.WriteLine(json);
                        break;
                    case OutputMode.None:
                    case OutputMode.Text:
                        WriteInfoTable(stats);
                        break;
                    default:
                        throw new ArgumentException("Invalid output type!");
                }
                return 0;
            } catch (Exception ex) {
                _console.WriteException(ex);
                return 500;
            }
        }

        private void WriteInfoTable(SettingsSave save) {
            var infoTable = new Table()
                .LeftAligned()
                .AddColumn("Setting")
                .AddColumn(new TableColumn("Value").RightAligned());
            if (save?.TelemetryAccepted != null) {
                infoTable.AddRow("Telemetry Enabled", save.TelemetryAccepted.Value.ToString());
            }

            if (save?.ToggleAoA != null) {
                infoTable.AddRow("AoA Toggle Mode", save.ToggleAoA.Value.ToString());
            }

            if (save?.CockpitFOV != null) {
                infoTable?.AddRow("Cockpit FOV", save.CockpitFOV.Value.ToString(CultureInfo.CurrentCulture));
            }

            if (save?.ThirdPersonFOV != null) {
                infoTable.AddRow("External FOV", save.ThirdPersonFOV.Value.ToString(CultureInfo.CurrentCulture));
            }
            
            _console.Render(new Rule("Game Settings").LeftAligned());
            _console.Render(infoTable);
        }
    }
}