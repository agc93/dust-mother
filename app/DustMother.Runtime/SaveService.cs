using System;
using System.IO;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Collections;

namespace DustMother.Runtime
{
    public sealed class SaveService : IBackgroundTask
    {
        private BackgroundTaskDeferral backgroundTaskDeferral;
        private AppServiceConnection appServiceconnection;
        private SaveClient _client;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            // Get a deferral so that the service isn't terminated.
            this.backgroundTaskDeferral = taskInstance.GetDeferral();

            // Associate a cancellation handler with the background task.
            taskInstance.Canceled += OnTaskCanceled;

            // Retrieve the app service connection and set up a listener for incoming app service requests.
            var details = taskInstance.TriggerDetails as AppServiceTriggerDetails;
            appServiceconnection = details.AppServiceConnection;
            appServiceconnection.RequestReceived += OnRequestReceived;
            _client = new SaveClient();

        }
        private async void OnRequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            var messageDeferral = args.GetDeferral();
            var message = args.Request.Message;
            var returnData = new ValueSet();

            var saveType = (message["type"] as string)?.ToLower();
            var path = (message["path"] as string)?.ToLower();
            // This function is called when the app service receives a request.
            var fi = GetSaveFile(saveType, path);

            switch (saveType)
            {
                case "campaign":
                    var data = _client.GetCampaignData(fi);
                    returnData.Add("campaign", SummaryBuilder.CreateCampaignSummary(data));
                    break;
                default:
                    break;
            }

            await args.Request.SendResponseAsync(returnData);

            messageDeferral.Complete();

        }

        private FileInfo GetSaveFile(string command, string? path)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                //TODO: handle this;
                return null;
            }
            else
            {
                var localAppData = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                var targetPath = Path.Combine(localAppData, "ProjectWingman", "Saved", "SaveGames");
                return new FileInfo(Path.Combine(targetPath, $"{command}.sav"));
            }
        }

        private void OnTaskCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            if (this.backgroundTaskDeferral != null)
            {
                // Complete the service deferral.
                this.backgroundTaskDeferral.Complete();
            }
        }
    }
}
