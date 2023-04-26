using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Reflection;

namespace CSharp.TestSuites
{
    public class ADODotNetTestSuite : TestSuite, ITestSuite
    {
        private static readonly string kNorthwindConnectionString = "Server=.\\SQLEXPRESS;Database=Northwind;Trusted_Connection=True;";

        public void Run()
        {
            WriteTestSuiteName();

            ADODotNet1();
            ADODotNet2();
            ADODotNet3();

            WriteTestSuiteName();
        }

        private void ADODotNet1()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            try
            {
                using (SqlConnection connection = new SqlConnection(kNorthwindConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Categories", connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            Console.WriteLine("Categories:");
                            while (reader.Read())
                            {
                                int CategoryID = reader.GetInt32(0);
                                string CategoryName = reader.GetString(1);
                                string Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                                SqlBytes Picture = reader.IsDBNull(3) ? null : reader.GetSqlBytes(3);

                                Console.WriteLine($"CategoryID: {CategoryID}, CategoryName: {CategoryName}, Description: {Description}, Bytes in Picture: {Picture.Length}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ADODotNet2()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            try
            {
                using (SqlConnection connection = new SqlConnection(kNorthwindConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Categories", connection))
                    {
                        DataTable categoriesTbl = new DataTable();
                        categoriesTbl.Load(cmd.ExecuteReader());

                        // A DataSet is a container for 1 or more DataTable's
                        DataSet categoriesDS = new DataSet();
                        categoriesDS.Tables.Add(categoriesTbl);

                        IEnumerable<DataRow> query =
                            from category
                            in categoriesTbl.AsEnumerable()
                            select category;

                        Console.WriteLine("Categories:");
                        foreach (DataRow c in query)
                        {
                            int CategoryID = c.Field<int>("CategoryID");
                            string CategoryName = c.Field<string>("CategoryName");
                            string Description = c.Field<string>("Description");
                            byte[] Picture = c.Field<byte[]>("Picture");

                            Console.WriteLine($"CategoryID: {CategoryID}, CategoryName: {CategoryName}, Description: {Description}, Bytes in Picture: {Picture.Length}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ADODotNet3()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            try
            {
                using (SqlConnection connection = new SqlConnection(kNorthwindConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Categories", connection))
                    {
                        DataTable categoriesTbl = new DataTable();
                        categoriesTbl.Load(cmd.ExecuteReader());

                        DataSet categoriesDS = new DataSet();
                        categoriesDS.Tables.Add(categoriesTbl);

                        var query =
                            categoriesTbl.AsEnumerable().
                            Select(c => new
                            {
                                CategoryID = c.Field<int>("CategoryID"),
                                CategoryName = c.Field<string>("CategoryName"),
                                Description = c.Field<string>("Description"),
                                Picture = c.Field<byte[]>("Picture")
                            });

                        Console.WriteLine("Categories:");
                        foreach (var c in query)
                        {
                            Console.WriteLine($"CategoryID: {c.CategoryID}, CategoryName: {c.CategoryName}, Description: {c.Description}, Bytes in Picture: {c.Picture.Length}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
