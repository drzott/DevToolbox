using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;

using Report.Util;

namespace SSG.KPI.ReportGenerator
{
    public class ReportManager
    {
        private string _conn_FE = string.Empty;
        private string _conn_BE = string.Empty;

        public ReportManager(string conn_BE, string conn_FE)
        {
            _conn_FE = conn_FE;
            _conn_BE = conn_BE;
        }

        public List<string> Get_PPT_ReportRecipients(long reportId)
        {
            List<string> r = new List<string>();

            using (SqlConnection conn = new SqlConnection(_conn_FE))
            {
                conn.Open();

                string sql = @"SELECT
                                DISTINCT 
                                U.Email as Mail
                                FROM KPI_RT_Report_PP_Group as RG

		                                INNER JOIN ADMIN_RT_User_Group as RTG
			                                ON RG.FK_Group = RTG.FK_Group_ID

		                                INNER JOIN ADMIN_T_User as U
			                                ON RTG.FK_User_ID = U.PK_User_ID
                                WHERE RG.FK_Report_PP = @id AND ISNULL(U.Email,'') != ''
                                UNION
                                SELECT 
                                DISTINCT
                                U.Email as Mail
                                FROM KPI_RT_Report_PP_User as RU
		                                INNER JOIN ADMIN_T_User as U
				                                ON RU.FK_User = U.PK_User_ID	
                                WHERE RU.FK_Report_PP = @id AND ISNULL(U.Email,'') != ''
                            ";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", reportId);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if(dr["Mail"] != null && dr["Mail"] != DBNull.Value)
                            {
                                r.Add(dr["Mail"].ToString());
                            }
                        }
                    }

                }
                return r;
            }
        }

        public string GetKpiReportFolder(long kpiId, string basePath)
        {
            if (kpiId == 0 || string.IsNullOrEmpty(basePath))
                return string.Empty;

            string baseReportPath = Path.Combine(basePath, "Public");

            if (!Directory.Exists(baseReportPath))
                return string.Empty;

            DirectoryInfo[] d = (new DirectoryInfo(baseReportPath)).GetDirectories("(" + kpiId + ")*", SearchOption.TopDirectoryOnly);

            if(d == null || d.Count() < 1)
            {
                // no directory --> create one

                try
                {

                    string kpiName = string.Empty;

                    using (SqlConnection conn = new SqlConnection(_conn_FE))
                    {
                        conn.Open();

                        string sql = @"SELECT Description FROM KPI_T_OperatingNumber WHERE PK_OperatingNumber_ID = @id";

                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", kpiId);

                            object o = cmd.ExecuteScalar();

                            if (o == null || o == DBNull.Value)
                                return string.Empty;

                            kpiName = o.ToString();
                        }
                    }

                    string newDir = Path.Combine(baseReportPath, "(" + kpiId + ") " + kpiName);

                    Directory.CreateDirectory(newDir);
                    return newDir;
                }catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                    return string.Empty;
                }
            }
            else
            {
                return d[0].FullName;
            }
        }

        public void SheduleTasks(List<long> taskIds)
        {
            using (SqlConnection conn = new SqlConnection(_conn_FE))
            {
                conn.Open();

                string sql = "UPDATE KPI_T_Report_PP SET InProgress = 1 WHERE PK_Report_PP_ID = @id ";

                foreach (long id in taskIds)
                {
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<ReportTask> GetSheduledTasks()
        {
            List<ReportTask> tasks = new List<ReportTask>();

            using (SqlConnection conn = new SqlConnection(_conn_FE))
            {
                conn.Open();

                string sql = @"

                                SELECT 

                                PK_Report_PP_ID,
                                Interval,
                                I_W,
                                I_M,
                                [Days] as D

                                FROM KPI_T_Report_PP WHERE ISNULL(Active,0) = 1";

                // TODO: später noch Word Reports per Union mit aufnehmen

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            ReportTask t = new ReportTask();

                            t.TaskId = Convert.ToInt64(dr["PK_Report_PP_ID"]);

                            if (dr["Interval"] != DBNull.Value && dr["Interval"] != null)
                                t.InterVal = dr["Interval"].ToString();
                            else
                                t.InterVal = string.Empty;

                            if (dr["I_W"] != DBNull.Value && dr["I_W"] != null)
                                t.I_W = dr["I_W"].ToString();
                            else
                                t.I_W = string.Empty;

                            if (dr["I_M"] != DBNull.Value && dr["I_M"] != null)
                                t.I_M = dr["I_M"].ToString();
                            else
                                t.I_M = string.Empty;

                            if (dr["D"] != DBNull.Value && dr["D"] != null)
                                t.I_D = Convert.ToInt32(dr["D"].ToString());
                            else
                                t.I_D = 0;

                            tasks.Add(t);
                        }
                    }
                }
            }

            return tasks;
        }

        public List<ReportTask> GetOpenTasks()
        {
            List<ReportTask> tasks = new List<ReportTask>();

            using (SqlConnection conn = new SqlConnection(_conn_FE))
            {
                conn.Open();

                string sql = @"
                                SELECT 
                                    PK_Report_PP_ID, 
                                    InProgress, 
                                    ReportName,
                                    'PPT' as ReportType,
                                    EMail,
                                    Active,
                                    FK_OperatingNumber_ID

                                FROM KPI_T_Report_PP

                                WHERE InProgress IN (1,2,99)";

                // TODO: später noch Word Reports per Union mit aufnehmen

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            ReportTask t = new ReportTask();

                            t.TaskId = Convert.ToInt64(dr["PK_Report_PP_ID"]);

                            if (dr["EMail"] != DBNull.Value && dr["EMail"] != null)
                                t.EMail = Convert.ToBoolean(dr["EMail"]);
                            else
                                t.EMail = false;

                            if (dr["Active"] != DBNull.Value && dr["Active"] != null)
                                t.Active = Convert.ToBoolean(dr["Active"]);
                            else
                                t.Active = false;

                            if (dr["FK_OperatingNumber_ID"] != DBNull.Value && dr["FK_OperatingNumber_ID"] != null)
                                t.OperatingNumer = Convert.ToInt32(dr["FK_OperatingNumber_ID"]);
                            else
                                t.OperatingNumer = 0;

                            if (dr["ReportType"] != DBNull.Value && dr["ReportType"] != null)
                                t.Report = (ReportType)Enum.Parse(typeof(ReportType), dr["ReportType"].ToString(), true);
                            else t.Report = ReportType.UNDEFINED;

                            if (dr["ReportName"] != DBNull.Value && dr["ReportName"] != null)
                                t.ReportName = dr["ReportName"].ToString();
                            else
                                t.ReportName = string.Empty;

                            if (dr["InProgress"] != DBNull.Value && dr["InProgress"] != null)
                            {
                                switch (Convert.ToInt32(dr["InProgress"]))
                                {

                                    case 1:
                                        {
                                            t.Task = TaskType.CREATE_REPORT;
                                            break;
                                        }

                                    case 2:
                                        {
                                            t.Task = TaskType.CHECK_TEMPLATE;
                                            break;
                                        }

                                    case 99:
                                        {
                                            t.Task = TaskType.CREATE_TEMPLATE;
                                            break;
                                        }

                                    default:
                                        {
                                            t.Task = TaskType.UNDEFINED;
                                            break;
                                        }

                                }
                            }
                            else
                            {
                                t.Task = TaskType.UNDEFINED;
                            }

                            tasks.Add(t);
                        }
                    }
                }
            }

            foreach (ReportTask t in tasks)
            {
                t.Contents = LoadReportContents(t.TaskId);
            }

            return tasks;
        }

        public void Set_Task_Done(long taskId)
        {
            using (SqlConnection conn = new SqlConnection(_conn_FE))
            {
                conn.Open();

                string sql = "UPDATE KPI_T_Report_PP SET InProgress = 0 WHERE PK_Report_PP_ID = @id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", taskId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public byte[] Read_PPT_Template(long taskId)
        {
            using (SqlConnection conn = new SqlConnection(_conn_FE))
            {
                conn.Open();

                string sql = "Select Template FROM KPI_T_Report_PP WHERE PK_Report_PP_ID = @id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", taskId);

                    object o = cmd.ExecuteScalar();

                    if (o == DBNull.Value || o == null)
                        return null;

                    return (byte[])o;
                    
                }
            }
        }

        public void Delete_PPT_Template(long taskId)
        {
            using (SqlConnection conn = new SqlConnection(_conn_FE))
            {
                conn.Open();

                string sql = "UPDATE KPI_T_Report_PP SET Template = NULL WHERE PK_Report_PP_ID = @id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", taskId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Store_PPT_Template(byte[] template, long taskId)
        {
            using (SqlConnection conn = new SqlConnection(_conn_FE))
            {
                conn.Open();

                string sql = "UPDATE KPI_T_Report_PP SET Template = @t, TemplateFileName = 'Template.pptx' WHERE PK_Report_PP_ID = @id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", taskId);
                    cmd.Parameters.AddWithValue("@t", template);

                    cmd.ExecuteNonQuery();
                }
            }                
        }

        public void Store_PPT_Report(byte[] report, long taskId, string fileName)
        {
            using (SqlConnection conn = new SqlConnection(_conn_FE))
            {
                conn.Open();

                string sql = "UPDATE KPI_T_Report_PP SET Report = @r, ReportFileName = @f WHERE PK_Report_PP_ID = @id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", taskId);
                    cmd.Parameters.AddWithValue("@r", report);
                    cmd.Parameters.AddWithValue("@f", fileName);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<ReportContent> LoadReportContents(long taskId)
        {
            List<ReportContent> l = new List<ReportContent>();

            using (SqlConnection conn = new SqlConnection(_conn_FE))
            {
                conn.Open();

                string sql = @"SELECT 
                                PK_ReportContent_PP_ID, Type, Name, Caption, Source, Folder, FK_Operating_Number,
                                -- Die Spalten wurden vertauscht. Wird über Alias geregelt
    
                                [From] as Report_From,
                                From_Date as Report_From_Date,
                                From_Range as Report_From_Range,
                                [From_Value] as Report_From_Range_Int,
                                [To] as Report_To,
                                To_Date as Report_To_Date,
                                To_Range as Report_To_Range -- wird nicht verwendet

                                FROM KPI_T_ReportContents_PP WHERE FK_Report_PP_ID = @id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", taskId);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            ReportContent c = new ReportContent();

                            c.Id = Convert.ToInt64(dr["PK_ReportContent_PP_ID"]);

                            if (dr["FK_Operating_Number"] != DBNull.Value && dr["FK_Operating_Number"] != null)
                                c.OperatingNumberId = Convert.ToInt64(dr["FK_Operating_Number"]);
                            else
                                c.OperatingNumberId = 0;

                            if (dr["Type"] != DBNull.Value && dr["Type"] != null)
                                c.ContentType = (ReportContentType)Enum.Parse(typeof(ReportContentType), dr["Type"].ToString(), true);
                            else
                                c.ContentType = ReportContentType.UNDEFINED;

                            if (dr["Name"] != DBNull.Value && dr["Name"] != null)
                                c.Name = dr["Name"].ToString();
                            else
                                c.Name = string.Empty;

                            if (dr["Caption"] != DBNull.Value && dr["Caption"] != null)
                                c.Caption = dr["Caption"].ToString();
                            else
                                c.Caption = string.Empty;

                            if (dr["Source"] != DBNull.Value && dr["Source"] != null)
                                c.ReportSource = (dr["Source"].ToString());
                            else
                                c.ReportSource = string.Empty;

                            if (dr["Folder"] != DBNull.Value && dr["Folder"] != null)
                                c.ReportFile = dr["Folder"].ToString();
                            else
                                c.ReportFile = string.Empty;

                            // DateTime From und To sind in der Datenbank vertauscht

                            if (dr["Report_From_Date"] == null || dr["Report_From_Date"] == DBNull.Value)
                                c.Report_From_Date = DateTime.MinValue;
                            else
                                c.Report_From_Date = Convert.ToDateTime(dr["Report_From_Date"]);

                            if (dr["Report_To_Date"] == null || dr["Report_To_Date"] == DBNull.Value)
                                c.Report_To_Date = DateTime.MinValue;
                            else
                                c.Report_To_Date = Convert.ToDateTime(dr["Report_To_Date"]);

                            if (dr["Report_To"] == null || dr["Report_To"] == DBNull.Value)
                                c.Report_To = string.Empty;
                            else
                                c.Report_To = dr["Report_To"].ToString();

                            if (dr["Report_From"] == null || dr["Report_From"] == DBNull.Value)
                                c.Report_From = string.Empty;
                            else
                                c.Report_From = dr["Report_From"].ToString();

                            if (dr["Report_From_Range_Int"] == null || dr["Report_From_Range_Int"] == DBNull.Value)
                                c.Report_From_Range_int = 0;
                            else
                                c.Report_From_Range_int = Convert.ToInt32(dr["Report_From_Range_Int"]);

                            if (dr["Report_From_Range"] == null || dr["Report_From_Range"] == DBNull.Value)
                                c.Report_From_Range = string.Empty;
                            else
                                c.Report_From_Range = dr["Report_From_Range"].ToString();


                            l.Add(c);

                        }
                    }
                }

                return l;
            }
        }
    }
    public class ReportTask
    {
        public long TaskId;
        public string ReportName = string.Empty;
        public TaskType Task;
        public ReportType Report;
        public List<ReportContent> Contents = new List<ReportContent>();

        public bool Active = false;
        public bool EMail = false;
        public long OperatingNumer = 0;


        public string InterVal = string.Empty;
        public string I_W = string.Empty;
        public string I_M = string.Empty;
        public int I_D = 0;

        public List<ReportRange> ReportRanges
        {
            get
            {
                List<ReportRange> l = new List<ReportRange>();

                foreach(ReportContent c in Contents)
                {
                    ReportRange r = c.Range;

                    if (r == null)
                        continue;

                    // keine doppelten hinzufügen
                    if (l.Count(x => x.From == r.From && x.To == r.To) == 0)
                        l.Add(r);

                }

                return l;
            }
        }
    }

    public class ReportContent
    {
        public long Id;
        public long OperatingNumberId;
        public ReportContentType ContentType;
        public string Name = string.Empty;
        public string Caption = string.Empty;
        public string ReportFile = string.Empty;
        public string ReportSource;

        public string Report_To = string.Empty;
        public DateTime Report_To_Date = DateTime.MinValue;
        public int Report_From_Range_int = 0;
        public string Report_From_Range = string.Empty;

        public string Report_From = string.Empty;
        public DateTime Report_From_Date = DateTime.MinValue;

        public string Result = string.Empty;
        public string Result_Filename = string.Empty;
        public string Result_File = string.Empty;

        public ReportRange Range
        {
            get
            {
                ReportRange range = DateUtil.GetReportRange(Report_From, Report_To, Report_From_Range, Report_From_Range_int, Report_From_Date, Report_To_Date);

                // kann kein gültiger zeitraum ermittelt werden, wird automatisch ein Jahr angegeben.
                // dies gilt nur für KPI Elemente
                // XLS Elemente werden nicht verändert wenn kein Zeitraum angegeben ist --> kein Auswertungszeitraum als fallback nötig

                if(range == null && ContentType != ReportContentType.XLS)
                {
                    range = new ReportRange(DateTime.Today.AddYears(-1), DateTime.Today.AddDays(1));
                }

                return range;
            }
        }
        public string Range_Text
        {
            get
            {
                return DateUtil.GetReportRangeText(Report_From, Report_To, Report_From_Range, Report_From_Range_int, Report_From_Date, Report_To_Date);
            }
        }

    }

    public enum ReportContentType
    {
        UNDEFINED,
        KPI,
        XLS
    }

    public enum TaskType
    {
        UNDEFINED,
        CHECK_TEMPLATE,
        CREATE_REPORT,
        CREATE_TEMPLATE
    }

    public enum ReportType
    {
        UNDEFINED,
        PPT,
        DOC
    }
}
