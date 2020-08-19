using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Drawing;

using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;

using SSG.KPI.Report.Util;

namespace SSG.KPI.ReportGenerator
{
    public class Generator_PPT
    {
        GlobalReportSettings _globalSettings = new GlobalReportSettings();

        ReportManager _rm = null;

        bool DidRunToday = false;

        public Generator_PPT(string connBE, string connFE, string reportPath, EMailSettings email)
        {
            Init(connBE, connFE, reportPath, email);
        }

        public Generator_PPT(Dictionary<string,string> settings, EMailSettings email)
        {
            Init(settings["BE"],settings["FE"], settings["ReportPath"], email);
        }

        public void Init(string connBE, string connFE, string reportPath, EMailSettings email)
        {
            // prüft ob alle Voraussetzungen gegeben sind um Reports zu erstellen
            // Wirft eine Exception damit der entsprechende Dienst garn icht erst startet --> keine Exception wird abgefangen

            System.Diagnostics.Debug.WriteLine("Init Report Generator PPT");

            _globalSettings.MailSettings = email;

            CheckParameters(connBE, connFE, reportPath);
            CheckDatabase(connBE, connFE);
            CheckReportDirectory(reportPath);
            CheckOfficePermissions();
            CreatePowerpointTemplate();

            _rm = new ReportManager(_globalSettings.ConnBe, _globalSettings.ConnFe);
        }

        public void DailyReporting(int reportHour)
        {
            DailyReporting(reportHour, false);
        }

        public void DailyReporting(int reportHour, bool saveLogToFile)
        {
            //System.Diagnostics.Debug.WriteLine(string.Format("Checking for DailyReports {0}", reportHour));

            SSG.KPI.ReportData.ReportLogging.Log(string.Format("SSG.KPI.ReportGenerator.DailyReporting Start - Checking for DailyReports {0}", reportHour), SSG.KPI.ReportData.ReportLogging.Stat.Info, null, null, saveLogToFile);

            if (DateTime.Now.Hour != reportHour)
            {
                SSG.KPI.ReportData.ReportLogging.Log("SSG.KPI.ReportGenerator.DailyReporting - Its no Time for creating Reports", SSG.KPI.ReportData.ReportLogging.Stat.Info, null, null, saveLogToFile);
                //System.Diagnostics.Debug.WriteLine("Its not time for creating reports");
                DidRunToday = false;
                return;
            }

            if(DidRunToday)
            {
                SSG.KPI.ReportData.ReportLogging.Log("SSG.KPI.ReportGenerator.DailyReporting - Report Creation did run today.", SSG.KPI.ReportData.ReportLogging.Stat.Warnung, null, null, saveLogToFile);
                //System.Diagnostics.Debug.WriteLine("Report Creation did run today");
                return;
            }

            //System.Diagnostics.Debug.WriteLine("Checking for sheduled reports");
            SSG.KPI.ReportData.ReportLogging.Log("SSG.KPI.ReportGenerator.DailyReporting - Checking for sheduled reports", SSG.KPI.ReportData.ReportLogging.Stat.Info, null, null, saveLogToFile);

            List<ReportTask> tasks = _rm.GetSheduledTasks();
            List<long> tasks_to_run = new List<long>();

            SSG.KPI.ReportData.ReportLogging.Log(string.Format("SSG.KPI.ReportGenerator.DailyReporting - Total Reports {0}",tasks.Count()), SSG.KPI.ReportData.ReportLogging.Stat.Info, null, null, saveLogToFile);


            foreach (ReportTask task in tasks)
            {
                SSG.KPI.ReportData.ReportLogging.Log(string.Format("SSG.KPI.ReportGenerator.DailyReporting - Report {0} - {1} - {2}",task.TaskId,task.ReportName,task.InterVal), SSG.KPI.ReportData.ReportLogging.Stat.Info, null, null, saveLogToFile);
                switch (task.InterVal.ToLower())
                {
                    case "täglich":
                        {
                            tasks_to_run.Add(task.TaskId);
                            break;
                        }

                    case "wöchentlich":
                        {
                            if (!string.IsNullOrEmpty(task.I_W))
                            {
                                char[] days = task.I_W.ToCharArray();

                                if (days == null || days.Length < 7)
                                    continue;

                                switch (DateTime.Today.DayOfWeek)
                                {
                                    case DayOfWeek.Monday:
                                        {
                                            if (days[0] == 'X')
                                                tasks_to_run.Add(task.TaskId);
                                            break;
                                        }
                                    case DayOfWeek.Tuesday:
                                        {
                                            if (days[1] == 'X')
                                                tasks_to_run.Add(task.TaskId);
                                            break;
                                        }
                                    case DayOfWeek.Wednesday:
                                        {
                                            if (days[2] == 'X')
                                                tasks_to_run.Add(task.TaskId);
                                            break;
                                        }
                                    case DayOfWeek.Thursday:
                                        {
                                            if (days[3] == 'X')
                                                tasks_to_run.Add(task.TaskId);
                                            break;
                                        }
                                    case DayOfWeek.Friday:
                                        {
                                            if (days[4] == 'X')
                                                tasks_to_run.Add(task.TaskId);
                                            break;
                                        }
                                    case DayOfWeek.Saturday:
                                        {
                                            if (days[5] == 'X')
                                                tasks_to_run.Add(task.TaskId);
                                            break;
                                        }
                                    case DayOfWeek.Sunday:
                                        {
                                            if (days[6] == 'X')
                                                tasks_to_run.Add(task.TaskId);
                                            break;
                                        }
                                }
                            }

                            break;
                        }

                    case "monatlich":
                        {
                            if (!string.IsNullOrEmpty(task.I_M))
                            {
                                char[] months = task.I_M.ToCharArray();

                                if (months == null || months.Length <= 12)
                                    continue;

                                if(months[(DateTime.Today.Month -1)] == 'X')
                                    tasks_to_run.Add(task.TaskId);
                            }

                            break;
                        }

                    default:
                        {
                            break;
                        }
                }
            }

            SSG.KPI.ReportData.ReportLogging.Log(string.Format("SSG.KPI.ReportGenerator.DailyReporting - Nr of task_to_run {0}", tasks_to_run.Count()), SSG.KPI.ReportData.ReportLogging.Stat.Info, null, null, saveLogToFile);

            _rm.SheduleTasks(tasks_to_run);

            DidRunToday = true;
            SSG.KPI.ReportData.ReportLogging.Log(string.Format("SSG.KPI.ReportGenerator.DailyReporting END"), SSG.KPI.ReportData.ReportLogging.Stat.Info, null, null, saveLogToFile);

        }

