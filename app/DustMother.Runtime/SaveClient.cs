using DustMother.Core;
using System.IO;
using UnSave;

namespace DustMother.Runtime
{
    internal sealed class SaveClient
    {
        private readonly SaveSerializer _serializer;

        internal SaveClient()
        {
            _serializer = new SaveSerializerBuilder().AddDefaultSerializers().Build();
        }

        internal CampaignSave GetCampaignData(FileInfo filePath)
        {
            var data = _serializer.ReadFile(filePath);
            var campaign = new CampaignSave(data);
            return campaign;
        }
    }
}
