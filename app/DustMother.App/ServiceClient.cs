using DustMother.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;

namespace DustMother.App
{
    class ServiceClient
    {
        private AppServiceConnection _connection;
        public bool Connected { get; set; }
        public ServiceClient()
        {
            this._connection = new AppServiceConnection
            {
                AppServiceName = "app.dustmother.savedata",
                PackageFamilyName = "b69d084f-6f58-42fc-8df8-4965047586bc_dbcb8bk53qt30"
            };
        }

        public async Task<ServiceClient> Connect()
        {
            var status = await this._connection.OpenAsync();
            Connected = status == AppServiceConnectionStatus.Success;
            return this;
        }

        public async Task<CampaignSaveSummary> GetCampaignSave()
        {
            var msg = new ValueSet();
            msg.Add("type", "Campaign");
            var response = await _connection.SendMessageAsync(msg);
            var result = response.Message["campaign"] as CampaignSaveSummary;
            return result;
        }


    }
}
