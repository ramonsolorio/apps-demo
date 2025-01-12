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
using System.Collections.Generic;
using OxxoPromotionFunctionApp.ReturnPromotionsRequest;
using OxxoPromotionFunctionApp.ReturnPromotionsResponse;
using Azure.Storage.Blobs;
using System.IO.Compression;
using System.IO.Pipes;

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
    ///       "PVDocType": "PRM"
    ///     }
    ///   ]
    /// }
    /// </summary>
    public static class FA_PRM_GetStorePromotions
    {
        [FunctionName("outbound")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Processing OXXO outbound promotions request.");
            string oxxoPromotionStorageConnectionString = Environment.GetEnvironmentVariable("OxxoPromotionStorageConnectionString", EnvironmentVariableTarget.Process);
            string oxxoMappedJsonBlobContainer = Environment.GetEnvironmentVariable("OxxoMappedJsonBlobContainer", EnvironmentVariableTarget.Process);
            ReturnPromotionsResponse.ReturnPromotionsResponse response = new ReturnPromotionsResponse.ReturnPromotionsResponse();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            ReturnPromotionsRequest.ReturnPromotionsRequest data;

            try
            {
                data = JsonConvert.DeserializeObject<ReturnPromotionsRequest.ReturnPromotionsRequest>(requestBody);

                if (string.IsNullOrEmpty(data.CRPlaza) || string.IsNullOrEmpty(data.CRTienda))
                {
                    throw new ApplicationException("test");
                }
            }
            catch
            {
                response.WMCode = "112";
                response.WMDesc = "La estructura del request no es la esperada";

                return new BadRequestObjectResult(JsonConvert.SerializeObject(response, Formatting.Indented));
            }

            response.documents = new ReturnPromotionsResponse.Document[data.documents.Length];
            List<ReturnPromotionsResponse.Document> documents = new List<ReturnPromotionsResponse.Document>();
            string sqlQuery = string.Empty;
            int responseDocumentIndex = 0;

            try
            {
                response.WMCode = "101";
                response.WMDesc = "Acción ejecutada correctamente";

                foreach (var document in data.documents)
                {
                    ReturnPromotionsResponse.Document doc = new ReturnPromotionsResponse.Document();
                    SqlConnectionStringBuilder SqlBuilder = new SqlConnectionStringBuilder(Environment.GetEnvironmentVariable("OxxoSqlConnectionString", EnvironmentVariableTarget.Process));

                    // Download the blob
                    BlobClient jsonBlobClient = new BlobClient(oxxoPromotionStorageConnectionString, oxxoMappedJsonBlobContainer, document.PVDocName);
                    if (!jsonBlobClient.Exists())
                    {
                        response.WMCode = "117";
                        response.WMDesc = "No se pudieron actualizar los documentos a enviar";

                        return new BadRequestObjectResult(JsonConvert.SerializeObject(response, Formatting.Indented));
                    }

                    using (SqlConnection connection = new SqlConnection(SqlBuilder.ConnectionString))
                    {
                        connection.Open();

                        // ToDo: Add "and SOURCE = '{data.source}'" to query
                        sqlQuery = $"SELECT * FROM [dbo].[CO_PRM_DCM_STS] WITH (NOLOCK) WHERE CR_PLAZA = '{data.CRPlaza}' and CR_TIENDA = '{data.CRTienda}' and SOURCE = '{data.source}' and PV_DOC_NAME = '{document.PVDocName}' and PV_DOC_TYPE = '{document.PVDocType}'";

                        using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (!reader.HasRows)
                                {
                                    response.WMCode = "117";
                                    response.WMDesc = "No se pudieron actualizar los documentos a enviar";

                                    return new BadRequestObjectResult(JsonConvert.SerializeObject(response, Formatting.Indented));
                                }

                                while (reader.Read())
                                {
                                    log.LogInformation($"Generating document fields for {data.CRPlaza}, {data.CRTienda} JSON: {document.PVDocName} Type: {document.PVDocType}");
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

                                    MemoryStream contentStream = new MemoryStream();

                                    try
                                    {
                                        jsonBlobClient.DownloadTo(contentStream);
                                    }
                                    catch
                                    {
                                        response.WMCode = "117";
                                        response.WMDesc = "No se pudieron actualizar los documentos a enviar";

                                        return new BadRequestObjectResult(JsonConvert.SerializeObject(response, Formatting.Indented));
                                    }

                                    byte[] bytes = contentStream.ToArray();

                                    // GZip the contents
                                    MemoryStream zipMS = new MemoryStream();
                                    using (GZipStream zipStream = new GZipStream(zipMS, CompressionMode.Compress, false))
                                    {
                                        zipStream.Write(bytes, 0, bytes.Length);
                                    }

                                    // Encode the JSON contents to base64
                                    string encodedBlobContent = Convert.ToBase64String(zipMS.ToArray());

                                    // Add the base64 contents to the documents List
                                    documents.Add(new ReturnPromotionsResponse.Document()
                                    {
                                        PVMime = "file/zip",
                                        PVDocName = currentKeyValuePairs["PV_DOC_NAME"].ToString(),
                                        PVDocType = currentKeyValuePairs["PV_DOC_TYPE"].ToString(),
                                        PVStatus = "E",
                                        PVData = encodedBlobContent,
                                    });
                                }

                                // Convert the documents List to an array and place in the response message
                                response.documents = documents.ToArray();
                            }
                        }

                        // Move to the next promotion
                        responseDocumentIndex++;
                    }
                }

                // Return the result as JSON
                return new OkObjectResult(JsonConvert.SerializeObject(response, Formatting.Indented));
            }
            catch (Exception ex)
            {
                response = new ReturnPromotionsResponse.ReturnPromotionsResponse();
                response.WMCode = "199";
                response.WMDesc = "Excepción General";

                return new BadRequestObjectResult(JsonConvert.SerializeObject(response, Formatting.Indented));
            }
        }
    }
}
