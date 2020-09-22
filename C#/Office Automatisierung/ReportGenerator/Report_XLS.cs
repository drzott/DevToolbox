using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


using Microsoft.Office.Interop.Excel;

using Report.Util;

namespace ReportGenerator
{
    public class Report_XLS : IDisposable
    {
        private Dictionary<int, Application> _processes = new Dictionary<int, Application>();

        private ReportLog _log = null;

        private string _conn_BE = string.Empty;

        public void SetXlsReportRange(string xlsFile, DateTime from, DateTime to, ReportLog log, string conn_BE)
        {
            if (string.IsNullOrEmpty(xlsFile))
                return;

            if (string.IsNullOrEmpty(conn_BE))
                return;

            _conn_BE = conn_BE;

            if (from == DateTime.MinValue || to == DateTime.MinValue)
                return;

            //AnD Da manche Kunden Ihre Excel-Dateien immer auf ReadOnly setzten, muss das hier entfernt werden. 
            try
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(xlsFile);
                if (fi.IsReadOnly == true)
                {
                    fi.IsReadOnly = false;
                }
            }
            catch (Exception ex1)
            {
                _log.Add_Log(string.Concat("Fehler beim entfernen des Schreibschutz der Datei ",xlsFile));
                _log.Add_Log(ex1.ToString());
            }

            try
            {

                _log = log;

                _log.Add_Log(string.Format("Auswertungszeitraum setzen: {0} {1} {2}", xlsFile, from.ToString(), to.ToString()));

                _log.Add_Log("Excel vorbereiten");

                Application app = new Application();

                _processes.Add(ProcessUtil.GetProcessId(app), app);
                _log.Add_Log("Excel App initialisiert");

                app.DisplayAlerts = false;
                app.Visible = false;

                _log.Add_Log(string.Format("{0} öffnen", xlsFile));

                app.Workbooks.Open(xlsFile);

                _log.Add_Log("Datei geöffnet");
                _log.Add_Log("Datenquellen prüfen");

                foreach(PivotCache c in app.ActiveWorkbook.PivotCaches())
                {
                    // Wir prüfen ob im SQL die spalte "ReportRefDate" vorkommt. Falls ja ist dies die Spalte auf die der auswertungszeitraum gelegt wird.

                    string cmd = c.CommandText;

                    if (string.IsNullOrEmpty(cmd))
                        continue;

                    _log.Add_Log("Datenquelle alt:");
                    _log.Add_Log(cmd);

                    if (!(cmd.ToLower()).Contains("[report_refdate]"))
                        continue;

                    string cmdNew = string.Format("SELECT * FROM ( {0} ) as ReportFilter WHERE [Report_RefDate] >= '{1}' AND [Report_RefDate] <= '{2}' ", cmd, from.ToString("yyyyMMdd"), to.ToString("yyyMMdd"));

                    _log.Add_Log("Datenquelle neu:");
                    _log.Add_Log(cmdNew);

                    _log.Add_Log("Datenquelle prüfen");
                    if (CheckDataSource(cmd))
                    {
                        _log.Add_Log("Datenquelle erfolgreich geprüft");
                        c.CommandText = cmdNew;
                    }
                    else
                    {
                        _log.Add_Log("Datenquelle fehlerhaft. Alte Datenquelle wird beibehalten");
                        continue;
                    } 
                }

                _log.Add_Log("Datenquellen aktualisieren");

                app.ActiveWorkbook.RefreshAll();

                _log.Add_Log("Datei speichern");

                app.ActiveWorkbook.Save();

                _log.Add_Log("Excel App beenden");

                app.Workbooks.Close();
                app.Quit();
            }
            catch(Exception ex)
            {
                _log.Add_Log("Fehler beim Setzen des Auswertungszeitraums");
                _log.Add_Log(ex.ToString());

                return;
            }
        }

        public bool CheckDataSource(string cmd)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_conn_BE))
                {
                    conn.Open();

                    using (SqlCommand c = new SqlCommand(cmd, conn))
                    {
                        c.ExecuteScalar();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Add_Log(LogEventType.ERROR, ex.ToString());
                return false;
            }
        }

        public void Dispose()
        {

            foreach (KeyValuePair<int, Application> p in _processes)
            {
                ProcessUtil.KillOfficeApplicationById(p.Key, false);
            }

            _processes.Clear();

            GC.Collect();
        }
    }
}
