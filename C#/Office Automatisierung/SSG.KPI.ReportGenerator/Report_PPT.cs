using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Drawing;

using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;

using SSG.KPI.Report.Util;

using SSG.DataService.KPI;
using SSG.DataService.Data.KPI.DTO;

using SSG.KPI.ReportExcel;
using System.Net.Mail;
using System.Net;

namespace SSG.KPI.ReportGenerator
{
    public class Report_PPT : IDisposable
    {
        private ReportManager _rm = null;

        private Dictionary<int, Application> _processes = new Dictionary<int, Application>();

        private ReportTask _task;
        private ReportLog _log = null;

        private string _templateDir = string.Empty;
        private string _reportBaseDir = string.Empty;
        private string _rp = string.Empty;

        private GlobalReportSettings _globalSettings = null;

        private string _reportDir
        {
            get
            {
                if (string.IsNullOrEmpty(_rp))
                {
                    _rp = Path.Combine(_reportBaseDir, _task.TaskId.ToString());

                    if (!Directory.Exists(_rp))
                        Directory.CreateDirectory(_rp);
                }

                return _rp;
            }
        }

        public Report_PPT(ReportTask task, string reportDir, string templateDir, ReportManager rm, GlobalReportSettings globalSettings)
        {
            _globalSettings = globalSettings;
            _rm = rm;
            _task = task;
            _reportBaseDir = reportDir;
            _templateDir = templateDir;
            LogType l;

            switch (task.Task)
            {
                case TaskType.CREATE_REPORT: l = LogType.CREATE_REPORT; break;
                case TaskType.CREATE_TEMPLATE:  l = LogType.CREATE_TEMPLATE; break;
                case TaskType.CHECK_TEMPLATE:   l = LogType.CHECK_TEMPLATE; break;
                default: l = LogType.UNDEFINED; break;
            }

            _log = new ReportLog(_reportDir, l);

            _log.Add_Log(string.Format("Report: {0} {1}", _task.TaskId, _task.ReportName));
        }

        public void AddErrorToLog(string error)
        {
            if (_log != null)
                _log.Add_Log(LogEventType.ERROR, error);
        }

        public void Process()
        {
            switch (_task.Task)
            {

                case TaskType.CREATE_REPORT:
                    {
                        _log.Add_Log("Report erstellen");
                        CreateReport();
                        break;
                    }

                case TaskType.CREATE_TEMPLATE:
                    {
                        _log.Add_Log("Vorlage erstellen");
                        CreateTemplate();
                        break;
                    }

                case TaskType.CHECK_TEMPLATE:
                    {
                        _log.Add_Log("Vorlage prüfen");
                        CheckTemplate();
                        break;
                    }

                default:
                    {

                        _log.Add_Log("Task nicht definiert");
                        break;
                    }
            }
        }

        #region Create Report

