using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Data.SqlClient;
using Azure;
using Microsoft.Azure.Amqp.Framing;
using System.Collections.Generic;
using OxxoPromotionFunctionApp.Models;
using Azure.Messaging.ServiceBus;
using System.Text;

namespace OxxoPromotionFunctionApp
{
    public static class FA_PRM_EnqueuePromotion
    {
        [FunctionName("FA_PRM_EnqueuePromotion")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Processing OXXO promotions enqueue promotions request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            EnqueuePromotionsRequest data = JsonConvert.DeserializeObject<EnqueuePromotionsRequest>(requestBody);
            int totalCount = 0;

            try
            {
                SqlConnectionStringBuilder SqlBuilder = new SqlConnectionStringBuilder(Environment.GetEnvironmentVariable("OxxoSqlConnectionString", EnvironmentVariableTarget.Process));
                string oxxoPromotionStorageConnectionString = Environment.GetEnvironmentVariable("OxxoPromotionStorageConnectionString", EnvironmentVariableTarget.Process);
                string serviceBusConnectionString = Environment.GetEnvironmentVariable("OxxoPromotionServiceBusConnectionString", EnvironmentVariableTarget.Process);
                string serviceBusQueuename = data.QUEUE_NAME;

                using (SqlConnection connection = new SqlConnection(SqlBuilder.ConnectionString))
                {
                    connection.Open();

                    string sqlQuery = $"SELECT [BATCH_ID], [LOAD_BATCH_ID], [LOCATION], [LOAD_WEEK] FROM [dbo].[CO_PRM_DAT_HDR] WITH (NOLOCK) WHERE [CLOUD_PRM_STATUS] = 'L'";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        await using var client = new ServiceBusClient(serviceBusConnectionString);
                        // Create a sender for the queue
                        ServiceBusSender sender = client.CreateSender(serviceBusQueuename);
                        ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();
                        int index = 0;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Dictionary<string, object> currentKeyValuePairs = new Dictionary<string, object>();

                                Object[] values = new Object[reader.FieldCount];
                                int resultCount = reader.GetValues(values);

                                for (int i = 0; i < resultCount; i++)
                                {
                                    if (!currentKeyValuePairs.ContainsKey(reader.GetName(i)))
                                    {
                                        currentKeyValuePairs.Add(reader.GetName(i), values[i]);
                                    }
                                }

                                try
                                {
                                    EnqueuePromotion currentRequest = new EnqueuePromotion()
                                    {
                                        BATCH_ID = currentKeyValuePairs["BATCH_ID"].ToString(),
                                        LOAD_BATCH_ID = currentKeyValuePairs["LOAD_BATCH_ID"].ToString(),
                                        LOCATION = currentKeyValuePairs["LOCATION"].ToString(),
                                        LOAD_WEEK = currentKeyValuePairs["LOAD_WEEK"].ToString(),
                                        RUN_IDENTIFIER = data.RUN_IDENTIFIER
                                    };

                                    messageBatch.TryAddMessage(new ServiceBusMessage(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(currentRequest))));
                                    totalCount++;
                                }
                                catch (Exception ex)
                                {
                                    throw new ApplicationException($"Failed to enqueue {data.RUN_IDENTIFIER}.  Exception: {ex.Message}");
                                }

                                index = (index + 1) % 50;
                                if (index == 0)
                                {
                                    await sender.SendMessagesAsync(messageBatch);

                                    messageBatch = await sender.CreateMessageBatchAsync();
                                }
                            }

                            await sender.SendMessagesAsync(messageBatch);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"RunIdentifier: {data.RUN_IDENTIFIER} Failed: {ex.Message}");
            }

            return new OkObjectResult($"Total count: {totalCount}");
        }
    }
}
