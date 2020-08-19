using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace ExcelImport
{
    class Program
    {
        static void Main(string[] args)
        {
            if (ExcelImport.Properties.Settings.Default.MultiSheetImport)
            {
                Console.WriteLine("MULTI SHEET IMPORT");

                MultiSheetImport mi = new MultiSheetImport();

                return;
            }

            Console.WriteLine("Starting Import...");

            List<List<string>> data = null;

            try
            {
                bool ignoreHeader = Convert.ToBoolean(ExcelImport.Properties.Settings.Default.IgnoreHeader);

                data = Excel.ReadData(
                    ExcelImport.Properties.Settings.Default.Importfile,
                    ExcelImport.Properties.Settings.Default.ImportSheet,
                    ExcelImport.Properties.Settings.Default.Importspalte_Beginn,
                    ExcelImport.Properties.Settings.Default.Importspalte_Ende,
                    ignoreHeader,
                    ExcelImport.Properties.Settings.Default.IndexOfDateTimeColumns,
                    ExcelImport.Properties.Settings.Default.IgnoreEmptyRows
                    );

                Console.WriteLine("Starting DB Import");

                using (SqlConnection conn = new SqlConnection(ExcelImport.Properties.Settings.Default.ConnectionString))
                {
                    conn.Open();

                    string sql = "Truncate TABLE " + ExcelImport.Properties.Settings.Default.ImportTable;

                    using(SqlCommand cmd = new SqlCommand(sql, conn)){
                    cmd.ExecuteNonQuery();
                    }

                    foreach (List<string> contentLine in data)
                    {
                        sql =   "INSERT INTO " + ExcelImport.Properties.Settings.Default.ImportTable;
                        sql += " VALUES( ";

                        int i = 0;
                        foreach (string cell in contentLine)
                        {
                            i++;
                            sql += ("'" + cell.Replace("'", string.Empty) + "'");

                            if (i < contentLine.Count)
                            {
                                sql += ",";
                            }
                        }

                        sql += " )";

                        Console.WriteLine(sql);

                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }


                Console.WriteLine("DB Import done");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
