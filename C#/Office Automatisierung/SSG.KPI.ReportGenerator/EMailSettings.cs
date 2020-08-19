using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSG.KPI.ReportGenerator
{
    public class EMailSettings
    {
        public string SMTP_Server = string.Empty;
        public int SMTP_Port = 0;
        public bool SSL = false;
        public string Login = string.Empty;
        public string Password = string.Empty;
        public string Sender = string.Empty;

    }
}
