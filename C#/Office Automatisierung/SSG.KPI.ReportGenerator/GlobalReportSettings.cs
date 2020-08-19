using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSG.KPI.ReportGenerator
{
    public class GlobalReportSettings
    {
        public string ConnBe { get; set; }
        public string ConnFe { get; set; }
        public string ReportDirectory_PPT_Init { get; set; }
        public string ReportDirectory_PPT_Template { get; set; }
        public string ReportDirectory_PPT { get; set; }
        public string ReportDirectory { get; set; }

        public EMailSettings MailSettings = new EMailSettings();
    }
}
