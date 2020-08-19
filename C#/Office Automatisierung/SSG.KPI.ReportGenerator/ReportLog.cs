using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SSG.KPI.ReportGenerator
{
    public class ReportLog
    {
        public List<ReportLog_Entry> Logs = new List<ReportLog_Entry>();
        private string _LogPath;
        private LogType _logType;

        private string _filename
        {
            get {   return Path.Combine(_LogPath, "log_" + _logType.ToString() + ".txt");}
        }

        public ReportLog(string logPath)
        {
            _LogPath = logPath;
            _logType = LogType.UNDEFINED;
        }

        public ReportLog(string logPath, LogType logType)
        {
            _LogPath = logPath;
            _logType = logType;
        }

        public void Add_Log(string text)
        {
            Add_Log(LogEventType.INFO, text);
        }

        public void Add_Log(LogEventType eventType, string text)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("{0}\t{1}", eventType.ToString(), text));


            if (string.IsNullOrEmpty(text))
                Logs.Add(new ReportLog_Entry(eventType, string.Empty));
            else
                Logs.Add(new ReportLog_Entry(eventType, text));
        }

        public void WriteLog()
        {
            try
            {
                if (File.Exists(_filename))
                    File.Delete(_filename);

                using (StreamWriter wr = new StreamWriter(_filename, false))
                {
                    foreach(ReportLog_Entry e in Logs)
                    {
                        wr.WriteLine(string.Format("{0}\t{1}\t{2}", e.Timestamp.ToString(), e.EventType, e.Text));
                    }
                    wr.Close();
                }                
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Failed to write Log" + ex.ToString());
            }
        }
    }

    public class ReportLog_Entry
    {
        public DateTime Timestamp;
        public LogEventType EventType;
        public string Text = string.Empty;

        public ReportLog_Entry(LogEventType eventType, string text)
        {
            Timestamp = DateTime.Now;
            EventType = eventType;
            Text = text;
        }
    }

    public enum LogEventType
    {
        INFO,
        ERROR,
    }

    public enum LogType
    {
        UNDEFINED,
        CHECK_TEMPLATE,
        CREATE_REPORT,
        CREATE_TEMPLATE
    }
}
