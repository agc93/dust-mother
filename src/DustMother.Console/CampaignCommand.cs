using System;
using System.IO;
using System.Threading.Tasks;
using DustMother.Core;
using Spectre.Console;
using Spectre.Console.Cli;
using UnSave;

namespace DustMother
{
    public class CampaignCommand : AsyncCommand<CampaignCommand.Settings> {
        private readonly SaveSerializer _serializer;
        private readonly IAnsiConsole _console;

        public CampaignCommand(SaveSerializer saveSerializer, IAnsiConsole console)
        {
            _serializer = saveSerializer;
            _console = console;
        }
        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
        {
            var fi = new FileInfo(settings.SaveFilePath);
            var isInteractive = settings.OutputMode == OutputMode.None;
            if (!fi.Exists || fi.Extension != ".sav" ) {
                throw new FileNotFoundException($"Provided campaign file path '{settings.SaveFilePath}' does not exist!");
            }
            try {
                var data = _serializer.ReadFile(fi);
                var campaign = new CampaignSave(data);
                if (isInteractive) {
                    _console.MarkupLine($"[bold green]Success![/] Loaded save data from [dim grey]{fi.Name}[/]");
                }
                campaign.WriteInfo(settings.OutputMode, _console);
                return 0;
            } catch (Exception ex) {
                _console.WriteException(ex);
                return 500;
            }
            
            
        }

        public class Settings : SaveCommandSettings
        {
            public Settings() : base("Campaign")
            {
            }
        }
    }
}