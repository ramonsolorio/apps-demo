using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Newtonsoft.Json;
using System.Reflection;
using System.Xml;
using System.Net.Http;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Azure;

namespace OxxoPromotionFunctionApp
{
    public static class FA_PRM_ConvertRawPromotionJsonToXml
    {
        [FunctionName("ConvertRawPromotionJsonToXml")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req, ILogger log)
        {
            string runIdentifier = req.Query["runIdentifier"];
            string uniqueIdentifier = Guid.NewGuid().ToString(); // req.Query["uniqueIdentifier"];
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder(Environment.GetEnvironmentVariable("OxxoSqlConnectionString", EnvironmentVariableTarget.Process));
            string jsonBlobName = $"Promotion_{req.Query["location"]}_{req.Query["batchId"]}_{req.Query["loadBatchId"]}.json";
            string jsonDocumentName = string.Empty;
            string xmlDocument = string.Empty;
            string sqlQuery = string.Empty;

            log.LogInformation($"[runIdentifier]|[uniqueIdentifier]:Converting JSON promotion data to XML");

            try
            {
                string oxxoPromotionStorageConnectionString = Environment.GetEnvironmentVariable("OxxoPromotionStorageConnectionString", EnvironmentVariableTarget.Process);
                string oxxoRawJsonBlobContainer = Environment.GetEnvironmentVariable("OxxoRawJsonBlobContainer", EnvironmentVariableTarget.Process);
                string oxxoRawXmlBlobContainer = Environment.GetEnvironmentVariable("OxxoRawXmlBlobContainer", EnvironmentVariableTarget.Process);
                string deleteJsonBlobAfterTransformation = Environment.GetEnvironmentVariable("OxxoDeleteJsonBlobAfterTransformation", EnvironmentVariableTarget.Process);
                bool deleteJsonBlob = (string.IsNullOrWhiteSpace(deleteJsonBlobAfterTransformation) || (deleteJsonBlobAfterTransformation.ToLower() == "yes")) ? true : false;

                log.LogInformation($"[runIdentifier]|[uniqueIdentifier]:Processing OXXO promotion JSON blob {jsonBlobName}.");

                // Download the blob content
                BlobClient jsonBlobClient = new BlobClient(oxxoPromotionStorageConnectionString, oxxoRawJsonBlobContainer, jsonBlobName);
                MemoryStream contentStream = new MemoryStream();
                jsonBlobClient.DownloadTo(contentStream);
                contentStream.Seek(0, SeekOrigin.Begin);
                var jsonContents = new StreamReader(contentStream).ReadToEnd();

                // Construct the XML output
                StorePromotionData batchData = JsonConvert.DeserializeObject<StorePromotionData>($"{{\"promotionData\": {jsonContents}}}");

                if (batchData.promotionData.Length > 0)
                {
                    log.LogInformation($"[runIdentifier]|[uniqueIdentifier]:Generating XML for {batchData.promotionData[0].ORACLE_CR_SUPERIOR?.ToString()}, {batchData.promotionData[0].ORACLE_CR?.ToString()}");
                    MemoryStream xmlStream = new MemoryStream();

                    XmlWriter xmlWriter = XmlWriter.Create(xmlStream);
                    int rowNumber = 1;

                    // Write the header information
                    xmlWriter.WriteStartElement("PromotionBatch", "rawxml");
                    // Write the unique identifiers
                    xmlWriter.WriteElementString("RUN_IDENTIFIER", "rawxml", runIdentifier);
                    xmlWriter.WriteElementString("UNIQUE_IDENTIFIER", "rawxml", uniqueIdentifier);
                    xmlWriter.WriteElementString("organizationCountry", "rawxml", "MX");
                    xmlWriter.WriteElementString("organizationName", "rawxml", "FEMSA Comercio");
                    xmlWriter.WriteElementString("businessName", "rawxml", "OXXO");
                    xmlWriter.WriteElementString("departmentId", "rawxml", batchData.promotionData[0].ORACLE_CR_SUPERIOR?.ToString());
                    xmlWriter.WriteElementString("storeId", "rawxml", batchData.promotionData[0].ORACLE_CR?.ToString());
                    xmlWriter.WriteElementString("numberOfRecords", "rawxml", batchData.promotionData.Length.ToString());
                    xmlWriter.WriteElementString("searchId", "rawxml", "000000000000000000000000");
                    xmlWriter.WriteElementString("searchDate", "rawxml", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    // Write the Maestro data
                    xmlWriter.WriteElementString("ORACLE_CR", "rawxml", batchData.promotionData[0].ORACLE_CR?.ToString());
                    xmlWriter.WriteElementString("ORACLE_CR_DESC", "rawxml", batchData.promotionData[0].ORACLE_CR_DESC?.ToString());
                    xmlWriter.WriteElementString("ORACLE_CR_SUPERIOR", "rawxml", batchData.promotionData[0].ORACLE_CR_SUPERIOR?.ToString());
                    xmlWriter.WriteElementString("ORACLE_CR_TYPE", "rawxml", batchData.promotionData[0].ORACLE_CR_TYPE?.ToString());
                    xmlWriter.WriteElementString("ESTADO", "rawxml", batchData.promotionData[0].ESTADO?.ToString());
                    xmlWriter.WriteElementString("RETEK_CR", "rawxml", batchData.promotionData[0].RETEK_CR?.ToString());
                    xmlWriter.WriteElementString("RETEK_ASESOR", "rawxml", batchData.promotionData[0].RETEK_ASESOR?.ToString());
                    xmlWriter.WriteElementString("RETEK_ASESOR_NOMBRE", "rawxml", batchData.promotionData[0].RETEK_ASESOR_NOMBRE?.ToString());
                    xmlWriter.WriteElementString("RETEK_DISTRITO", "rawxml", batchData.promotionData[0].RETEK_DISTRITO?.ToString());
                    xmlWriter.WriteElementString("RETEK_PLAZA", "rawxml", batchData.promotionData[0].RETEK_PLAZA?.ToString());
                    xmlWriter.WriteElementString("RETEK_STATUS", "rawxml", batchData.promotionData[0].RETEK_STATUS?.ToString());
                    xmlWriter.WriteElementString("SURH_CR", "rawxml", batchData.promotionData[0].SURH_CR?.ToString());
                    xmlWriter.WriteElementString("SURH_FLAG", "rawxml", batchData.promotionData[0].SURH_FLAG?.ToString());
                    xmlWriter.WriteElementString("CR_FLEX_VALUE_ID", "rawxml", batchData.promotionData[0].CR_FLEX_VALUE_ID?.ToString());
                    xmlWriter.WriteElementString("ORACLE_EF", "rawxml", batchData.promotionData[0].ORACLE_EF?.ToString());
                    xmlWriter.WriteElementString("ORACLE_EF_DESC", "rawxml", batchData.promotionData[0].ORACLE_EF_DESC?.ToString());
                    xmlWriter.WriteElementString("EF_FLEX_VALUE_ID", "rawxml", batchData.promotionData[0].EF_FLEX_VALUE_ID?.ToString());
                    xmlWriter.WriteElementString("ORACLE_CIA", "rawxml", batchData.promotionData[0].ORACLE_CIA?.ToString());
                    xmlWriter.WriteElementString("ORACLE_CIA_DESC", "rawxml", batchData.promotionData[0].ORACLE_CIA_DESC?.ToString());
                    xmlWriter.WriteElementString("CIA_FLEX_VALUE_ID", "rawxml", batchData.promotionData[0].CIA_FLEX_VALUE_ID?.ToString());
                    xmlWriter.WriteElementString("ID_ESTADO_FINANCIERO", "rawxml", batchData.promotionData[0].ID_ESTADO_FINANCIERO?.ToString());
                    xmlWriter.WriteElementString("ID_CENTRO_RESPONSABILIDAD", "rawxml", batchData.promotionData[0].ID_CENTRO_RESPONSABILIDAD?.ToString());
                    xmlWriter.WriteElementString("ID_COMPANIA", "rawxml", batchData.promotionData[0].ID_COMPANIA?.ToString());
                    xmlWriter.WriteElementString("LEGACY_EF", "rawxml", batchData.promotionData[0].LEGACY_EF?.ToString());
                    xmlWriter.WriteElementString("LEGACY_CR", "rawxml", batchData.promotionData[0].LEGACY_CR?.ToString());

                    // Create a container for the promotions
                    xmlWriter.WriteStartElement("Promotions", "rawxml");
                    // Enumerate through all the Promotiondata properties
                    foreach (var promotion in batchData.promotionData)
                    {
                        // JSON Document Name: "PRM"<plaza><tienda><sysdate(yyMMddHHmmss)>".json.gz"
                        jsonDocumentName = $"PRM{promotion.ORACLE_CR_SUPERIOR}{promotion.ORACLE_CR}{DateTime.Now.ToString("yyMMddHHmmss")}_{uniqueIdentifier}.json";

                        xmlWriter.WriteStartElement("PromotionData", "rawxml");

                        xmlWriter.WriteElementString("lineNumber", "rawxml", rowNumber.ToString());
                        xmlWriter.WriteElementString("folio", "rawxml", promotion.PROMOTION?.ToString());
                        xmlWriter.WriteElementString("itemCategory", "rawxml", string.Empty);
                        xmlWriter.WriteElementString("type", "rawxml", string.Empty);
                        xmlWriter.WriteElementString("startDate", "rawxml", promotion.START_DATE?.ToString("yyyyMMdd"));
                        xmlWriter.WriteElementString("endDate", "rawxml", promotion.END_DATE?.ToString("yyyyMMdd"));
                        xmlWriter.WriteElementString("itemSku", "rawxml", promotion.ITEM?.ToString());
                        xmlWriter.WriteElementString("category", "rawxml", promotion.MANUAL_PRICE_ENTRY?.ToString());
                        xmlWriter.WriteElementString("providerType", "rawxml", string.Empty);
                        xmlWriter.WriteElementString("requiredQuantity", "rawxml", string.Empty);
                        xmlWriter.WriteElementString("discount", "rawxml", string.Empty);
                        xmlWriter.WriteElementString("discountType", "rawxml", string.Empty);
                        xmlWriter.WriteElementString("optative", "rawxml", string.Empty);
                        xmlWriter.WriteElementString("description", "rawxml", promotion.PROM_NAME?.ToString());
                        xmlWriter.WriteElementString("itemSubcategory", "rawxml", promotion.DEPT?.ToString());
                        xmlWriter.WriteElementString("itemSegment", "rawxml", promotion.CLASS?.ToString());
                        xmlWriter.WriteElementString("itemSubsegment", "rawxml", promotion.SUBCLASS?.ToString());
                        xmlWriter.WriteElementString("durationCode", "rawxml", string.Empty);
                        xmlWriter.WriteElementString("grouper", "rawxml", promotion.REF_PREFIX?.ToString());
                        xmlWriter.WriteElementString("percent", "rawxml", string.Empty);
                        xmlWriter.WriteElementString("optativeOne", "rawxml", string.Empty);
                        xmlWriter.WriteElementString("sent", "rawxml", string.Empty);
                        xmlWriter.WriteElementString("action", "rawxml", string.Empty);
                        xmlWriter.WriteElementString("binCode", "rawxml", promotion.PROM_BIN_CODE?.ToString());
                        xmlWriter.WriteElementString("messagePsg", "rawxml", promotion.MSG_POS?.ToString());
                        xmlWriter.WriteElementString("tranType", "rawxml", promotion.TRAN_TYPE?.ToString());
                        xmlWriter.WriteElementString("promTranType", "rawxml", promotion.PROM_TRAN_TYPE?.ToString());
                        xmlWriter.WriteElementString("thresHold_no", "rawxml", promotion.THRESHOLD_NO?.ToString());
                        xmlWriter.WriteElementString("thresHoldAmt", "rawxml", promotion.THRESHOLD_AMT?.ToString());
                        xmlWriter.WriteElementString("newPrice", "rawxml", promotion.NEW_PRICE?.ToString());
                        xmlWriter.WriteElementString("discountAmt", "rawxml", promotion.DISCOUNT_AMT?.ToString());
                        xmlWriter.WriteElementString("newSellingUom", "rawxml", promotion.NEW_SELLING_UOM?.ToString());
                        xmlWriter.WriteElementString("dept", "rawxml", promotion.DEPT?.ToString());
                        xmlWriter.WriteElementString("class", "rawxml", promotion.CLASS?.ToString());
                        xmlWriter.WriteElementString("subClass", "rawxml", promotion.SUBCLASS?.ToString());
                        xmlWriter.WriteElementString("refPrefix", "rawxml", promotion.REF_PREFIX?.ToString());
                        xmlWriter.WriteElementString("prefix", "rawxml", promotion.PREFIX?.ToString());
                        xmlWriter.WriteElementString("manualPriceEntry", "rawxml", promotion.MANUAL_PRICE_ENTRY?.ToString());
                        xmlWriter.WriteElementString("msgPos", "rawxml", promotion.MSG_POS?.ToString());
                        xmlWriter.WriteElementString("startTime", "rawxml", promotion.START_TIME?.ToString());
                        xmlWriter.WriteElementString("endTime", "rawxml", promotion.END_TIME?.ToString());
                        xmlWriter.WriteElementString("shortDesc", "rawxml", promotion.ITEM_SHORT_DESC?.ToString());
                        xmlWriter.WriteElementString("promBinCode", "rawxml", promotion.PROM_BIN_CODE?.ToString());

                        rowNumber++;
                        xmlWriter.WriteEndElement();

                        try
                        {
                            log.LogInformation($"[runIdentifier]|[uniqueIdentifier]:Updating the promotion status table for [CR_PLAZA] = '{batchData.promotionData[0].ORACLE_CR_SUPERIOR?.ToString()}', [CR_TIENDA] = '{batchData.promotionData[0].ORACLE_CR?.ToString()}'.");

                            // Update the promotion document status table
                            using (SqlConnection connection = new SqlConnection(sqlBuilder.ConnectionString))
                            {
                                connection.Open();

                                sqlQuery = $"UPDATE [dbo].[CO_PRM_DCM_STS] SET [PV_DOC_NAME] = '{jsonDocumentName}', [UNIQUE_IDENTIFIER] = '{uniqueIdentifier}', [PV_DOC_TYPE] = 'PRM', [CR_PLAZA] = '{batchData.promotionData[0].ORACLE_CR_SUPERIOR?.ToString()}', [CR_TIENDA] = '{batchData.promotionData[0].ORACLE_CR?.ToString()}', [SOURCE] = 'POS' WHERE [BATCH_ID] = {req.Query["batchId"]} and [LOCATION] = {req.Query["location"]} and [LOAD_BATCH_ID] = {req.Query["loadBatchId"]}";
                                log.LogInformation($"[runIdentifier]|[uniqueIdentifier]:Updating SQL table: CO_PRM_DCM_STS using '{sqlQuery}'");

                                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                                {
                                    var result = command.ExecuteNonQuery();
                                }

                                sqlQuery = $"UPDATE [dbo].[CO_PRM_PRCS] SET [UNIQUE_IDENTIFIER] = '{uniqueIdentifier}' WHERE [RUN_IDENTIFIER] = '{runIdentifier}' and [BATCH_ID] = {req.Query["batchId"]} and [LOCATION] = {req.Query["location"]} and [LOAD_BATCH_ID] = {req.Query["loadBatchId"]}";

                                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                                {
                                    var result = command.ExecuteNonQuery();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            string errorInfo = $"[runIdentifier]|[uniqueIdentifier]:Failed to update the promotion status table for [CR_PLAZA] = '{batchData.promotionData[0].ORACLE_CR_SUPERIOR?.ToString()}', [CR_TIENDA] = '{batchData.promotionData[0].ORACLE_CR?.ToString()}'.\r\nException: {ex.Message}";
                            log.LogInformation(errorInfo);
                            return new BadRequestObjectResult(errorInfo);
                        }
                    }

                    xmlWriter.WriteEndElement();

                    // Finish the XML document
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();
                    xmlStream.Seek(0, SeekOrigin.Begin);

                    // This code writes the XML to a blob
                    // Write the Generated XML to the XML container
                    BlobClient xmlBlobClient = new BlobClient(oxxoPromotionStorageConnectionString, oxxoRawXmlBlobContainer, jsonBlobName);
                    xmlBlobClient.Upload(xmlStream, true);
                    xmlStream.Seek(0, SeekOrigin.Begin);

                    xmlDocument = new StreamReader(xmlStream).ReadToEnd();

                    //// Call the Logic App using the HTTP POST method
                    //HttpClient client = new HttpClient();
                    //StringContent content = new StringContent($"{new StreamReader(xmlStream).ReadToEnd()}");
                    //var response = await client.PostAsync(logicAppRequestUrl, content);

                    //if ((response == null) || (!response.IsSuccessStatusCode))
                    //{
                    //    throw new ApplicationException($"Batch {req.Query["location"]}_{req.Query["batchId"]}_{req.Query["loadBatchId"]}.json failed with an HTTP status code of: {response.StatusCode} and an error of {response.ReasonPhrase}");
                    //}

                    if (deleteJsonBlob)
                    {
                        // Delete the JSON Blob
                        jsonBlobClient.DeleteIfExists(DeleteSnapshotsOption.IncludeSnapshots);
                    }
                }
                else
                {
                    log.LogInformation($"[runIdentifier]|[uniqueIdentifier]:Batch {req.Query["location"]}_{req.Query["batchId"]}_{req.Query["loadBatchId"]}.json did not contain promotion data.");

                    if (deleteJsonBlob)
                    {
                        // Delete the JSON blob
                        jsonBlobClient.DeleteIfExists(DeleteSnapshotsOption.IncludeSnapshots);
                    }

                    try
                    {
                        log.LogInformation($"[runIdentifier]|[uniqueIdentifier]:Deleting running promotion entry for failed promotion document.");

                        // Update the promotion document status table
                        using (SqlConnection connection = new SqlConnection(sqlBuilder.ConnectionString))
                        {
                            connection.Open();

                            sqlQuery = $"DELETE FROM [dbo].[CO_PRM_PRCS] WHERE [RUN_IDENTIFIER] = '{runIdentifier}' and [BATCH_ID] = {req.Query["batchId"]} and [LOCATION] = {req.Query["location"]} and [LOAD_BATCH_ID] = {req.Query["loadBatchId"]}";

                            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                            {
                                var result = command.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string errorInfo = $"[runIdentifier]|[uniqueIdentifier]:Failed to update the promotion status table for [CR_PLAZA] = '{batchData.promotionData[0].ORACLE_CR_SUPERIOR?.ToString()}', [CR_TIENDA] = '{batchData.promotionData[0].ORACLE_CR?.ToString()}'.\r\nException: {ex.Message}";
                        log.LogInformation(errorInfo);
                        return new BadRequestObjectResult(errorInfo);
                    }

                    // Throw an exception indicating the batch data was invalid
                    throw new ApplicationException($"Batch {req.Query["location"]}_{req.Query["batchId"]}_{req.Query["loadBatchId"]}.json did not contain promotion data.");
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"[runIdentifier]|[uniqueIdentifier]:Blob: {jsonBlobName}\r\nFailed: {ex.Message}");
            }

            // return new OkObjectResult(xmlDocument);
            return new OkObjectResult($"[runIdentifier]|[uniqueIdentifier]:Successfully generated {jsonDocumentName}");
        }

        static string GetUniqueIdentifier(string connectionString, string runIdentifier, string batchID, string location, string loadBatchID)
        {
            SqlConnectionStringBuilder SqlBuilder = new SqlConnectionStringBuilder(Environment.GetEnvironmentVariable("OxxoSqlConnectionString", EnvironmentVariableTarget.Process));
            string sqlQuery = $"SELECT [UNIQUE_IDENTIFIER] FROM [dbo].[CO_PRM_DCM_STS] WHERE [BATCH_ID] = {batchID} and [LOCATION] = {location} and [LOAD_BATCH_ID] = {loadBatchID} and [RUN_IDENTIFIER] = '{runIdentifier}'";

            using (SqlConnection connection = new SqlConnection(SqlBuilder.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return reader.GetValue(0).ToString();
                        }
                    }
                }
            }

            throw new ApplicationException($"Unique identifier not found.");
        }
    }
}