        public void Process()
        {
            Process(false);
        }

        public void Process(bool saveLogToFile)
        {
            SSG.KPI.ReportData.ReportLogging.Log(string.Format("SSG.KPI.ReportGenerator.Process Start"), SSG.KPI.ReportData.ReportLogging.Stat.Info, null, null, saveLogToFile);

            List<ReportTask> tasks = _rm.GetOpenTasks();

            SSG.KPI.ReportData.ReportLogging.Log(string.Format("SSG.KPI.ReportGenerator.Process - Total Reports {0}",tasks.Count()), SSG.KPI.ReportData.ReportLogging.Stat.Info, null, null, saveLogToFile);

            foreach (ReportTask task in tasks)
            {
                SSG.KPI.ReportData.ReportLogging.Log(string.Format("SSG.KPI.ReportGenerator.Process - Report {0} - {1} - {2}", task.TaskId, task.ReportName, task.InterVal), SSG.KPI.ReportData.ReportLogging.Stat.Info, null, null, saveLogToFile);

                if (task.Report != ReportType.PPT || task.Task == TaskType.UNDEFINED)
                    continue;

                switch (task.Task)
                {
                    case TaskType.CREATE_REPORT:
                        {
                            SSG.KPI.ReportData.ReportLogging.Log(string.Format("SSG.KPI.ReportGenerator.Process - CreateReport(task) {0} - {1} - {2}", task.TaskId, task.ReportName, task.InterVal), SSG.KPI.ReportData.ReportLogging.Stat.Info, null, null, saveLogToFile);
                            CreateReport(task);
                            break;
                        }

                    case TaskType.CREATE_TEMPLATE:
                        {
                            SSG.KPI.ReportData.ReportLogging.Log(string.Format("SSG.KPI.ReportGenerator.Process - CreateTemplate(task) {0} - {1} - {2}", task.TaskId, task.ReportName, task.InterVal), SSG.KPI.ReportData.ReportLogging.Stat.Info, null, null, saveLogToFile);
                            CreateTemplate(task);
                            break;
                        }

                    case TaskType.CHECK_TEMPLATE:
                        {
                            SSG.KPI.ReportData.ReportLogging.Log(string.Format("SSG.KPI.ReportGenerator.Process - CheckTemplate(task) {0} - {1} - {2}", task.TaskId, task.ReportName, task.InterVal), SSG.KPI.ReportData.ReportLogging.Stat.Info, null, null, saveLogToFile);
                            CheckTemplate(task);
                            break;
                        }


                    default: break;
                }
            }
        }

