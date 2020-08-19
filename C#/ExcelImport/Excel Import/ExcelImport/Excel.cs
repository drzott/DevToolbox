using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.IO;

using Microsoft.Office.Interop.Excel;

namespace ExcelImport
{
    public static class Excel
    {
        public static List<List<string>> ReadData(string importFile,int SheetIndex, string columnIndexStart, string columnIndexEnd, bool ignoreHeader, System.Collections.Specialized.StringCollection dateTimeColumns,int IgnoreEmptyRows)
        {
            Application xlsApp = CheckSettings(importFile, columnIndexStart, columnIndexEnd);

            List<List<string>> data = new List<List<string>>();

            Console.WriteLine("Import {0} FROM {1} to {2}", importFile, columnIndexStart, columnIndexEnd);

            Workbook xlsfile;

            string ConfiCulture = ExcelImport.Properties.Settings.Default.Culture;
            if (string.IsNullOrEmpty(ConfiCulture))
            {
                ConfiCulture = System.Threading.Thread.CurrentThread.CurrentCulture.ToString();
            }

            System.Globalization.CultureInfo systemCultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(ConfiCulture);

            //Nachfolgender try catch wurde hinterlegt, da bei manchen Excel-Dateien das öffnen des Workbooks mit
            //allen Parametern nicht funktioniert. Daher versucht die Anwendung im ersten Schritt das öffnen mit 
            //allen Parametern. Im zweiten Schritt wird die Datei übergeben. 
            //Aufgefallen xlsx Dateien AnD
            try
            {
                xlsfile = xlsApp.Workbooks.Open(
                   importFile,
                   0,
                   true,
                   5,
                   string.Empty,
                   string.Empty,
                   true,
                   XlPlatform.xlWindows,
                   string.Empty,
                   false,
                   false,
                   0,
                   false,
                   true,
                   false);
            }
            catch
            {
                xlsfile = xlsApp.Workbooks.Open(importFile);
            }


            Worksheet ws = (Worksheet)xlsfile.Sheets[SheetIndex];


            int ZaehlerLeerZeilen = 0;
            int i = 1;

            while (true)
            {
                string index_start = columnIndexStart + i;
                string index_end = columnIndexEnd + i;

                i++;

                List<string> lineContent = new List<string>();

                Range excelLine = ws.get_Range(index_start, index_end);

                Array array = (Array)excelLine.Cells.Value2;

                if (excelLine == null || excelLine.Cells == null || excelLine.Cells.Count < 1)
                {
                    data.Add(lineContent);
                    break;
                }

                int cellindex = 0;

                foreach (Range cell in excelLine.Cells)
                {
                    cellindex++;

                    if (cell.Value2 == null)
                    {
                        lineContent.Add(string.Empty);
                        Console.WriteLine(string.Empty);
                    }
                    else
                    {
                        // check for DateTime Column
                        if (dateTimeColumns.Contains(cellindex.ToString()))
                        {
                            try
                            {
                                double oleDate = Double.Parse(cell.Value2.ToString());
                                DateTime converted = DateTime.FromOADate(oleDate);
                                lineContent.Add(converted.ToShortDateString());
                            }
                            catch (Exception)
                            {
                                // Formatting Error
                                lineContent.Add(string.Empty);
                            }
                        }
                        else // simple string cell
                        {
                            lineContent.Add((cell.Value2.ToString()).Trim());
                            Console.WriteLine((cell.Value2.ToString()).Trim());
                        }
                    }
                }

                data.Add(lineContent);

                if (ignoreHeader && i == 2)
                {
                    // reset container to delete headerline
                    data = new List<List<string>>();
                }

                // check eof
                if (lineContent.Count == lineContent.Count(emptyline => emptyline == string.Empty))
                {
                    ZaehlerLeerZeilen = ZaehlerLeerZeilen + 1;
                    if (ZaehlerLeerZeilen >= IgnoreEmptyRows)
                    {
                        break;
                    }

                }
                else
                {
                    ZaehlerLeerZeilen = 0;
                }
            }

            xlsApp.DisplayAlerts = false;

            xlsApp.Quit();

            System.Threading.Thread.CurrentThread.CurrentCulture = systemCultureInfo;

            return data;
        }

        public static Application CheckSettings(string importFile, string columnIndexStart, string columnIndexEnd)
        {
            if (string.IsNullOrEmpty(importFile))
            {
                throw new Exception("Missing Setting ImportFile");
            }

            if (string.IsNullOrEmpty(columnIndexStart))
            {
                throw new Exception("Missing Setting ColumnIndexStart");
            }

            if (string.IsNullOrEmpty(importFile))
            {
                throw new Exception("Missing Setting ColumnIndexEnd");
            }

            if (!File.Exists(importFile))
            {
                throw new Exception(importFile + " does not exist !!!");                   
            }

            Application xlsApp = new Microsoft.Office.Interop.Excel.Application();

            xlsApp.Visible = false;

            if (xlsApp == null)
            {
                throw new Exception("No Excel on Server !!!");
            }

            return xlsApp;
        }
    }
}
