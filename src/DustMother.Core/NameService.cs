using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DustMother.Core
{
    public static class NameService
    {
        private static Dictionary<string, string> _specialNames = new Dictionary<string, string>
        {
            ["mig21_1"] = "T-21"
        };
        public static string GetAircraftName(string input, bool useDefault = true)
        {
            if (_specialNames.TryGetValue(input, out var specialName))
            {
                return specialName;
            }
            try
            {
                var nameParts = input.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                var codeParts = nameParts.Where(a => a.All(char.IsDigit)).ToList();
                var trimmedParts = codeParts.Count > 1 ? codeParts.SkipLast(1) : new List<string>();
                return string.Join("-", nameParts.Where(a => !a.All(char.IsDigit)).Concat(trimmedParts).Select(p => p.ToUpper()));
            } catch
            {
                return input.ToUpper();
            }
            
        }

        public static string GetObjectiveType(string input, string defaultValue = null)
        {
            var knownNames = new Dictionary<string, string>
            {
                ["NewEnumerator1"] = "AA Suppression",
                ["NewEnumerator4"] = "Fleet Hunt",
                ["NewEnumerator5"] = "Transport Hunt",
                ["NewEnumerator6"] = "Experimental Unit",
                ["NewEnumerator7"] = "Most Wanted",
                ["NewEnumerator9"] = "Score Attack"
            };
            var name = input.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries).Last();
            if (knownNames.TryGetValue(name, out var objName))
            {
                return objName;
            } else
            {
                return defaultValue ?? $"Unknown ({new string(name.Where(char.IsDigit).ToArray())}";
            }
        }
    }
}