        #region Create Report
        private void CreateReport(ReportTask task)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Neuer Report Task: Report erstellen {0}", task.TaskId));
            try
            {

                using (Report_PPT report = new Report_PPT(task, _globalSettings.ReportDirectory_PPT, _globalSettings.ReportDirectory_PPT_Template, _rm, _globalSettings))
                {

                    try
                    {
                        report.Process();
                    }
                    catch (Exception e)
                    {
                        report.AddErrorToLog(e.ToString());
                        System.Diagnostics.Debug.WriteLine("Fehler Report erstellen: " + e.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Fehler Report erstellen: " + ex.ToString());
            }
        }

        #endregion

        #region Check Template

        private void CheckTemplate(ReportTask task)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Neuer Report Task: Template prüfen {0}", task.TaskId));
            try
            {

                using (Report_PPT report = new Report_PPT(task, _globalSettings.ReportDirectory_PPT, _globalSettings.ReportDirectory_PPT_Template, _rm, _globalSettings))
                {
                    try
                    {
                        report.Process();
                    }
                    catch (Exception e)
                    {
                        report.AddErrorToLog(e.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Fehler Template prüfen: " + ex.ToString());
            }
        }

        #endregion

        #region Create Template
        private void CreateTemplate(ReportTask task)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Neuer Report Task: Template erstellen {0}", task.TaskId));
            try
            {

                using (Report_PPT report = new Report_PPT(task, _globalSettings.ReportDirectory_PPT, _globalSettings.ReportDirectory_PPT_Template, _rm, _globalSettings)) {

                    try
                    {
                        report.Process();

                    }catch(Exception e)
                    {
                        report.AddErrorToLog(e.ToString());
                        System.Diagnostics.Debug.WriteLine("Fehler Template erstellen: " + e.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Fehler Template erstellen: " + ex.ToString());
            }
        }

        #endregion

        #region Init Checks

        private void CreatePowerpointTemplate()
        {
            // Erstellt die Powerpoint Vorlage die im Designer über "Vorlage herunterladen" zur Verfügung gestellt wird
            // Ist bereits eine Vorlage vorhanden, wird diese beibehalten

            if (!File.Exists(Path.Combine(_globalSettings.ReportDirectory_PPT_Template, "Logo.png"))){

                using (Stream s = (Assembly.GetExecutingAssembly()).GetManifestResourceStream("SSG.KPI.ReportGenerator.Logo.png"))
                {
                    Image i = Image.FromStream(s);
                    i.Save(Path.Combine(_globalSettings.ReportDirectory_PPT_Template, "Logo.png"));
                }
            }

            if (File.Exists(Path.Combine(_globalSettings.ReportDirectory_PPT_Template, "Template.pptx")))
                return;

            Application pptApp = null;
            int pId = 0;

            try
            {
                pptApp = new Application();
                pId = ProcessUtil.GetProcessId(pptApp, ApplicationType.POWERPOINT);

                pptApp.DisplayAlerts = PpAlertLevel.ppAlertsNone;

                Presentation ppt = pptApp.Presentations.Add(MsoTriState.msoFalse);

                ppt.SlideMaster.Shapes.AddPicture(Path.Combine(_globalSettings.ReportDirectory_PPT_Template,"Logo.png"), MsoTriState.msoFalse, MsoTriState.msoTrue, ppt.SlideMaster.Width - 300, 0, 266, 45);

                Microsoft.Office.Interop.PowerPoint.Shape s = ppt.SlideMaster.Shapes.AddTextbox(MsoTextOrientation.msoTextOrientationHorizontal, 0, ppt.SlideMaster.Height - 25, 400, 25);

                s.TextFrame.TextRange.Font.Bold = MsoTriState.msoTrue;
                s.TextFrame.TextRange.Font.Size = 12;
                //Zeile unten links
                //s.TextFrame.TextRange.Font.Color.RGB = 0 * 65536 + 0 * 256 + 255;
                s.TextFrame.TextRange.Text = string.Empty;

                ppt.SaveAs(Path.Combine(_globalSettings.ReportDirectory_PPT_Template, "Template.pptx"), PpSaveAsFileType.ppSaveAsDefault, MsoTriState.msoFalse);
                ppt.Close();

                ProcessUtil.KillOfficeApplication(pptApp, ApplicationType.POWERPOINT, true);

                ppt = null;
                pptApp = null;

                GC.Collect();
            }
            catch (Exception ex)
            {
                try
                {
                    ProcessUtil.KillOfficeApplication(pptApp, ApplicationType.POWERPOINT, true);
                }
                catch (Exception)
                {

                    ProcessUtil.KillProcessById(pId);

                }

                pptApp = null;

                throw new Exception("Error Creating PPT Init Template", ex);
            }
        }

        private void CheckOfficePermissions()
        {
            System.Diagnostics.Debug.WriteLine("Checking Office Permissions");

            Application pptApp = null;
            int pId = 0;

            try
            {


                // Prüft ob auf dem Rechner alle erforderlichen Rechte vorliegen um Reports zu erstellen
                // Es wird ein Testreport erstellt

                FileUtil.DeleteAllFilesInDirectory(_globalSettings.ReportDirectory_PPT_Init);

                pptApp = new Application();
                pId = ProcessUtil.GetProcessId(pptApp, ApplicationType.POWERPOINT);

                Presentation ppt = pptApp.Presentations.Add(MsoTriState.msoFalse);
                CustomLayout customLayout = ppt.SlideMaster.CustomLayouts[PpSlideLayout.ppLayoutText];
                Slides slides = ppt.Slides;
                _Slide slide = slides.AddSlide(1, customLayout);

                TextRange objText = slide.Shapes[1].TextFrame.TextRange;
                objText.Text = "Test1";
                objText.Font.Name = "Arial";
                objText.Font.Size = 20;

                ppt.SaveAs(Path.Combine(_globalSettings.ReportDirectory_PPT_Init,"Text.pptx"), PpSaveAsFileType.ppSaveAsDefault, MsoTriState.msoFalse);
                ppt.Close();

                ProcessUtil.KillOfficeApplication(pptApp, ApplicationType.POWERPOINT, true);

                ppt = null;
                pptApp = null;

                GC.Collect();

            }
            catch(Exception ex)
            {
                try
                {
                    ProcessUtil.KillOfficeApplication(pptApp, ApplicationType.POWERPOINT, true);
                }
                catch (Exception) {

                    ProcessUtil.KillProcessById(pId);

                }

                pptApp = null;

                throw new Exception("Error Creating Test PPT Report", ex);
            }
        }

        private void CheckReportDirectory(string reportPath)
        {
            // Prüfen ob Verzeichnisse existieren und wenn nötig anlegen
            // Sollte das nicht klappen liegt ein Rechteproblem vor --> Exception
            System.Diagnostics.Debug.WriteLine("Checking Report directory");
            if (!Directory.Exists(reportPath))
            {
                if (reportPath.Contains("\\"))
                    throw new Exception("Report Directory (Network share) is missing but cant be created !!!");

                Directory.CreateDirectory(reportPath);

            }

            _globalSettings.ReportDirectory = reportPath;

            if (!Directory.Exists(Path.Combine(reportPath, "Report")))
                Directory.CreateDirectory(Path.Combine(reportPath, "Report"));

            if (!Directory.Exists(Path.Combine(reportPath, @"Report\PPT")))
                Directory.CreateDirectory(Path.Combine(reportPath, @"Report\PPT"));

            _globalSettings.ReportDirectory_PPT = Path.Combine(reportPath, @"Report\PPT");

            if (!Directory.Exists(Path.Combine(reportPath, @"Report\PPT\Init")))
                Directory.CreateDirectory(Path.Combine(reportPath, @"Report\PPT\Init"));

            _globalSettings.ReportDirectory_PPT_Init = Path.Combine(reportPath, @"Report\PPT\Init");

            if (!Directory.Exists(Path.Combine(reportPath, @"Report\PPT\Template")))
                Directory.CreateDirectory(Path.Combine(reportPath, @"Report\PPT\Template"));

            _globalSettings.ReportDirectory_PPT_Template = Path.Combine(reportPath, @"Report\PPT\Template");

        }

        private void CheckParameters(string connBE, string connFE, string reportPath)
        {
            if (string.IsNullOrEmpty(connBE))
                throw new Exception("BE Connection String missing");

            if (string.IsNullOrEmpty(connFE))
                throw new Exception("FE Connection String missing");

            if (string.IsNullOrEmpty(reportPath))
                throw new Exception("Report Path missing");

            System.Diagnostics.Debug.WriteLine("Generator PPT Parameters Set");
        }

        private void CheckDatabase(string connBE, string connFE)
        {
            using (SqlConnection conn = new SqlConnection(connBE))
            {
                conn.Open();
                conn.Close();
            }

            _globalSettings.ConnBe = connBE;

            using (SqlConnection conn = new SqlConnection(connFE))
            {
                conn.Open();
                conn.Close();
            }

            _globalSettings.ConnFe = connFE;
        }
        #endregion
    }
}
