using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Runtime;
using System.Diagnostics;

namespace SSG.KPI.Report.Util
{
    public static class ProcessUtil
    {
        [DllImport("user32.dll")]
        static extern int GetWindowThreadProcessId(int hWnd, out int lpdwProcessId);

        // Generische Listen mit Interop-Objekten können aus unerfindlichen Gründen nicht aus einer anderen Assembly aufgerufen werden

        //public static void KillOfficeApplications(Dictionary<object, ApplicationType> apps)
        //{
        //    foreach (KeyValuePair<object, ApplicationType> kvp in apps)
        //    {
        //        KillOfficeApplication(kvp.Key, kvp.Value, false);
        //    }

        //    apps.Clear();

        //    GC.Collect();
        //}

        //public static void KillOfficeApplications(List<Microsoft.Office.Interop.PowerPoint.Application> apps)
        //{
        //    foreach (Microsoft.Office.Interop.PowerPoint.Application app in apps)
        //    {
        //        KillOfficeApplication(app, ApplicationType.POWERPOINT, false);
        //    }

        //    apps.Clear();

        //    GC.Collect();
        //}

        //public static void KillOfficeApplications(List<Microsoft.Office.Interop.Excel.Application> apps)
        //{
        //    foreach (Microsoft.Office.Interop.Excel.Application app in apps)
        //    {
        //        KillOfficeApplication(app, ApplicationType.POWERPOINT, false);
        //    }

        //    apps.Clear();

        //    GC.Collect();
        //}

        public static void KillProcessById(int id)
        {
            try
            {
                Process p = Process.GetProcessById(id);
                p.Kill();

                GC.Collect();
            }
            catch (Exception) {; }
        }

        public static int GetProcessId(Microsoft.Office.Interop.PowerPoint.Application app)
        {
            return GetProcessId(app, ApplicationType.POWERPOINT);
        }

        public static int GetProcessId(Microsoft.Office.Interop.Excel.Application app)
        {
            return GetProcessId(app, ApplicationType.EXCEL);
        }

        public static int GetProcessId(object app, ApplicationType appType)
        {
            try
            {

                if (app == null || appType == ApplicationType.UNDEFINED)
                    return 0;

                int pId = 0;

                switch (appType)
                {
                    case ApplicationType.EXCEL:
                        {
                            GetWindowThreadProcessId(((Microsoft.Office.Interop.Excel.Application)app).Hwnd, out pId);
                            break;
                        }

                    case ApplicationType.POWERPOINT:
                        {
                            GetWindowThreadProcessId(((Microsoft.Office.Interop.PowerPoint.Application)app).HWND, out pId);
                            break;
                        }
                }

                return pId;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static void KillOfficeApplication(Microsoft.Office.Interop.PowerPoint.Application app, bool gc)
        {
            KillOfficeApplication(app, ApplicationType.POWERPOINT, gc);
        }

        public static void KillOfficeApplication(Microsoft.Office.Interop.Excel.Application app, bool gc)
        {
            KillOfficeApplication(app, ApplicationType.EXCEL, gc);
        }

        public static void KillOfficeApplicationById(int pId)
        {
            KillOfficeApplicationById(pId, true);
        }

        public static void KillOfficeApplicationById(int pId, bool gc)
        {
            try
            {
                using (Process p = Process.GetProcessById(pId))
                {
                    p.Kill();

                    if (gc)
                        GC.Collect();
                }

            }
            catch (Exception) {; }
        }

        public static void KillOfficeApplication(object app, ApplicationType appType, bool gc)
        {
            if (app == null || appType == ApplicationType.UNDEFINED)
                return;

            int pId = 0;

            try
            {

                switch (appType)
                {
                    case ApplicationType.EXCEL:
                        {
                            GetWindowThreadProcessId(((Microsoft.Office.Interop.Excel.Application)app).Hwnd, out pId);
                            break;
                        }

                    case ApplicationType.POWERPOINT:
                        {
                            GetWindowThreadProcessId(((Microsoft.Office.Interop.PowerPoint.Application)app).HWND, out pId);
                            break;
                        }
                }
            }
            catch (Exception)
            {
                return;
            }
            try
            {
                using (Process p = Process.GetProcessById(pId))
                {
                    p.Kill();

                    if (gc)
                        GC.Collect();
                }
            }
            catch (Exception) {; }
        }

        public static void KillOfficeApplication(object app, ApplicationType appType)
        {
            KillOfficeApplication(app, appType, true);
        }
    }

    public enum ApplicationType
    {
        UNDEFINED,
        EXCEL,
        POWERPOINT
    }
}