using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;

using OfficeOpenXml;

namespace Excel_Import
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                Console.ForegroundColor = ConsoleColor.Green;

                CheckSettings();
                //CreateDBTable();
                CreateDBTableIfNotExists();
                EmptyDBTable();
                List<List<string>> importData = ReadExcelData();
                ImportDB(importData);

                Console.WriteLine("Import complete");

            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine("*******************************************************************************");
                Console.WriteLine("*************************** I M P O R T   E R R O R ***************************");
                Console.WriteLine("*******************************************************************************");
                Console.WriteLine();
                Console.WriteLine(ex.ToString());

                System.Threading.Thread.Sleep(10000);
            }
        }

        private static void ImportDB(List<List<string>> importData)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("DB Import");
            // build insert string

            string sqlFirstPart = "INSERT INTO " + Excel_Import.Properties.Settings.Default.ImportTable + " (";

                bool firstColumn = true;
                for(int i = Excel_Import.Properties.Settings.Default.ColumnIndexStart; i <= Excel_Import.Properties.Settings.Default.ColumnIndexEnd; i++){
                    if (firstColumn) { firstColumn = false; } else { sqlFirstPart += ","; }
                    sqlFirstPart += "[" + i + "] ";
                }

            sqlFirstPart += " ) VALUES ( ";
            
            using (SqlConnection conn = new SqlConnection(Excel_Import.Properties.Settings.Default.ConnectionString))
            {
                conn.Open();

                
                foreach (List<string> line in importData)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;

                    string sql = sqlFirstPart;
                    firstColumn = true;

                    int index = 0;
                    foreach (string c in line)
                    {
                        if (firstColumn) { firstColumn = false; } else { sql += ","; }

                        index++;

                        sql += "@p" + index;
                        cmd.Parameters.AddWithValue("@p" + index, c);
                    }
                    sql += " )";

                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static List<List<string>> ReadExcelData()
        {
            List<int> dtoCols = new List<int>();
            List<FileInfo> lfInfo;

            foreach (string di in Excel_Import.Properties.Settings.Default.DateTimeColumns)
            {
                dtoCols.Add(Convert.ToInt32(di));
            }

            Console.WriteLine();

            int wBookIndex = -1;


            lfInfo = getDateienListe(Excel_Import.Properties.Settings.Default.ImportFile);
            List<List<string>> importData = new List<List<string>>();

            foreach (FileInfo fi in lfInfo)
            {

                Console.ForegroundColor = ConsoleColor.Blue;

                int emptyRowcounter = 0;
                importData = new List<List<string>>();
                Console.WriteLine("OPEN Excel File...");

                //FileInfo fi = new FileInfo(Excel_Import.Properties.Settings.Default.ImportFile);

                using (ExcelPackage xls = new ExcelPackage(fi))
                {
                    ExcelWorkbook xlsb = xls.Workbook;

                    ExcelWorksheet xlsw = null;



                    foreach (string strWBook in Excel_Import.Properties.Settings.Default.WorkbookIndex)
                    {

                        try { wBookIndex = Convert.ToInt32(Excel_Import.Properties.Settings.Default.WorkbookIndex); }
                        catch (Exception) { ;}


                        if (wBookIndex > 0)
                        {
                            Console.WriteLine("Open Sheet: " + wBookIndex);
                            xlsw = xlsb.Worksheets[wBookIndex];
                        }
                        else
                        {
                            Console.WriteLine("Open Sheet: " + wBookIndex);
                            xlsw = xlsb.Worksheets[strWBook];
                        }

                        Console.WriteLine(string.Format("Loading Columns FROM {0} TO {1}", Excel_Import.Properties.Settings.Default.ColumnIndexStart, Excel_Import.Properties.Settings.Default.ColumnIndexEnd));

                        int lineCounter = 0;

                        while (true)
                        {
                            lineCounter++;
                            if (emptyRowcounter >= Excel_Import.Properties.Settings.Default.MaxEmptyRows)
                                break;

                            if (xlsw.Cells[lineCounter, Excel_Import.Properties.Settings.Default.ColumnIndexStart].Value == null)
                            {
                                emptyRowcounter++;
                                continue;
                            };

                            List<string> line = new List<string>();

                            bool emptyRow = true;
                            for (int x = Excel_Import.Properties.Settings.Default.ColumnIndexStart; x <= Excel_Import.Properties.Settings.Default.ColumnIndexEnd; x++)
                            {
                                string cellType = string.Empty;

                                //if(xlsw.Cells[lineCounter, x] != null && xlsw.Cells[lineCounter, x].Value != null)

                                //cellType = xlsw.Cells[lineCounter, x].Value.GetType().ToString();

                                if (dtoCols.Contains(x))
                                    cellType = "System.DateTime";


                                switch (cellType)
                                {
                                    case "System.DateTime":
                                        {
                                            try
                                            {
                                                //DateTime d = DateTime.Parse(xlsw.Cells[lineCounter, x].Value.ToString());
                                                DateTime d = DateTime.FromOADate(Convert.ToDouble(xlsw.Cells[lineCounter, x].Value));
                                                line.Add(d.ToString("yyyyMMdd"));
                                                emptyRow = false;
                                            }
                                            catch (Exception)
                                            {
                                                string cell = xlsw.Cells[lineCounter, x].Value.ToString();

                                                if (cell.Length >= 500)
                                                    cell = cell.Substring(0, 499);

                                                line.Add(cell);

                                                if (!string.IsNullOrEmpty(cell))
                                                    emptyRow = false;
                                            }

                                            break;
                                        }
                                    default:
                                        {
                                            string cell = string.Empty;

                                            if (xlsw.Cells[lineCounter, x] != null && xlsw.Cells[lineCounter, x].Value != null)
                                                cell = xlsw.Cells[lineCounter, x].Value.ToString();

                                            if (cell.Length >= 500)
                                                cell = cell.Substring(0, 499);

                                            line.Add(cell);

                                            if (!string.IsNullOrEmpty(cell))
                                                emptyRow = false;

                                            break;
                                        }
                                }
                            }

                            if (lineCounter > 1 || (!Excel_Import.Properties.Settings.Default.IgnoreHeader))
                            {
                                line.Add(fi.FullName);
                                line.Add(xlsw.Name);
                                importData.Add(line);
                            }

                            if (emptyRow)
                                emptyRowcounter++;
                        }
                    }

                }                
            }



            return importData;
        }

        private static void EmptyDBTable()
        {

            using (SqlConnection conn = new SqlConnection(Excel_Import.Properties.Settings.Default.ConnectionString))
            {
                conn.Open();

                string sql = "TRUNCATE TABLE " + Excel_Import.Properties.Settings.Default.ImportTable;

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
        }

        private static void CreateDBTable()
        {
            Console.WriteLine("Creating Import Table...");

            using (SqlConnection conn = new SqlConnection(Excel_Import.Properties.Settings.Default.ConnectionString))
            {
                conn.Open();

                string sql = string.Format(@" IF EXISTS (SELECT Table_Name from information_schema.tables WHERE TABLE_TYPE = 'BASE TABLE' AND Table_Name = '{0}')
                                                BEGIN
                                                    DROP TABLE {1}
                                                END",
                                                    Excel_Import.Properties.Settings.Default.ImportTable,
                                                    Excel_Import.Properties.Settings.Default.ImportTable);

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();


                sql = "CREATE TABLE " + Excel_Import.Properties.Settings.Default.ImportTable + " ( ";
                
                bool firstColumn = true;
                for(int i = Excel_Import.Properties.Settings.Default.ColumnIndexStart; i <= Excel_Import.Properties.Settings.Default.ColumnIndexEnd; i++){
                    if(firstColumn) { firstColumn = false; } else { sql += ","; }
                    sql += "[" + i + "] VARCHAR(500) NULL ";
                }

                sql += " )";

                cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Import Table created.");
        }

        private static void CreateDBTableIfNotExists()
        {
            Console.WriteLine("Creating Import Table...");

            using (SqlConnection conn = new SqlConnection(Excel_Import.Properties.Settings.Default.ConnectionString))
            {
                conn.Open();

                string sql = string.Format(@" IF EXISTS (SELECT Table_Name from information_schema.tables WHERE TABLE_TYPE = 'BASE TABLE' AND Table_Name = '{0}')
                                                BEGIN
                                                    DROP TABLE {1}
                                                END",
                                                    Excel_Import.Properties.Settings.Default.ImportTable,
                                                    Excel_Import.Properties.Settings.Default.ImportTable);

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();


                sql = @"                         IF NOT EXISTS (SELECT Table_Name from information_schema.tables WHERE TABLE_TYPE = 'BASE TABLE' AND Table_Name = '" ;
                
                sql += Excel_Import.Properties.Settings.Default.ImportTable;

                sql += @" {0}')

                        BEGIN
                
                CREATE TABLE " + Excel_Import.Properties.Settings.Default.ImportTable + " ( ";

                bool firstColumn = true;
                for (int i = Excel_Import.Properties.Settings.Default.ColumnIndexStart; i <= Excel_Import.Properties.Settings.Default.ColumnIndexEnd +2 ; i++)
                {
                    if (firstColumn) { firstColumn = false; } else { sql += ","; }
                    sql += "[" + i + "] VARCHAR(500) NULL ";
                }

                sql += " )";

                sql += " END ";

                cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Import Table created.");
        }

        private static void CheckSettings()
        {
            Console.WriteLine("Loading Settings");

            if (string.IsNullOrEmpty(Excel_Import.Properties.Settings.Default.ImportFile))
                throw new Exception("Missing Import File");

            if (!File.Exists(Excel_Import.Properties.Settings.Default.ImportFile))
                throw new Exception("Missing File:" + Excel_Import.Properties.Settings.Default.ImportFile);

            Console.WriteLine(Excel_Import.Properties.Settings.Default.ImportFile);

            if (string.IsNullOrEmpty(Excel_Import.Properties.Settings.Default.ConnectionString))
                throw new Exception("Missing ConnectionString");

            Console.WriteLine(Excel_Import.Properties.Settings.Default.ConnectionString);

            Console.WriteLine("Checking Connection...");

            using (SqlConnection conn = new SqlConnection(Excel_Import.Properties.Settings.Default.ConnectionString))
                conn.Open();

            Console.WriteLine("OK");

            if (string.IsNullOrEmpty(Excel_Import.Properties.Settings.Default.ImportTable))
                throw new Exception("Missing ImportTable");

            Console.WriteLine("ImportTable: " + Excel_Import.Properties.Settings.Default.ImportTable);

            if (string.IsNullOrEmpty(Excel_Import.Properties.Settings.Default.WorkbookIndex))
                throw new Exception("Missing WorkbookIndex");

            Console.WriteLine("Workbook Index: " + Excel_Import.Properties.Settings.Default.WorkbookIndex);

            if (Excel_Import.Properties.Settings.Default.ColumnIndexStart == 0)
                throw new Exception("Missing or Illegal ColumnIndexStart");

            Console.WriteLine("ColumnIndexStart: " + Excel_Import.Properties.Settings.Default.ColumnIndexStart);

            if (Excel_Import.Properties.Settings.Default.ColumnIndexEnd == 0)
                throw new Exception("Missing or Illegal ColumnIndexEnd");

            Console.WriteLine("ColumnIndexEnd: " + Excel_Import.Properties.Settings.Default.ColumnIndexEnd);

            Console.WriteLine("MaxEmptyRows: " + Excel_Import.Properties.Settings.Default.MaxEmptyRows);


            foreach (string s in Excel_Import.Properties.Settings.Default.DateTimeColumns)
            {
                try
                {
                    Convert.ToInt32(s);
                }
                catch (Exception)
                {
                    throw new Exception("Illegal DateTime Index");
                }
            }
        }

        private static List<FileInfo> getDateienListe(string pPfad)
        {
            List<FileInfo> lfInfo = new List<FileInfo>();
            FileInfo fInfo;
            string OrdnerPfad;
            string Dateiname;
            //string Datei;

            if (pPfad.Contains("*"))
            {
                Dateiname = pPfad.Substring(pPfad.LastIndexOf("\\")).Replace("\\", "");
                OrdnerPfad = pPfad.Substring(0, pPfad.LastIndexOf("\\"));
                foreach (string Datei in Directory.GetFiles(OrdnerPfad,Dateiname))
                {
                    if (File.Exists(Datei)==true)
                    {
                        fInfo = new FileInfo(Datei);
                        if ((fInfo.Extension.ToUpper()).Contains("XLS"))
                        {
                            lfInfo.Add(fInfo);
                        }
                    }   
                }
            }
            else
            {
                if (File.Exists(pPfad)==true)
                {
                        fInfo = new FileInfo(pPfad);
                        if ((fInfo.Extension.ToUpper()).Contains("XLS"))
                        {
                            lfInfo.Add(fInfo);
                        }
                }
                if (Directory.Exists(pPfad)==true)
                {
                    foreach (string Datei in Directory.GetFiles(pPfad))
                    {
                        if (File.Exists(Datei)==true)
                        {
                            fInfo = new FileInfo(Datei);
                            if ((fInfo.Extension.ToUpper()).Contains("XLS"))
                            {
                                lfInfo.Add(fInfo);
                            }
                        }   
                    }
                }
            }
            return lfInfo;
        }

    }
}
