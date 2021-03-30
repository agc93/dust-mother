using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.Json;
using DustMother.Core;
using Spectre.Console;
using Spectre.Console.Cli;
using UnSave;

namespace DustMother
{
    public class StatisticsCommand : Command<StatisticsCommand.Settings>
    {
        private readonly SaveSerializer _serializer;
        private readonly IAnsiConsole _console;

        public StatisticsCommand(SaveSerializer saveSerializer, IAnsiConsole console)
        {
            _serializer = saveSerializer;
            _console = console;
        }

        public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
        {
            var fi = new FileInfo(settings.StatisticsFilePath);
            var isInteractive = settings.OutputMode == OutputMode.None;
            if (!fi.Exists) {
                throw new FileNotFoundException($"Provided statistics file '{fi.Name}' was not found!");
            }
            try {
                var data = _serializer.ReadFile(fi);
                var stats = new StatisticsSave(data);
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
                        var json = JsonSerializer.Serialize<StatisticsSave>(stats, opts);
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

        private void WriteInfoTable(StatisticsSave save) {
            var infoTable = new Table()
                .LeftAligned()
                .AddColumn("Statistic")
                .AddColumn( new TableColumn("Value").RightAligned());
            if (save?.AircraftKills != null) {
                infoTable.AddRow("[darkorange]Aircraft Kills[/]", save.AircraftKills.ToString());
            }
            if (save?.GroundTargetKills != null) {
                infoTable.AddRow("[green]Ground Targets[/]", save.GroundTargetKills.ToString());
            }
            if (save?.VesselKills != null) {
                infoTable.AddRow("[blue]Vessel Kills[/]", save.VesselKills.ToString());
            }
            if (save?.TotalKills != null) {
                infoTable.AddRow("[bold]Total Kills[/]", $"[bold]{save.TotalKills}[/]");
            }

            var missionTable = new Table()
                .LeftAligned()
                .AddColumn(new TableColumn("Mission ID").LeftAligned())
                .AddColumn(new TableColumn("[green]Easy[/]").Centered())
                .AddColumn(new TableColumn("[blue]Normal[/]").Centered())
                .AddColumn(new TableColumn("[darkorange]Hard[/]").Centered())
                .AddColumn(new TableColumn("[darkred_1]Mercenary[/]").Centered());
            if (save?.Missions != null && save.Missions.Any()) {
                foreach (var mission in save.Missions)
                {
                    missionTable.AddRow(mission.MissionName, 
                        mission.CompletedEasy.ToMark(), 
                        mission.CompletedNormal.ToMark(), 
                        mission.CompletedHard.ToMark(), 
                        mission.CompletedMercenary.ToMark());
                }
            }

            _console.Render(new Rule("General Stats").LeftAligned());
            _console.Render(infoTable);
            _console.Render(new Rule("Mission Completion").LeftAligned());
            _console.Render(missionTable);
            _console.Render(new Rule());
        }

        public class Settings : OutputSettings {
            [CommandArgument(0, "[filePath]")]
            [Description("Path to a statistics file to read")]
            public string StatisticsFilePath {get;set;}
        }
    }
}