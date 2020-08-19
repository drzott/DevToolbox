using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace ExcelImport
{
    /// <summary>
    /// Imports identical !!! sheets to identical tables
    /// Index of Import Sheet is the same Index as Target Table
    /// </summary>
    public class MultiSheetImport
    {
        public MultiSheetImport()
        {
            List<string> ImportSheets = ExcelImport.Properties.Settings.Default.MultiSheetImport_Sheets.Cast<string>().ToList();
            List<string> TargetTables = ExcelImport.Properties.Settings.Default.MultiSheetImport_TargetTables.Cast<string>().ToList();
            int j = 0;


            foreach (string source in ImportSheets)
            {
                List<List<string>> data = null;

                try
                {
                    bool ignoreHeader = Convert.ToBoolean(ExcelImport.Properties.Settings.Default.IgnoreHeader);

                    data = Excel.ReadData(
                        ExcelImport.Properties.Settings.Default.Importfile,
                        Convert.ToInt32(source),
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

                        string sql = "Truncate TABLE " + TargetTables[j];

                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }

                        foreach (List<string> contentLine in data)
                        {
                            sql = "INSERT INTO " + TargetTables[j];
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

                j++;
            }
        }
    }
}
