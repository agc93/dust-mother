using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace DustMother.App.TypeHelpers
{
    public class FileSummary
    {
        public FileSummary(StorageFile file)
        {
            _file = file;
        }

        public FileSummary(StorageFile file, BasicProperties props)
        {
            _props = props;
        }

        private StorageFile _file;
        private BasicProperties _props;

        public DateTimeOffset? Created => _file?.DateCreated;
        public string Name => System.IO.Path.GetFileNameWithoutExtension(_file.Name);
        public ulong? Size => _props?.Size;
        public string GetTimestamp()
        {
            return _file.Name.Split(':').FirstOrDefault(seg => seg.All(char.IsDigit));
        }
    }
}
