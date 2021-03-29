using System;
using System.Collections.Generic;
using System.Linq;
using DustMother.Core;
using UnSave;
using UnSave.Serialization;
using UnSave.Types;

namespace DustMother
{
    class Program
    {
        static void Main(string[] args)
        {
            var structSerializers = new List<IUnrealStructSerializer> {new UEGuidStructProperty()};
            var builder = new SaveSerializerBuilder();
            var serializer = builder.AddDefaultSerializers().AddCollectionSerializer(new UEStructSerializer(structSerializers)).Build();
            var saveData = serializer.ReadFile(args[0]);
            var campaign = new CampaignSave(saveData);
            if (campaign?.CampaignCompleted == true) {
                Console.WriteLine("Successfully loaded campaign save file");
                Console.WriteLine($"Credits: {campaign.Credits}");
                Console.WriteLine($"Completed up to: {campaign.FurthestMission}");
                Console.WriteLine($"Campaign {(campaign.CampaignActive == true ? "currently" : "not")} active. Progress: {campaign.CurrentCampaign?.CurrentMission} ({campaign.CurrentCampaign.Difficulty} difficulty)");
                Console.WriteLine($"{campaign.UnlockedAircraft.Count(a => a.Purchased == true)} of {campaign.UnlockedAircraft.Count(a => a.Unlocked == true && a.Available == true)} aircraft purchased");
            }
            
            Console.WriteLine("Save parsing complete!");
        }
    }
}