        private void CreateReport()
        {
            _log.Add_Log("Report Erstellung gestartet");
            _log.Add_Log("Vorlage aus Datenbank laden");

            byte[] t = _rm.Read_PPT_Template(_task.TaskId);

            if (t == null)
            {
                _log.Add_Log(LogEventType.ERROR, "Keine Vorlage in Datenbank vorhanden");
                return;
            }

            _log.Add_Log("Vorlage aus Datenbank geladen");

            string templateFile = Path.Combine(_reportDir, "Template_Report_Creation.pptx");
            string templateFile_pdf = Path.Combine(_reportDir, "Template_Report_Creation.pdf");

            File.WriteAllBytes(templateFile, t);

            _log.Add_Log("Vorlagendatei zur Verfügung erstellt");
            _log.Add_Log(templateFile);

            _log.Add_Log("Report Elemente sammeln");

            _log.Add_Log("Kennzahlen & Auswertungszeiträume");

            KPI_GetOperatingNumbersCalculated kc = new KPI_GetOperatingNumbersCalculated();

            Dictionary<ReportRange, List<OperatingNumber>> results = new Dictionary<ReportRange, List<OperatingNumber>>();

            foreach (ReportRange r in _task.ReportRanges)
            {
                _log.Add_Log(string.Format("Auswertungszeitraum: {0} {1}", r.From.ToString(), r.To.ToString()));

                List<OperatingNumber> kpis = kc.GetOperatingNumbersCalculatedForReporting(_globalSettings.ConnFe, _globalSettings.ConnBe, r.From, r.To);

                results.Add(r, kpis);

                _log.Add_Log("Berechnung abgeschlossen");


                foreach (ReportContent c in _task.Contents)
                {
                    // prüfen ob Report Element in den Auswertungszeitraum passt
                    if (c.ContentType != ReportContentType.KPI || c.Range.From != r.From || c.Range.To != r.To)
                        continue;

                    foreach (OperatingNumber k in kpis)
                    {
                        if (k.Id == c.OperatingNumberId)
                        {
                            // Kennzahl gefunden die zum Element passt

                            _log.Add_Log(string.Format("{0} {1} {2}\t\t{3} {4} {5}", c.Id, c.Name, c.Caption, r.From, r.To, k.Id));

                            switch (c.ReportSource.ToLower())
                            {
                                case "absolutwert":
                                    {
                                        string res = string.Empty;

                                        if (k.Result != null && k.Result.Result_Abs_Set)
                                        {
                                            if (string.IsNullOrEmpty(k.Result_Format))
                                            {
                                                res = k.Result.Result_Abs.ToString();
                                                _log.Add_Log(string.Format("{0} KPI {1}: {2}", c.Id, k.Id, res));
                                            }
                                            else
                                            {
                                                try
                                                {
                                                    res = k.Result.Result_Abs.ToString(k.Result_Format);
                                                    _log.Add_Log(res);
                                                }
                                                catch (Exception)
                                                {
                                                    res = k.Result.Result_Abs.ToString();
                                                    _log.Add_Log(string.Format("{0} KPI {1}: {2}", c.Id, k.Id, res));
                                                    _log.Add_Log("Ungültiges Format");
                                                    _log.Add_Log(res);
                                                }
                                            }
                                        }

                                        c.Result = res;
                                        break;
                                    }
                                case "zielwert":
                                    {
                                        string res = string.Empty;

                                        if (k.Result != null && k.Result.Result_Target_Set)
                                        {
                                            try
                                            {
                                                res = k.Result.Result_Target.ToString("#,##0.00%");
                                                _log.Add_Log(res);
                                            }
                                            catch (Exception)
                                            {
                                                res = k.Result.Result_Target.ToString();
                                                _log.Add_Log("Ungültiges Format");
                                                _log.Add_Log(res);
                                            }
                                        }

                                        c.Result = res;

                                        break;
                                    }
                                default:
                                    {
                                        _log.Add_Log("Ungültige Elementart");
                                        c.Result = string.Empty;
                                        break;
                                    }

                            }

                            break;
                        }
                    }
                }
            }

            _log.Add_Log("Excel Dateien vorbereiten");

            _log.Add_Log("Alte Dateien löschen");

            //foreach (FileInfo fi in (new DirectoryInfo(_reportDir)).GetFiles("*.xls*")) { fi.Delete(); }
            //foreach (FileInfo fi in (new DirectoryInfo(_reportDir)).GetFiles("*.bmp*")) { fi.Delete(); }

            foreach (FileInfo fi in (new DirectoryInfo(_reportDir)).GetFiles("*.xls*"))
            {
                if (fi.IsReadOnly == true)
                {
                    fi.IsReadOnly = false;
                }
                fi.Delete();
            }
            foreach (FileInfo fi in (new DirectoryInfo(_reportDir)).GetFiles("*.bmp*"))
            {
                if (fi.IsReadOnly == true)
                {
                    fi.IsReadOnly = false;
                }
                fi.Delete();
            }

            _log.Add_Log("Excel Dateien vorbereiten");

            List<CLExcelScreenShot> xls_tab = new List<CLExcelScreenShot>();
            List<CLExcelScreenShot> xls_chart = new List<CLExcelScreenShot>();

            foreach (ReportContent c in _task.Contents)
            {
                // prüfen ob Report Element ein Excel Element mit gültigen Dateinamen ist
                if (c.ContentType != ReportContentType.XLS || string.IsNullOrEmpty(c.ReportFile) || string.IsNullOrEmpty(c.ReportSource))
                    continue;

                try
                {

                    FileInfo fi = new FileInfo(c.ReportFile);
                    _log.Add_Log(fi.FullName);
                    File.Copy(fi.FullName, Path.Combine(_reportDir, fi.Name), true);
                    // Pfad auf die neue Excel umschreiben
                    c.ReportFile = Path.Combine(_reportDir, fi.Name);
                    // vorläufigen Dateinamen für den Screenshot vergeben
                    c.Result_Filename = Guid.NewGuid().ToString() + ".bmp";
                    c.Result_File = Path.Combine(_reportDir, c.Result_Filename);

                    _log.Add_Log(Path.Combine(_reportDir, fi.Name));

                    // da wir nicht wissen ob das Tabellenblatt ein Chart oder eine Tabelle enthält wird auf beide Arten ein Screenshot erstellt

                    CLExcelScreenShot tab_s = new CLExcelScreenShot();

                    tab_s.Pfad = c.ReportFile;
                    tab_s.TabName = c.ReportSource;
                    tab_s.TabTyp = CLExcelScreenShot.eTabTyp.Tabelle;
                    tab_s.BildName = c.Result_Filename;

                    xls_tab.Add(tab_s);

                    CLExcelScreenShot chart_s = new CLExcelScreenShot();

                    chart_s.Pfad = c.ReportFile;
                    chart_s.TabName = c.ReportSource;
                    chart_s.TabTyp = CLExcelScreenShot.eTabTyp.Chart;
                    chart_s.BildName = c.Result_Filename;

                    xls_chart.Add(chart_s);


                    // Auswertungszeitraum der Datei XLS Datei anpassen

                    if (c.Range != null)
                    {
                        _log.Add_Log("Auswertungszeitraum für Excel gesetzt --> Datenquelle anpassen");

                        using (Report_XLS xls = new Report_XLS())
                        {
                            xls.SetXlsReportRange(c.ReportFile, c.Range.From, c.Range.To, _log, _globalSettings.ConnBe);
                        }
                    }

                }
                catch (Exception ex)
                {
                    _log.Add_Log(LogEventType.ERROR, "Fehler bei der Ermittlung der Report Datei");
                    _log.Add_Log(ex.ToString());
                }
            }

            if (xls_tab.Count > 0)
            {
                _log.Add_Log("Screenshots Tab erstellen");

                new ExcelScreenShot().getScreenshots(xls_tab, _reportDir);
            }

            if (xls_tab.Count > 0)
            {
                _log.Add_Log("Screenshots Chart erstellen");
                new ExcelScreenShot().getScreenshots(xls_chart, _reportDir);
            }

            _log.Add_Log("Screenshots Excel Datei erstellt");

            _log.Add_Log("Alle Daten bereitgestellt. Report wird erstellt");

            Application app = new Application();
            _processes.Add(ProcessUtil.GetProcessId(app), app);
            _log.Add_Log("Powerpoint App initialisiert");


            app.DisplayAlerts = PpAlertLevel.ppAlertsNone;

            Presentation ppt = app.Presentations.Open(templateFile, MsoTriState.msoFalse, MsoTriState.msoFalse, MsoTriState.msoFalse);
            _log.Add_Log("Präsentation geöffnet");

            List<Slide> templateSlides = new List<Slide>();

            // Vorlagenfolie suchen und löschen
            foreach (Slide slide in ppt.Slides)
            {
                foreach (Microsoft.Office.Interop.PowerPoint.Shape shape in slide.Shapes)
                {
                    // alle Shapes nach dem vorlagenfolientext durchsuchen.
                    // falls gefunden handelt es sich um eine Vorlagenfolie
                    if (
                                shape != null
                           && shape.HasTextFrame == MsoTriState.msoTrue && shape.TextFrame != null
                           && shape.TextFrame.HasText == MsoTriState.msoTrue && shape.TextFrame.TextRange != null
                           && (!string.IsNullOrEmpty(shape.TextFrame.TextRange.Text))
                      )
                    {
                        if (shape.TextFrame.TextRange.Text == "|Report - Vorlagenfolie|")
                        {

                            // aus Liste löschen da beim Iterieren zur Laufzeit der Index verschoben wird
                            templateSlides.Add(slide);
                            break;
                        }
                    }
                }
            }

            foreach (Slide slide in templateSlides)
            {
                slide.Delete();
            }

            // Folien durchlaufen und Report-Elemente ersetzen

            foreach (Slide slide in ppt.Slides)
            {
                foreach (Microsoft.Office.Interop.PowerPoint.Shape shape in slide.Shapes)
                {

                    if (
                            shape != null
                            && shape.HasTextFrame == MsoTriState.msoTrue && shape.TextFrame != null
                            && shape.TextFrame.HasText == MsoTriState.msoTrue && shape.TextFrame.TextRange != null
                            && (!string.IsNullOrEmpty(shape.TextFrame.TextRange.Text))
                            )
                    {

                        #region Datumsplatzhalter ersetzen

                        // Prüfen ob es sich um ein Datums Platzhalter Shape handelt

                        string phText = shape.TextFrame.TextRange.Text;

                        if(phText.ToLower() == "|aktuelles datum|")
                        {
                            shape.TextFrame.TextRange.Text = DateUtil.GetReportDate("|aktuelles datum|");

                            if (shape.Line != null)
                            {
                                shape.Line.Visible = MsoTriState.msoFalse;
                            }

                            continue;
                        }

                        if (phText.ToLower() == "|aktuelles jahr|")
                        {
                            shape.TextFrame.TextRange.Text = DateUtil.GetReportDate("|aktuelles jahr|");

                            if (shape.Line != null)
                            {
                                shape.Line.Visible = MsoTriState.msoFalse;
                            }

                            continue;
                        }

                        if (phText.ToLower() == "|aktueller monat|")
                        {
                            shape.TextFrame.TextRange.Text = DateUtil.GetReportDate("|aktueller monat|");

                            if (shape.Line != null)
                            {
                                shape.Line.Visible = MsoTriState.msoFalse;
                            }

                            continue;
                        }

                        if (phText.ToLower() == "|aktuelles quartal|")
                        {
                            shape.TextFrame.TextRange.Text = DateUtil.GetReportDate("|aktuelles quartal|");

                            if (shape.Line != null)
                            {
                                shape.Line.Visible = MsoTriState.msoFalse;
                            }

                            continue;
                        }

                        if (phText.ToLower() == "|vorjahr|")
                        {
                            shape.TextFrame.TextRange.Text = DateUtil.GetReportDate("|vorjahr|");

                            if (shape.Line != null)
                            {
                                shape.Line.Visible = MsoTriState.msoFalse;
                            }

                            continue;
                        }

                        if (phText.ToLower() == "|letzter abgeschlossener monat|")
                        {
                            shape.TextFrame.TextRange.Text = DateUtil.GetReportDate("|letzter abgeschlossener monat|");

                            if (shape.Line != null)
                            {
                                shape.Line.Visible = MsoTriState.msoFalse;
                            }

                            continue;
                        }

                        if (phText.ToLower() == "|vorletzter abgeschlossener monat|")
                        {
                            shape.TextFrame.TextRange.Text = DateUtil.GetReportDate("|vorletzter abgeschlossener monat|");

                            if (shape.Line != null)
                            {
                                shape.Line.Visible = MsoTriState.msoFalse;
                            }

                            continue;
                        }

                        if (phText.ToLower() == "|letztes abgeschlossenes quartal|")
                        {
                            shape.TextFrame.TextRange.Text = DateUtil.GetReportDate("|letztes abgeschlossenes quartal|");

                            if (shape.Line != null)
                            {
                                shape.Line.Visible = MsoTriState.msoFalse;
                            }

                            continue;
                        }

                        if (phText.ToLower() == "|vorletztes abgeschlossenes quartal|")
                        {
                            shape.TextFrame.TextRange.Text = DateUtil.GetReportDate("|vorletztes abgeschlossenes quartal|");

                            if (shape.Line != null)
                            {
                                shape.Line.Visible = MsoTriState.msoFalse;
                            }

                            continue;
                        }

                        #endregion

                        // Prüfen ob es sich bei dem Shape um einen Platzhalter handelt
                        PPT_Report_Shape prs = new PPT_Report_Shape(shape.TextFrame.TextRange.Text);

                        if (!prs.IsValid)
                            continue;

                        // gültiges Shape
                        // Je nach Typ den Text ersetzen

                        foreach(ReportContent r in _task.Contents)
                        {
                            if(r.Id == prs.Id)
                            {
                                // je nach Typ werden verschiedene Sachen ersetzt
                                // Die Typen müssen eigentlich immer zusammen passen aber für den Fall dass das nicht so ist hat die Datenbank recht 
                                // da diese auch die entsprechenden Daten vorbereitet
                                if(r.ContentType == ReportContentType.KPI)
                                {
                                    // Bei KPIs steht immer schon das Richtige im Ergebnis
                                    // Unterscheidung zwischen Absolut und Zeilwert ist nicht notwendig

                                    if (string.IsNullOrEmpty(r.Result))
                                        shape.TextFrame.TextRange.Text = string.Empty;
                                    else
                                        shape.TextFrame.TextRange.Text = r.Result;

                                    // zusätzlich den Rahmen aus der Vorlage entfernen

                                    if(shape.Line != null)
                                    {
                                        shape.Line.Visible = MsoTriState.msoFalse;
                                    }

                                    _log.Add_Log(string.Format("Element {0} Wert: {1}", r.Id, r.Result));

                                }

                                if(r.ContentType == ReportContentType.XLS)
                                {
                                    // prüfen ob der Screenshot vorhanden und verfügbar ist

                                    try
                                    {

                                        if (!string.IsNullOrEmpty(r.Result_File))
                                        {
                                            Bitmap b = (Bitmap)Image.FromFile(r.Result_File);

                                            // Größe skalieren

                                            float slideHeight = shape.Height;
                                            float slideWidth = shape.Width;

                                            float picHeight = b.Height;
                                            float picWidth = b.Width;

                                            if ((slideHeight / slideWidth) < (picHeight / picWidth))
                                            {
                                                picWidth = (int)((slideHeight / picHeight) * picWidth);
                                                picHeight = slideHeight;
                                            }
                                            else
                                            {

                                                picHeight = (int)((slideWidth / picWidth) * picHeight);
                                                picWidth = slideWidth;
                                            }

                                            slide.Shapes.AddPicture(r.Result_File, MsoTriState.msoFalse, MsoTriState.msoTrue, shape.Left, shape.Top, picWidth, picHeight);

                                            shape.TextFrame.TextRange.Text = string.Empty;
                                            if (shape.Line != null)
                                                shape.Line.Visible = MsoTriState.msoFalse;

                                            _log.Add_Log(string.Format("Screenshot eingefügt {0}", r.Result_File));
                                        }
                                    }
                                    catch(Exception ex)
                                    {
                                        _log.Add_Log(string.Format("Fehler bei Screenshot {0}", r.Result_File));
                                        _log.Add_Log(ex.ToString());
                                        shape.TextFrame.TextRange.Text = string.Empty;
                                        if (shape.Line != null)                                        
                                            shape.Line.Visible = MsoTriState.msoFalse;

                                    }
                                }
                            }
                        }
                    }
                }
            }

            _log.Add_Log("Ersetzen der Report-Elemente abgeschlossen");

            ppt.SaveAs(templateFile, PpSaveAsFileType.ppSaveAsDefault, MsoTriState.msoFalse);
            ppt.SaveAs(templateFile_pdf, PpSaveAsFileType.ppSaveAsPDF, MsoTriState.msoFalse);
            _log.Add_Log(string.Format("Bericht gespeichert gespeichert: {0}", templateFile));

            ppt.Close();
            app.Quit();

            _log.Add_Log("Powerpoint App beendet");

            _log.Add_Log("Report Verteilung");

            try
            {
                string reportFileName = FileUtil.GetValidFilename(_task.ReportName);

                if (string.IsNullOrEmpty(reportFileName))
                    reportFileName = Guid.NewGuid().ToString();

                string reportFileName_pdf = reportFileName + ".pdf";
                reportFileName += ".pptx";

                string reportFile = Path.Combine(_reportDir, reportFileName);
                string reportFile_pdf = Path.Combine(_reportDir, reportFileName_pdf);

                File.Copy(templateFile, reportFile, true);
                File.Copy(templateFile_pdf, reportFile_pdf, true);

                _log.Add_Log(reportFile);

                _log.Add_Log("Report in Datenbank speichern");

                WriteReportToDatabase(reportFile, reportFileName);

                _log.Add_Log("Report in Datenbank gespeichert");

                _log.Add_Log(string.Format("Report Einstellung Aktiv: {0}", _task.Active));

                if (_task.Active)
                {
                    if(_task.OperatingNumer != 0)
                    {
                        _log.Add_Log(string.Format("Report wird bei Kennzahl {0} hinterlegt", _task.OperatingNumer));

                        string kpiPath = _rm.GetKpiReportFolder(_task.OperatingNumer, _globalSettings.ReportDirectory);

                        if (string.IsNullOrEmpty(kpiPath))
                        {
                            _log.Add_Log("KPI Pfad konnte nicht ermittelt werden");
                        }
                        else
                        {
                            _log.Add_Log(kpiPath);
                            File.Copy(templateFile, Path.Combine(kpiPath, reportFileName), true);
                            File.Copy(templateFile_pdf, Path.Combine(kpiPath, reportFileName_pdf), true);
                            _log.Add_Log("Report bei Kennzahl hinterlegt");
                        }
                    }

                    if (_task.EMail)
                    {
                        _log.Add_Log("E-Mail Empfänger laden");

                        List<string> rec = _rm.Get_PPT_ReportRecipients(_task.TaskId);

                        _log.Add_Log(rec.Count + " Empfänger");

                        if(rec.Count > 0)
                        {
                            _log.Add_Log("SMTP initialisieren");

                            if (!string.IsNullOrEmpty(_globalSettings.MailSettings.SMTP_Server))
                            {
                                SmtpClient smtp = null;

                                if (_globalSettings.MailSettings.SMTP_Port != 0)
                                {
                                    _log.Add_Log(string.Format("{0} {1}", _globalSettings.MailSettings.SMTP_Server, _globalSettings.MailSettings.SMTP_Port));
                                    smtp = new SmtpClient(_globalSettings.MailSettings.SMTP_Server, _globalSettings.MailSettings.SMTP_Port);
                                }
                                else
                                {
                                    _log.Add_Log(string.Format(_globalSettings.MailSettings.SMTP_Server));
                                    smtp = new SmtpClient(_globalSettings.MailSettings.SMTP_Server);
                                }

                                _log.Add_Log(string.Format("SSL: {0}", _globalSettings.MailSettings.SSL));

                                smtp.EnableSsl = _globalSettings.MailSettings.SSL;

                                if ((!string.IsNullOrEmpty(_globalSettings.MailSettings.Login)) && (!string.IsNullOrEmpty(_globalSettings.MailSettings.Password)))
                                {
                                    _log.Add_Log("Login & Passwort gesetzt");
                                    smtp.Credentials = new NetworkCredential(_globalSettings.MailSettings.Login, _globalSettings.MailSettings.Password);
                                }

                                using (MailMessage msg = new MailMessage())
                                {

                                    if (!string.IsNullOrEmpty(_globalSettings.MailSettings.Sender))
                                    {
                                        _log.Add_Log(string.Format("Absender: {0}", _globalSettings.MailSettings.Sender));

                                        msg.Sender = new MailAddress(_globalSettings.MailSettings.Sender, "KPI Report-Generator");
                                        msg.From = new MailAddress(_globalSettings.MailSettings.Sender, "KPI Report-Generator");
                                    }

                                    msg.Subject = "KPI Bericht" + _task.ReportName;
                                    msg.Body = string.Empty;

                                    foreach (string r in rec)
                                    {
                                        _log.Add_Log(r);
                                        msg.To.Add(r);
                                    }

                                    msg.Attachments.Add(new Attachment(reportFile));
                                    msg.Attachments.Add(new Attachment(reportFile_pdf));

                                    _log.Add_Log("E-Mail versenden");
                                    smtp.Send(msg);
                                    smtp.Dispose();
                                    _log.Add_Log("E-Mail versendet");
                                }

                            }
                            else
                            {
                                _log.Add_Log(LogEventType.ERROR, "Kein SMTP konfiguriert");
                            }
                        }
                    }
                }


            }catch(Exception ex)
            {
                _log.Add_Log(LogEventType.ERROR, string.Format("Fehler bei der Reportverteilung {0}", ex.ToString()));
            }
        }

