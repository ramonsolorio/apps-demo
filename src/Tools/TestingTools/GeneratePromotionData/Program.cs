using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using System.Xml.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace GeneratePromotionData
{
    internal class Program
    {
        private static string PromotionConnectionString { get; set; } = "Promotion DB";
        private static string MockConnectionString { get; set; } = "Mock DB";
        private static int NumberOfPromotions { get; set; } = 10;
        private static PromotionList Promotions { get; set; } = new PromotionList();
        private static int LoadWeek = 42;
        private static int LoadBatchId = 300000000;
        private static int BatchId = 300000;
        private static string LoadTimestamp = "2024-10-28 07:54:53.0000000";


        private static bool ParseCommmandLine(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].Replace('/', '-'))
                {
                    case "-?":
                        {
                            return true;
                        }

                    case "-pcs":
                    case "-promotionconnectionstring":
                        {
                            PromotionConnectionString = args[++i];
                            break;
                        }

                    case "-mcs":
                    case "-mockconnectionstring":
                        {
                            MockConnectionString = args[++i];
                            break;
                        }

                    case "-n":
                    case "-numberofpromotions":
                        {
                            NumberOfPromotions = int.Parse(args[++i]);
                            break;
                        }

                    default:
                        {
                            Console.WriteLine("Invalid command line argument passed.");
                            return true;
                        }
                }
            }

            return false;
        }

        private static void Usage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("GeneratePromotionData.exe [-?] | [-pcs <connectionstring> -mcs <connectionstring> -n <numberofpromotions>]");
            Console.WriteLine();
            Console.WriteLine("where: ");
            Console.WriteLine("   -?                  - usage");
            Console.WriteLine("   -pcs                - Promotion SQL database connection string");
            Console.WriteLine("   -mcs                - Mock SQL database connection string");
            Console.WriteLine("   -n                  - number of promotions to generate");
            Console.WriteLine();
            Console.WriteLine("Example:");
            Console.WriteLine();
            Console.WriteLine(" GeneratePromotionData.exe /pcs \"<connectionstring>\" /mcs \"<connectionstring>\" /n 1000");
            Console.WriteLine("");
        }

        static void Main(string[] args)
        {
            if (!ParseCommmandLine(args))
            {
                Promotions.promotions = new List<Promotion>();
                string sqlTemplate = File.ReadAllText(@".\PromotionDataTemplate.txt");

                GetPromotionInformation();

                foreach (Promotion promotion in Promotions.promotions)
                {
                    string insertScript = sqlTemplate.Replace("<Location>", promotion.location).Replace("<BatchID>", promotion.batchId).Replace("<LoadTimestamp>", promotion.loadTimestamp).Replace("<Promotion>", promotion.loadWeek).Replace("<LoadBatchId>", promotion.loadBatchId).Replace("<LoadWeek>", promotion.loadWeek);

                    SqlConnectionStringBuilder SqlBuilder = new SqlConnectionStringBuilder(MockConnectionString);

                    using (SqlConnection connection = new SqlConnection(SqlBuilder.ConnectionString))
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand(insertScript, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            else
            {
                Usage();
            }
        }

        private static void GetPromotionInformation()
        {
            SqlConnectionStringBuilder SqlBuilder = new SqlConnectionStringBuilder(PromotionConnectionString);
            string sqlQuery = $"SELECT top {NumberOfPromotions} [RETEK_CR] FROM [dbo].[CO_ERDD_MCRS] WHERE [RETEK_CR] > 0";

            using (SqlConnection connection = new SqlConnection(SqlBuilder.ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Promotion promotion = null;

                        while (reader.Read())
                        {
                            Object[] values = new Object[reader.FieldCount];
                            int resultCount = reader.GetValues(values);

                            for (int i = 0; i < resultCount; i++)
                            {
                                promotion = new Promotion()
                                {
                                    batchId = BatchId.ToString(),
                                    location = values[i].ToString(),
                                    loadBatchId = LoadBatchId.ToString(),
                                    loadWeek = LoadWeek.ToString(),
                                    loadTimestamp = LoadTimestamp
                                };
                            }

                            if (!string.IsNullOrEmpty(promotion.location))
                            {
                                Promotions.promotions.Add(promotion);
                            }
                        }
                    }
                }
            }
        }
    }
}
