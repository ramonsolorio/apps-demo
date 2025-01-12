using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OxxoPromotionFunctionApp.ListPromotionsResponse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OxxoPromotionFunctionApp
{
/// <summary>
/// Sample Request:
/// 
/// {
///   "CRPlaza": "10LGA",
///   "CRTienda": "50FBH",
///   "source": "POS"
/// }
/// 
/// </summary>
public static class FA_PRM_ListStorePromotions
    {
        [FunctionName("list")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Processing OXXO list promotions request.");
            ListPromotionsResponse.ListPromotionsResponse response = null;

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                ListPromotionsRequest.ListPromotionsRequest data;
                
                try
                {
                    data = JsonConvert.DeserializeObject<ListPromotionsRequest.ListPromotionsRequest>(requestBody);

                    if (string.IsNullOrEmpty(data.CRPlaza) || string.IsNullOrEmpty(data.CRTienda))
                    {
                        throw new ApplicationException("test");
                    }
                }
                catch
                {
                    response = new ListPromotionsResponse.ListPromotionsResponse();
                    response.WMCode = "112";
                    response.WMDesc = "La estructura del request no es la esperada";
                    response.maxFiles = "0";

                    return new BadRequestObjectResult(JsonConvert.SerializeObject(response, Formatting.Indented));
                }

                List<Document> documents = new List<Document>();

                SqlConnectionStringBuilder SqlBuilder = new SqlConnectionStringBuilder(Environment.GetEnvironmentVariable("OxxoSqlConnectionString", EnvironmentVariableTarget.Process));
                string oxxoPromotionStorageConnectionString = Environment.GetEnvironmentVariable("OxxoPromotionStorageConnectionString", EnvironmentVariableTarget.Process);
                string oxxoMappedJsonBlobContainer = Environment.GetEnvironmentVariable("OxxoMappedJsonBlobContainer", EnvironmentVariableTarget.Process);

                using (SqlConnection connection = new SqlConnection(SqlBuilder.ConnectionString))
                {
                    connection.Open();

                    // ToDo: Add "and SOURCE = '{data.source}'" to query
                    string sqlQuery = $"SELECT * FROM [dbo].[CO_PRM_DCM_STS] WITH (NOLOCK) WHERE CR_PLAZA = '{data.CRPlaza}' and CR_TIENDA = '{data.CRTienda}'";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            documents = new List<Document>();
                            response = new ListPromotionsResponse.ListPromotionsResponse();
                            response.WMCode = "101";
                            response.WMDesc = "Acción ejecutada correctamente";
                            response.maxFiles = "5";

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

                                // ToDo: this needs to be reviewed.
                                BlobClient jsonBlobClient = new BlobClient(oxxoPromotionStorageConnectionString, oxxoMappedJsonBlobContainer, currentKeyValuePairs["PV_DOC_NAME"].ToString().Replace(".gz", string.Empty));

                                if (jsonBlobClient.Exists())
                                {
                                    documents.Add(new Document()
                                    {
                                        PVMime = "file/zip",
                                        PVDocName = currentKeyValuePairs["PV_DOC_NAME"].ToString(),
                                        PVDocType = currentKeyValuePairs["PV_DOC_TYPE"].ToString(),
                                        PVSize = ((BlobProperties)jsonBlobClient.GetProperties()).ContentLength.ToString(),
                                        PVRecords = currentKeyValuePairs["NUMBER_OF_RECORDS"].ToString(),
                                        PVPriority = "1"
                                    });
                                }
                            }

                            response.documents = documents.ToArray();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response = new ListPromotionsResponse.ListPromotionsResponse();
                response.WMCode = "199";
                response.WMDesc = "Excepción General";
                response.maxFiles = "0";

                return new BadRequestObjectResult(JsonConvert.SerializeObject(response, Formatting.Indented));
            }

            if (response.documents.Count() == 0)
            {
                response.WMCode = "116";
                response.WMDesc = "No hay documentos por enviar";
            }

            return new OkObjectResult(JsonConvert.SerializeObject(response, Formatting.Indented));
        }
    }
}