            #endregion

            #region CheckTemplate

            private void CheckTemplate()
        {
            _log.Add_Log("Template prüfen gestartet");

            byte[] t = _rm.Read_PPT_Template(_task.TaskId);

            if(t == null)
            {
                _log.Add_Log(LogEventType.ERROR, "Keine Vorlage vorhanden");
                return;
            }

            try
            {
                bool newTemplate = false;

                _log.Add_Log("Vorlage aus Datenbank geladen");

                string templateFile = Path.Combine(_reportDir, "Template_Check.pptx");

                File.WriteAllBytes(templateFile, t);

                _log.Add_Log("Template Datei zur Prüfung erstellt");
                _log.Add_Log(templateFile);

                _log.Add_Log("Überprüfung gestartet");


                Dictionary<long, bool> rElements = new Dictionary<long, bool>();

                // alle Elemente des Reports werden später überprüft ob sie auf der vorlagenfolie enthalten sind
                // falls nicht werden sie wieder hinzugefügt
                foreach (ReportContent c in _task.Contents)
                {
                    rElements.Add(c.Id, false);
                }

                _log.Add_Log("Elemente vorbereitet");

                Application app = new Application();

                app.DisplayAlerts = PpAlertLevel.ppAlertsNone;

                _processes.Add(ProcessUtil.GetProcessId(app), app);
                _log.Add_Log("Powerpoint App initialisiert");

                Presentation ppt = app.Presentations.Open(templateFile, MsoTriState.msoFalse, MsoTriState.msoFalse, MsoTriState.msoFalse);
                _log.Add_Log("Präsentation geöffnet");

                if (rElements.Count > 0)
                {
                    _log.Add_Log("Überprüfung der Report-Elemente gestartet");

                    foreach (Slide slide in ppt.Slides)
                    {
                        foreach (Microsoft.Office.Interop.PowerPoint.Shape shape in slide.Shapes)
                        {

                            if (
                                    shape != null
                                    && shape.HasTextFrame == MsoTriState.msoTrue && shape.TextFrame != null
                                    && shape.TextFrame.HasText == MsoTriState.msoTrue && shape.TextFrame.TextRange != null
                                    && (!string.IsNullOrEmpty(shape.TextFrame.TextRange.Text))
                                    )
                            {

                                // Prüfen ob es sich bei dem Shape um einen Platzhalter handelt
                                PPT_Report_Shape prs = new PPT_Report_Shape(shape.TextFrame.TextRange.Text);

                                if (!prs.IsValid)
                                    continue;

                                // Id gefunden --> Element auf vorhanden setzen

                                long id = Convert.ToInt64(prs.Id);

                                if (rElements.ContainsKey(id))
                                    rElements[id] = true;
                            }
                        }
                    }

                    if(rElements.Count(x => x.Value == false) > 0)
                    {
                        _log.Add_Log("Fehlende Report-Elemente");
                        _log.Add_Log("Vorlagenfolie suchen");

                        foreach (Slide slide in ppt.Slides)
                        {
                            bool templateSlide = false;

                            foreach (Microsoft.Office.Interop.PowerPoint.Shape shape in slide.Shapes)
                            {
                                // alle Shapes nach dem vorlagenfolientext durchsuchen.
                                // falls gefunden handelt es sich um eine Vorlagenfolie
                                if (
                                            shape != null
                                       && shape.HasTextFrame == MsoTriState.msoTrue && shape.TextFrame != null
                                       && shape.TextFrame.HasText == MsoTriState.msoTrue && shape.TextFrame.TextRange != null
                                       && (!string.IsNullOrEmpty(shape.TextFrame.TextRange.Text))
                                  )
                                {
                                    if (shape.TextFrame.TextRange.Text == "|Report - Vorlagenfolie|")
                                    {

                                        // aus Liste löschen da beim Iterieren zur Laufzeit der Index verschoben wird
                                        templateSlide = true;
                                        break;
                                    }
                                }
                            }

                            if (templateSlide)
                            {
                                _log.Add_Log("Vorlagenfolie gefunden");

                                _log.Add_Log("Neue Elemente hinzufügen");

                                List<ReportContent> missing = new List<ReportContent>();

                                foreach(KeyValuePair<long, bool> kvp in rElements)
                                {
                                    if(kvp.Value == false)
                                    {
                                        foreach(ReportContent c in _task.Contents)
                                        {
                                            if(c.Id == kvp.Key)
                                            {
                                                missing.Add(c);
                                                break;
                                            }
                                        }                                            
                                    }
                                }

                                int x = 80;
                                int y = 160;

                                foreach (ReportContent c in missing)
                                {
                                    string shapeText = string.Empty;
                                    bool writeShape = false;

                                    if(c.ContentType == ReportContentType.KPI && c.ReportSource == "Absolutwert" && c.Caption != null && c.Id != 0)
                                    {
                                        shapeText = "|KPI WERT|";
                                        writeShape = true;
                                    }

                                    if (c.ContentType == ReportContentType.KPI && c.ReportSource == "Zielwert" && c.Caption != null && c.Id != 0)
                                    {
                                        shapeText = "|KPI ZIELWERT|";
                                        writeShape = true;
                                    }

                                    if (c.ContentType == ReportContentType.XLS && c.Caption != null && c.Id != 0 && (!string.IsNullOrEmpty(c.ReportFile)))
                                    {
                                        shapeText = "|Excel|";
                                        writeShape = true;
                                    }

                                    if (!writeShape)
                                        continue;


                                    y = y + 30;

                                    Microsoft.Office.Interop.PowerPoint.Shape tShape = slide.Shapes.AddTextbox(MsoTextOrientation.msoTextOrientationHorizontal, x, y, 250, 20);
                                    tShape.TextFrame.TextRange.Font.Size = 10;
                                    tShape.TextFrame.AutoSize = PpAutoSize.ppAutoSizeNone;

                                    Microsoft.Office.Interop.PowerPoint.LineFormat f = tShape.Line;
                                    f.Style = MsoLineStyle.msoLineSingle;
                                    f.BackColor.RGB = 256 * 65536 + 255 * 256 + 255;

                                    shapeText += string.Format("{0}| {1} {2}", c.Id, c.Caption, c.Range_Text);

                                    tShape.TextFrame.TextRange.Text = shapeText;

                                    _log.Add_Log(shapeText);

                                    // wenn wir zu weit unten in der Folie sind fangen wir oben wieder an aber versetzen die nächsten shapes etwas nach rechts
                                    if (y > 400)
                                    {
                                        y = 150;
                                        x = x + 50;
                                    }
                                }

                                _log.Add_Log("Änderungen an Template vorgenommen");
                                newTemplate = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        _log.Add_Log("Alle Report-elemente gefunden");
                    }
                }

                if (newTemplate)
                {
                    _log.Add_Log("angepasste Vorlage speichern");
                    string newTemplateFile = Path.Combine(_reportDir, "Template.pptx");
                    ppt.SaveAs(newTemplateFile, PpSaveAsFileType.ppSaveAsDefault, MsoTriState.msoFalse);
                    _log.Add_Log(string.Format("Vorlage gespeichert: {0}", newTemplateFile));

                    ppt.Close();
                    app.Quit();

                    Thread.Sleep(500);
                    // ein Sekündchen warten damit die Datei auch sauber freigegeben wird

                    WriteTemplateToDatabase(newTemplateFile);
                    _log.Add_Log("angepasste Vorlage in Datenbank gespeichert");
                }
                else
                {
                    ppt.Close();
                    app.Quit();
                }

                _log.Add_Log("Präsentation erfolgreich überprüft");

            }
            catch (Exception ex)
            {
                _log.Add_Log(LogEventType.ERROR, "Fehler bei der Überprüfung der Vorlage");
                _log.Add_Log(LogEventType.ERROR, "Vorlage wird gelöscht");
                _log.Add_Log(LogEventType.ERROR, ex.ToString());
                _rm.Delete_PPT_Template(_task.TaskId);
            }
        }

        #endregion

        #region Create Template

        private void CreateTemplate()
        {
            _log.Add_Log("Erstellung Template gestartet");

            Application app = new Application();

            app.DisplayAlerts = PpAlertLevel.ppAlertsNone;

            _processes.Add(ProcessUtil.GetProcessId(app), app);

            _log.Add_Log("Powerpoint App initialisiert");

            Presentation ppt = app.Presentations.Open(Path.Combine(_templateDir, "Template.pptx"), MsoTriState.msoFalse, MsoTriState.msoFalse, MsoTriState.msoFalse);
            _log.Add_Log("Präsentation geöffnet");

            CustomLayout cl = ppt.SlideMaster.CustomLayouts[PpSlideLayout.ppLayoutText];

            // neue Folie am Ende einfügen
            int templateSlide_Index = ppt.Slides.Count;
            Slide templateSlide = ppt.Slides.AddSlide(++templateSlide_Index, cl);
            _log.Add_Log("Vorlagenfolie erstellt");

            // Je nach vorlage sind bereits verschiedene Elemente auf der folie vorhanden
            // Zuerst die Vorlagenfolie bereinigen
            // Da die Reihenfolge der Shapes beim Durchlauf und Entfernen verändert wird das Ganze rest mal in eine Liste wegsichern damit
            // auch zuverlässig alles gelöscht wird

            // da die alte Interop Klasse noch vor generischen Klassen erzeugt wurde ist auch kein AddRange möglich...
            List<Microsoft.Office.Interop.PowerPoint.Shape> templateShapes = new List<Microsoft.Office.Interop.PowerPoint.Shape>();

            foreach (Microsoft.Office.Interop.PowerPoint.Shape shape in templateSlide.Shapes){ templateShapes.Add(shape);}
            foreach (Microsoft.Office.Interop.PowerPoint.Shape sd in templateShapes){ sd.Delete();}

            _log.Add_Log("Vorlagenfolie bereinigt");

            // Statische Elemente der vorlagenfolie hinzufügen

            // Eine Textbox oben links damit die Folie als Template Folie erkannt wird und später gelöscht werden kann
            Microsoft.Office.Interop.PowerPoint.Shape s = templateSlide.Shapes.AddTextbox(MsoTextOrientation.msoTextOrientationHorizontal, 0, 0, 30, 5);

            s.TextFrame.TextRange.Font.Size = 1;
            s.TextFrame.TextRange.Text = "|Report - Vorlagenfolie|";
            s.TextFrame.TextRange.Font.Color.RGB = 256 * 65536 + 255 * 256 + 255;
            s.Visible = MsoTriState.msoFalse;

            // Titel
            s = templateSlide.Shapes.AddTextbox(MsoTextOrientation.msoTextOrientationHorizontal, 0, 0, ppt.SlideMaster.Width, 40);

            s.TextFrame.TextRange.Font.Bold = MsoTriState.msoTrue;
            s.TextFrame.TextRange.Font.Size = 20;
            s.TextFrame.TextRange.Text = "KPI Reportgenerator - Vorlagenfolie";

            // Statischer Text

            s = templateSlide.Shapes.AddTextbox(MsoTextOrientation.msoTextOrientationHorizontal, 0, 40, ppt.SlideMaster.Width, 20);

            s.TextFrame.TextRange.Font.Size = 10;
            s.TextFrame.TextRange.Text = "Diese Folie enthält alle Vorlagenobjekte, die im KPI-Designer für diesen Report definiert wurden. Diese Objekte werden während der Report-Erstellung mit den entsprechenden Grafiken und Werten ausgetauscht.";

            s = templateSlide.Shapes.AddTextbox(MsoTextOrientation.msoTextOrientationHorizontal, 0, 60, ppt.SlideMaster.Width, 30);

            s.TextFrame.TextRange.Font.Size = 10;
            s.TextFrame.TextRange.Text = "Die Vorlagenobjekte können durch Kopieren oder Verschieben an der gewünschten Stelle eingefügt werden. Die Darstellung der Werte kann über die Formatierungseinstellungen in Powerpoint konfiguriert werden. Die Größe der entsprechenden Grafiken und Tabellen wird durch das Verändern der Größe des Vorlagenobjekts bestimmt";

            s = templateSlide.Shapes.AddTextbox(MsoTextOrientation.msoTextOrientationHorizontal, 0, 90, ppt.SlideMaster.Width, 20);

            s.TextFrame.TextRange.Font.Size = 10;
            s.TextFrame.TextRange.Text = "Bitte verändern Sie die Texte innerhalb der Vorlagenobjekte nicht. Diese Folie wird automatisch bei der Erstellung des Reports gelöscht.";
            s.TextFrame.TextRange.Font.Bold = MsoTriState.msoTrue;

            // horizontale Abgrenzungslinie
            s = templateSlide.Shapes.AddLine(0, 110, ppt.SlideMaster.Width, 110);
            s.Line.DashStyle = MsoLineDashStyle.msoLineSolid;
            s.Line.ForeColor.RGB = 0;

            _log.Add_Log("Statische Elemente der Vorlagenfolie erstellt");

            int kpis_x = 10;
            int kpis_y = 150;

            // Vorlagen für Datumsfelder

            for (int df = 0; df < 10; df++)
            {
                kpis_y = kpis_y + 30;

                Microsoft.Office.Interop.PowerPoint.Shape kpiShape = templateSlide.Shapes.AddTextbox(MsoTextOrientation.msoTextOrientationHorizontal, kpis_x, kpis_y, 100, 20);
                kpiShape.TextFrame.TextRange.Font.Size = 10;
                kpiShape.TextFrame.AutoSize = PpAutoSize.ppAutoSizeNone;

                Microsoft.Office.Interop.PowerPoint.LineFormat f = kpiShape.Line;
                f.Style = MsoLineStyle.msoLineSingle;
                f.BackColor.RGB = 256 * 65536 + 255 * 256 + 255;

                string text = string.Empty;

                switch (df)
                {
                    case 1: { text = "|Aktuelles Datum|"; break; }
                    case 2: { text = "|Aktuelles Jahr|"; break; }
                    case 3: { text = "|Aktueller Monat|"; break; }
                    case 4: { text = "|Aktuelles Quartal|"; break; }
                    case 5: { text = "|Vorjahr|"; break; }
                    case 6: { text = "|letzter abgeschlossener Monat|"; break; }
                    case 7: { text = "|letztes abgeschlossenes Quartal|"; break; }
                    case 8: { text = "|vorletzter abgeschlossener Monat|"; break; }
                    case 9: { text = "|vorletztes abgeschlossenes Quartal|"; break; }

                    default: continue;
                }

                kpiShape.TextFrame.TextRange.Text = text;
                _log.Add_Log(text);

                kpis_y = kpis_y + 30;
            }

            // Dynamische Elemente erstellen
            // Für jedes Element der Vorlage aus dem KPI-Designer wird ein neues Shape erstellt

            // Kennzahlen

            var kpis = _task.Contents.Where(x => x.ContentType == ReportContentType.KPI 
                                                && x.ReportSource == "Absolutwert"
                                                && x.Caption != null 
                                                && x.Id != 0);

            kpis_x = 50;
            kpis_y = 150;

            if(kpis != null && kpis.Count() > 0)
            {
                foreach(ReportContent c in kpis)
                {
                    kpis_y = kpis_y + 30;

                    Microsoft.Office.Interop.PowerPoint.Shape kpiShape = templateSlide.Shapes.AddTextbox(MsoTextOrientation.msoTextOrientationHorizontal, kpis_x, kpis_y, 250, 20);
                    kpiShape.TextFrame.TextRange.Font.Size = 10;
                    kpiShape.TextFrame.AutoSize = PpAutoSize.ppAutoSizeNone;

                    Microsoft.Office.Interop.PowerPoint.LineFormat f = kpiShape.Line;
                    f.Style = MsoLineStyle.msoLineSingle;
                    f.BackColor.RGB = 256 * 65536 + 255 * 256 + 255;

                    string text = string.Format("|KPI WERT|{0}| {1} {2}", c.Id, c.Caption, c.Range_Text);

                    kpiShape.TextFrame.TextRange.Text = text;

                    _log.Add_Log(text);

                    // wenn wir zu weit unten in der Folie sind fangen wir oben wieder an aber versetzen die nächsten shapes etwas nach rechts
                    if (kpis_y > 400)
                    {
                        kpis_y = 150;
                        kpis_x = kpis_x + 50;
                    }
                }
            }
            else
            {
                _log.Add_Log("Keine Absolutwerte in der Reportvorlage definiert");
            }

            // Zielwerte

            var tkpis = _task.Contents.Where(x => x.ContentType == ReportContentType.KPI 
                                                && x.ReportSource == "Zielwert"  
                                                && x.Caption != null 
                                                && x.Id != 0);

            int t_x = 300;
            int t_y = 150;

            if (tkpis != null && tkpis.Count() > 0)
            {
                foreach (ReportContent c in tkpis)
                {
                    t_y = t_y + 30;

                    Microsoft.Office.Interop.PowerPoint.Shape tShape = templateSlide.Shapes.AddTextbox(MsoTextOrientation.msoTextOrientationHorizontal, t_x, t_y, 250, 20);
                    tShape.TextFrame.TextRange.Font.Size = 10;
                    tShape.TextFrame.AutoSize = PpAutoSize.ppAutoSizeNone;

                    Microsoft.Office.Interop.PowerPoint.LineFormat f = tShape.Line;
                    f.Style = MsoLineStyle.msoLineSingle;
                    f.BackColor.RGB = 256 * 65536 + 255 * 256 + 255;

                    string text = string.Format("|KPI ZIELWERT|{0}| {1} {2}", c.Id, c.Caption, c.Range_Text);

                    tShape.TextFrame.TextRange.Text = text;

                    _log.Add_Log(text);

                    // wenn wir zu weit unten in der Folie sind fangen wir oben wieder an aber versetzen die nächsten shapes etwas nach rechts
                    if (t_y > 400)
                    {
                        t_y = 150;
                        t_x = t_x + 50;
                    }
                }
            }
            else
            {
                _log.Add_Log("Keine Zielwerte in der Reportvorlage definiert");
            }

            // Excel Pivot Diagramm & Tabellen

            var xls = _task.Contents.Where(x => x.ContentType == ReportContentType.XLS
                                                && x.Caption != null
                                                && x.Id != 0
                                                && (!string.IsNullOrEmpty(x.ReportFile)));

            int xls_x = 550;
            int xls_y = 150;

            if (xls != null && xls.Count() > 0)
            {
                foreach (ReportContent c in xls)
                {
                    // Bei xlsx werden zwei Vorlagen generiert (Diagramm & Tabelle)

                    xls_y = xls_y + 30;

                    Microsoft.Office.Interop.PowerPoint.Shape dShape = templateSlide.Shapes.AddTextbox(MsoTextOrientation.msoTextOrientationHorizontal, xls_x, xls_y, 250, 20);
                    dShape.TextFrame.TextRange.Font.Size = 10;
                    dShape.TextFrame.AutoSize = PpAutoSize.ppAutoSizeNone;

                    Microsoft.Office.Interop.PowerPoint.LineFormat f1 = dShape.Line;
                    f1.Style = MsoLineStyle.msoLineSingle;
                    f1.BackColor.RGB = 256 * 65536 + 255 * 256 + 255;

                    string text = string.Format("|Excel|{0}| {1} {2}", c.Id, c.Caption, c.Range_Text);
                    dShape.TextFrame.TextRange.Text = text;
                    _log.Add_Log(text);

                    //xls_y = xls_y + 30;

                    //Microsoft.Office.Interop.PowerPoint.Shape tabShape = templateSlide.Shapes.AddTextbox(MsoTextOrientation.msoTextOrientationHorizontal, xls_x, xls_y, 250, 20);
                    //tabShape.TextFrame.TextRange.Font.Size = 10;
                    //tabShape.TextFrame.AutoSize = PpAutoSize.ppAutoSizeNone;

                    //Microsoft.Office.Interop.PowerPoint.LineFormat f2 = tabShape.Line;
                    //f2.Style = MsoLineStyle.msoLineSingle;
                    //f2.BackColor.RGB = 256 * 65536 + 255 * 256 + 255;

                    //text = string.Format("|KPI Tabelle|{0}| {1}", c.Id, c.Caption);
                    //tabShape.TextFrame.TextRange.Text = text;

                    _log.Add_Log(text);

                    // wenn wir zu weit unten in der Folie sind fangen wir oben wieder an aber versetzen die nächsten shapes etwas nach rechts
                    if (xls_y > 400)
                    {
                        xls_y = 150;
                        xls_x = xls_x + 50;
                    }
                }
            }
            else
            {
                _log.Add_Log("Keine Zielwerte in der Reportvorlage definiert");
            }

            _log.Add_Log("Vorlagenfolie erstellt");

            string templateFile = Path.Combine(_reportDir, "Template.pptx");

            ppt.SaveAs(templateFile, PpSaveAsFileType.ppSaveAsDefault, MsoTriState.msoFalse);
            _log.Add_Log(string.Format("Vorlage gespeichert: {0}", templateFile));

            ppt.Close();
            app.Quit();

            ////// an dieser Stelle muss Powerpoint beendet werden damit die datei wieder gelesen werden kann

            ////ProcessUtil.KillOfficeApplication(app, true);
            Thread.Sleep(500);
            // ein Sekündchen warten damit die Datei auch sauber freigegeben wird

            // Datei als Template in der Datenbank speichern

            WriteTemplateToDatabase(templateFile);

            _log.Add_Log("Vorlagenerstellung abgeschlossen");
        }

        private void WriteTemplateToDatabase(string file)
        {
            // 3 Versuche, kann schonmal vorkommen dass das Freigeben der Datei länger dauert

            for(int i = 0; i < 3; i++)
            {
                try
                {
                    using (FileStream fs = new FileStream(file, FileMode.Open))
                    {
                        byte[] buffer = new byte[fs.Length];
                        fs.Read(buffer, 0, Convert.ToInt32(fs.Length));
                        fs.Close();

                        _rm.Store_PPT_Template(buffer, _task.TaskId);
                        _log.Add_Log("Vorlage in Datenbank gespeichert");
                        return;
                    }
                }catch(Exception ex)
                {
                    _log.Add_Log(LogEventType.ERROR, string.Format("Template speichern Versuch {0}: {1}", i, ex.ToString()));
                    Thread.Sleep(1000);
                }
            }
        }

        private void WriteReportToDatabase(string file, string fileName)
        {
            // 3 Versuche, kann schonmal vorkommen dass das Freigeben der Datei länger dauert

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    using (FileStream fs = new FileStream(file, FileMode.Open))
                    {
                        byte[] buffer = new byte[fs.Length];
                        fs.Read(buffer, 0, Convert.ToInt32(fs.Length));
                        fs.Close();

                        _rm.Store_PPT_Report(buffer, _task.TaskId, fileName);
                        _log.Add_Log("Bericht in Datenbank gespeichert");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    _log.Add_Log(LogEventType.ERROR, string.Format("Bericht speichern Versuch {0}: {1}", i, ex.ToString()));
                    Thread.Sleep(1000);
                }
            }
        }

        #endregion
        public void Dispose()
        {
            _rm.Set_Task_Done(_task.TaskId);
            _log.Add_Log("Task als erledigt gesetzt");

            _log.Add_Log("Prozesse bereinigen");

            foreach(KeyValuePair<int, Application> p in _processes)
            {
                ProcessUtil.KillOfficeApplicationById(p.Key, false);
            }

            _processes.Clear();

            _log.Add_Log("Prozesse bereinigt");
            _log.WriteLog();

            GC.Collect();
        }
    }

