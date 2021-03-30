using System.Linq;
using System.Text.Json;
using DustMother.Core;
using Spectre.Console;
using static DustMother.Core.Constants;

namespace DustMother
{
    public static class OutputExtensions
    {
        public static void WriteInfo(this CampaignSave save, OutputMode mode, IAnsiConsole console) {
            switch (mode)
            {
                
                case OutputMode.Json:
                    WriteInfoJson(save, console);
                    break;
                case OutputMode.Text:
                    WriteInfoTable(save, console);
                    break;
                case OutputMode.None:
                default:
                    break;
            }
        }

        public static void WriteInfoJson(this CampaignSave save, IAnsiConsole console) {
            var settings = new JsonSerializerOptions {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize<CampaignSave>(save, settings);
            console.WriteLine(json);
        }

        public static void WriteInfoTable(this CampaignSave save, IAnsiConsole console) {
            var infoTable = new Table();
            infoTable.AddColumn(new TableColumn("").LeftAligned());
            infoTable.AddColumn(new TableColumn("Value").RightAligned());
            infoTable.AddRow("Credits", save.Credits.ToString());
            infoTable.AddRow("Campaign", save.CampaignCompleted == true ? "Completed" : "Not Completed");
            infoTable.AddRow("Furthest Mission", save.FurthestMission);
            infoTable.AddRow("Current Mission", save.CurrentCampaign?.CurrentMission);
            infoTable.AddRow("Current Difficulty", Difficulties[save.CurrentCampaign.Difficulty ?? 0]);

            var aircraftTable = new Table();
            aircraftTable.AddColumn(new TableColumn("Name").LeftAligned());
            aircraftTable.AddColumn(new TableColumn("Unlocked").Centered());
            aircraftTable.AddColumn(new TableColumn("Purchased").Centered());
            foreach (var aircraft in save.UnlockedAircraft.Where(a => a.Available == true))
            {
                aircraftTable.AddRow(
                    new Markup($"[bold]{aircraft.Id}[/]"), 
                    new Markup(aircraft.Unlocked.GetValueOrDefault(false) ? "[blue]Yes[/]" : "[orange]No[/]"), 
                    new Markup(aircraft.Purchased.GetValueOrDefault(false) ? "[green]Yes[/]" : "[orangered]No[/]")
                    );
            }
            console.Render(new Rule("Campaign Data") { Alignment = Justify.Left });
            console.Render(infoTable);
            console.Render(new Rule("Aircraft") { Alignment = Justify.Left });
            console.Render(aircraftTable);
        }

        public static string ToMark(this bool? value, bool useSymbol = true) {
            if (useSymbol) {
            return value.HasValue ? value.Value ? "[green]✔[/]" : "❌" : "❓";
            } else {
                return Emoji.Replace(value.HasValue ? value.Value ? ":check_mark" : "cross_mark" : ":question_mark:"); 
            }
        }
    }
}