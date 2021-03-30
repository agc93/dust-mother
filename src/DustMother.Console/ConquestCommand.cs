using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DustMother.Core;
using Spectre.Console;
using Spectre.Console.Cli;
using UnSave;

namespace DustMother
{
    public class ConquestCommand : AsyncCommand<ConquestCommand.Settings>
    {
        private readonly SaveSerializer _serializer;
        private readonly IAnsiConsole _console;

        public ConquestCommand(SaveSerializer saveSerializer, IAnsiConsole console)
        {
            _serializer = saveSerializer;
            _console = console;
        }
        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
        {
            var fi = new FileInfo(settings.ConquestFilePath);
            var isInteractive = settings.OutputMode == OutputMode.None;
            if (!fi.Exists) {
                throw new FileNotFoundException($"Provided conquest file '{fi.Name}' does not exist!");
            }
            try {
                var data = _serializer.ReadFile(fi);
                var conquest = new ConquestSave(data);
                if (isInteractive) {
                    _console.MarkupLine($"[bold green]Success![/] Loaded save data from [dim grey]{fi.Name}[/]");
                }
                switch (settings.OutputMode)
                {
                    case OutputMode.Json:
                        var opts = new JsonSerializerOptions {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                            WriteIndented = true
                        };
                        var json = JsonSerializer.Serialize<ConquestSave>(conquest, opts);
                        _console.WriteLine(json);
                        break;
                    case OutputMode.None:
                    case OutputMode.Text:
                        WriteInfoTable(conquest);
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

        public class Settings : OutputSettings {
            [CommandArgument(0, "[filePath]")]
            [Description("Path to a conquest save file to read")]
            public string ConquestFilePath {get;set;}
        }

        private void WriteInfoTable(ConquestSave save) {
            var infoTable = new Table();
            infoTable.AddColumn(new TableColumn("Details").LeftAligned());
            infoTable.AddColumn(new TableColumn("").RightAligned());
            if (save.Credits != null) {
                infoTable.AddRow("[blue]Credits[/]", $"[bold]{save.Credits.ToString()}[/]");
            }
            if (save?.TotalScore != null) {
                infoTable.AddRow("Total Score", save.TotalScore.ToString());
            }
            if (save?.AlertLevel != null && save?.AlertLevelProgress != null) {
                infoTable.AddRow(new Markup("[darkred_1]Alert Level[/]"), new Markup($"{save.AlertLevel} ({Math.Round((decimal)(save.AlertLevelProgress * 100), 1)}%)"));
            }
            if (save?.Regions != null && save.Regions.Any()) {
                var unlocked = save.Regions.Count(r => r.Team == 1);
                infoTable.AddRow("Regions", $"{unlocked}/{save.Regions.Count}");
            }

            var persistentTable = new Table();
            persistentTable.AddColumn("Details").LeftAligned();
            persistentTable.AddColumn("").RightAligned();
            if (save?.Prestige != null) {
                persistentTable.AddRow("[darkorange]Prestige[/]", $"[bold]{save.Prestige.ToString()}[/]");
            }
            if (save?.Aircraft != null && save.Aircraft.Any()) {
                var available = save.Aircraft.Where(a => a.Available == true).ToList();
                var unlocked = available.Where(a => a.Unlocked == true && a.Purchased == true);
                persistentTable.AddRow("Aircraft", $"{unlocked.Count()}/{available.Count}");
            }

            var loadoutTable = new Table();
            loadoutTable.AddColumn("").LeftAligned();
            loadoutTable.AddColumn("").RightAligned();
            if (save?.CurrentLoadout?.AircraftName != null) {
                loadoutTable.AddRow("Aircraft", save.CurrentLoadout.AircraftName);
            }
            if (save?.CurrentLoadout?.Weapons != null && save.CurrentLoadout.Weapons.Any()) {
                loadoutTable.AddRow("Weapons", string.Join("[bold]/[/]", save.CurrentLoadout.Weapons.Where(w => w != "EMPTY")));
            }
            if (save?.Allies != null && save.Allies.Any()) {
                var alliesTable = new Table();
                alliesTable.AddColumn("Type").LeftAligned();
                alliesTable.AddColumn("Count").LeftAligned();
                foreach(var ally in save.Allies) {
                    alliesTable.AddRow(ally.Key, ally.Value.ToString());
                }
                if (alliesTable.Rows.Any()) {
                    loadoutTable.AddRow(new Markup("Allies"), alliesTable);
                }
            }
            _console.Render(new Rule("Current Data").LeftAligned());
            _console.Render(infoTable);
            _console.Render(new Rule("Overall Stats").LeftAligned());
            _console.Render(persistentTable.LeftAligned());
            _console.Render(new Rule("Loadout").LeftAligned());
            _console.Render(loadoutTable.LeftAligned());
        }
    }
}