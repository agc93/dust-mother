using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DustMother.App
{
    internal static class PropertyValueConverter
    {
        internal static class Difficulty
        {
            internal static List<string> Names => new List<string> { "Easy", "Normal", "Hard", "Mercenary" };
            internal static string FromValue(int? value)
            {
                return value switch
                {
                    0 => "Easy",
                    1 => "Normal",
                    2 => "Hard",
                    3 => "Mercenary",
                    _ => "Unknown"
                };
            }

            internal static int? ToValue(string? name)
            {
                return name is string strValue
                    ? strValue.ToLower() switch
                    {
                        "easy" => 0,
                        "normal" => 1,
                        "hard" => 2,
                        "mercenary" => 3,
                        _ => null
                    }
                    : null;
            }
        }
        
    }
}
