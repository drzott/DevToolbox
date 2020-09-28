using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;

namespace BulkImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            string connStr = BulkImporter.Properties.Settings.Default.ConnectionString;

            string file = BulkImporter.Properties.Settings.Default.File;



            // wenn der Filter aktiv ist, werden nur Zeilen importiert die in der spalte (0 basiert) den definierten Wert haben
            bool filter = BulkImporter.Properties.Settings.Default.Filter;
            int filterColumn = BulkImporter.Properties.Settings.Default.FilterColumn;
            string filterValue = BulkImporter.Properties.Settings.Default.FilterValue;


            // Aktiviert den Splitmode wenn true
            // --> Je nach Index wird immer nur die geraden oder ungeraden Zeilen importiert --> Import mit mehreren Threads
            // Split Mode = true --> nur gerade Zeilen
            // Split Mode = false --> nur ungerade Zeilen
            bool splitModeAktiv = BulkImporter.Properties.Settings.Default.SplitModeAktiv;
            bool splitMode = BulkImporter.Properties.Settings.Default.SplitMode;

            Console.WriteLine(file);

            string delim = BulkImporter.Properties.Settings.Default.Delim;

            if (string.IsNullOrEmpty(delim))
                delim = ";";

            Console.WriteLine("Delim: " + delim);

            string table = BulkImporter.Properties.Settings.Default.Table;
            Console.WriteLine("Table: " + table);

            Console.WriteLine("");
            Console.WriteLine("Columns");

            foreach(string c in BulkImporter.Properties.Settings.Default.Columns)
            {
                Console.WriteLine(c);
            }

            Console.WriteLine("Checking for table & connection");

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string sql = @"IF NOT EXISTS(SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @t)
                             BEGIN 

                                CREATE TABLE " + table + " (";

                int i = 1;
                foreach (string c in BulkImporter.Properties.Settings.Default.Columns)
                {
                    sql += c + " varchar(500) NULL ";

                    if (i < BulkImporter.Properties.Settings.Default.Columns.Count)
                        sql += ",";

                    i++;
                }


                sql += ") END TRUNCATE TABLE " +table;

                Console.WriteLine(sql);

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@t", table);

                cmd.ExecuteNonQuery();



            }

            Console.WriteLine("Check done");

            Console.WriteLine("Reading file");

            string line = string.Empty;
            string[] s = { delim };


            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                int i = 0;
                using (StreamReader r = new StreamReader(file))
                {
                    while ((line = r.ReadLine()) != null)
                    {

                        i++;

                        if (splitModeAktiv)
                        {
                            if (splitMode)
                            {
                                if (i % 2 != 0)
                                    continue;
                            }
                            else
                            {
                                if (i % 2 == 0)
                                    continue;
                            }
                        }

                        if (i % 10000 == 0)
                            Console.WriteLine(i);

                        if (i % 10001 == 0)
                            Console.WriteLine(i);

                        if (string.IsNullOrEmpty(line))
                        {
                            Console.WriteLine("Illegal line");
                            continue;
                        }

                        string[] splitLine = line.Split(s, StringSplitOptions.None);

                        if (splitLine.Count() != BulkImporter.Properties.Settings.Default.Columns.Count)
                        {
                            Console.WriteLine("Illegal line");
                            continue;
                        }

                        if (filter)
                        {
                            if (splitLine[filterColumn] != filterValue)
                                continue;
                        }

                        string sql = "INSERT INTO " + table + " Values(";

                        int j = 1;

                        SqlCommand cmd = new SqlCommand();

                        foreach(string v in splitLine)
                        {
                            string param = "@X" + j;

                            sql += param;

                            if (j < splitLine.Count())
                                sql += ",";

                            string vc = v;
                            if (string.IsNullOrEmpty(vc))
                                vc = string.Empty;

                            cmd.Parameters.AddWithValue(param, vc);
                            j++;
                        }


                        sql += ")";

                        cmd.Connection = conn;
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                    }

                    r.Close();
                }


            }

        }
    }
}
