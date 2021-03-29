using System.ComponentModel;
using Spectre.Console.Cli;

namespace DustMother
{
    public enum OutputMode {
        None,
        Text,
        Json
    }
    public class OutputSettings : CommandSettings
    {
        [CommandOption("-o|--output <mod>")]
        [Description("Specify the output format for parsed data.")]
        public OutputMode OutputMode {get;set;} = OutputMode.None;
    }
}