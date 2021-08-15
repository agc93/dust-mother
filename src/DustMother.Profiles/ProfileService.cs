using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DustMother.Profiles
{
    public class ProfileService
    {
        private readonly IPathOptions _pathOptions;

        public ProfileService(IPathOptions pathOptions) {
            _pathOptions = pathOptions;
        }
        private static string[] SaveFiles = new[] { "Campaign", "Conquest", "savegame", "stat" };

        public IEnumerable<string> GetProfiles() {
            var root = _pathOptions.SaveFilePath;
            var allSaves = new DirectoryInfo(root).GetFiles("savegame*.sav");
            var names = allSaves
                .Select(f => Path.GetFileNameWithoutExtension(f.Name))
                .Select(f => f.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries))
                .Where(c => c.Length > 1);
            return names.Select(n => n.Last());
        }

        public string SaveCurrentToProfile(string profileName, bool overwrite = false) {
            return null;
        }
    }
}