    public class PPT_Report_Shape
    {
        private string _shapeText = string.Empty;
        public bool IsValid {
            get
            {
                if (_shapeType != PPT_Report_Shape_Type.UNDEFINED && _shapeId != 0)
                    return true;

                return false;
            }
        }

        private PPT_Report_Shape_Type _shapeType = PPT_Report_Shape_Type.UNDEFINED;

        public PPT_Report_Shape_Type ShapeType
        {
            get
            {
                return _shapeType;
            }
        }

        private int _shapeId = 0;

        public int Id
        {
            get
            {
                return _shapeId;
            }
        }

        public PPT_Report_Shape(string shapeText)
        {
            _shapeText = shapeText;

            if (string.IsNullOrEmpty(_shapeText))
                return;

            // ein Shape ist gültig wenn:
            // mindestens 3 |
            // KPI WERT, KPI ZIELWERT, Excel
            // Ganzzahl zwischen dem ersten und dem zweiten |

            string[] split = _shapeText.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

            if (split == null || split.Count() < 2)
                return;

            switch (split[0])
            {
                case "KPI WERT": { _shapeType = PPT_Report_Shape_Type.KPI_WERT; break; }
                case "KPI ZIELWERT": { _shapeType = PPT_Report_Shape_Type.KPI_ZIEL; break; }
                case "Excel": { _shapeType = PPT_Report_Shape_Type.XLS; break; }
                default: { return; }
            }

            if (!Int32.TryParse(split[1], out _shapeId))
            {
                _shapeId = 0;
                return;
            }
        }
    }

    public enum PPT_Report_Shape_Type
    {
        UNDEFINED,
        KPI_WERT,
        KPI_ZIEL,
        XLS
    }

}
