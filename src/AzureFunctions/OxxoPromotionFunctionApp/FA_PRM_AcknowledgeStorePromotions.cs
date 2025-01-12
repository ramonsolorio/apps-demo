using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using OxxoPromotionFunctionApp.AcknowledgeStorePromotionsRequest;
using OxxoPromotionFunctionApp.AcknowlegePromotionsResponse;
using OxxoPromotionFunctionApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OxxoPromotionFunctionApp
{
    /// <summary>
    /// {
    ///   "CRPlaza": "10LGA",
    ///   "CRTienda": "50FBH",
    ///   "source": "POS"
    ///   "documents": [
    ///     {
    ///       "PVDocName": "PRM10LGA50FBH20241210T0107324928056Z_dab0f1ca-538b-4d26-9886-e627316ed7d7.json",
    ///       "PVDocType": "PRM",
    ///       "PVStatus": "R",
    ///       "PVEventDate": "12/02/2024 18:47:30"
    ///     }
    ///   ]
    /// }
    /// </summary>
    public static class FA_PRM_AcknowledgeStorePromotions
    {
        [FunctionName("ack")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Processing OXXO promotions acknowledgement request.");
            string oxxoPromotionStorageConnectionString = Environment.GetEnvironmentVariable("OxxoPromotionStorageConnectionString", EnvironmentVariableTarget.Process);
            string oxxoMappedJsonBlobContainer = Environment.GetEnvironmentVariable("OxxoMappedJsonBlobContainer", EnvironmentVariableTarget.Process);
            string oxxoWorkflowAckUri = Environment.GetEnvironmentVariable("OxxoWorkflowAckUri", EnvironmentVariableTarget.Process);
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            AcknowledgePromotionsRequest data;
            AcknowledgePromotionsResponse response = new AcknowledgePromotionsResponse()
            {
                WMCode = "101",
                WMDesc = "Acción ejecutada correctamente"
            };

            try
            {
                data = JsonSerializer.Deserialize<AcknowledgePromotionsRequest>(requestBody);

                if (string.IsNullOrEmpty(data.CRPlaza) || string.IsNullOrEmpty(data.CRTienda))
                {
                    throw new ApplicationException("test");
                }
            }
            catch
            {
                response.WMCode = "112";
                response.WMDesc = "La estructura del request no es la esperada";

                return new BadRequestObjectResult(JsonSerializer.Serialize(response));
            }

            try
            {
                foreach (var document in data.documents)
                {
                    // Download the blob
                    BlobClient jsonBlobClient = new BlobClient(oxxoPromotionStorageConnectionString, oxxoMappedJsonBlobContainer, document.PVDocName.Replace(".gz", string.Empty));

                    if (jsonBlobClient.Exists())
                    {
                        jsonBlobClient.Delete();
                    }
                    else
                    {
                        response.WMCode = "116";
                        response.WMDesc = "No hay documentos por enviar";

                        return new OkObjectResult(JsonSerializer.Serialize(response));
                    }

                    // Research: https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient
                    WorkflowAckRequest workflowAckRequest = new WorkflowAckRequest()
                    {
                        CRPlaza = data.CRPlaza,
                        CRTienda = data.CRTienda,
                        PVDocName = document.PVDocName,
                        PVDocType = document.PVDocType,
                        PVStatus = document.PVStatus,
                        PVEventDate = document.PVEventDate
                    };

                    string workflowRequest = JsonSerializer.Serialize(workflowAckRequest);

                    log.LogInformation($"Workflow request: {workflowAckRequest}");

                    HttpClient client = new HttpClient();
                    using StringContent jsonContent = new(
                        workflowRequest,
                        Encoding.UTF8,
                        "application/json");

                    await client.PostAsync(oxxoWorkflowAckUri, jsonContent);
                }
            }
            catch (Exception ex)
            {
                response = new AcknowledgePromotionsResponse();
                response.WMCode = "199";
                response.WMDesc = "Excepción General";

                return new BadRequestObjectResult(JsonSerializer.Serialize(response));
            }

            return new OkObjectResult(JsonSerializer.Serialize(response));
        }
    }
}
