using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using System.Net.Http;
using System.Collections.Generic;
using Microsoft.Azure.Documents;

namespace AuditTrail.Feature.AuditTrail.AzureFunctions.SignalR
{
    public static class GeneralHub
    {
        [FunctionName("GeneralHubSubscribe")]
        public static SignalRConnectionInfo GetSignalRInfo(
            [HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequestMessage req,
            [SignalRConnectionInfo(HubName = "General")] SignalRConnectionInfo connectionInfo)
        {
            return connectionInfo;
        }

        // called when cosmos changes
        [FunctionName("GeneralHubUpdate")]
        public static async Task SendMessage(
            [CosmosDBTrigger(
            databaseName: "audit-trail",
            collectionName: "audit-records",
            ConnectionStringSetting = "COSMOS_CONNECTION_STRING",
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> documents,
            [SignalR(HubName = "General")] IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            log.LogInformation("Cosmos Trigger!!");
            await signalRMessages.AddAsync(
                new SignalRMessage
                {
                    Target = "GeneralEventUpdate", // the name of this value corresponds with the function called from the client using JavaScript
                    Arguments = new object[] { JsonConvert.SerializeObject(documents) }
                });
        }
    }
}
