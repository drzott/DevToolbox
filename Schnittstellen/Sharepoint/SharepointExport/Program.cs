using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.IO;
using Microsoft.SharePoint.Client;
using System.Net;

namespace SharepointExport
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Magenta ;

            Console.WriteLine("********************************");
            Console.WriteLine("******* Sharepoint Import ******");
            Console.WriteLine("********************************");
            Console.WriteLine();
            Console.WriteLine();

            string workingDir = SharepointExport.Properties.Settings.Default.WorkingDir;
            string SPBaseUrl = SharepointExport.Properties.Settings.Default.SharepointBaseUrl;
            string SPUser = SharepointExport.Properties.Settings.Default.SharepointUser;
            string SPDomain = SharepointExport.Properties.Settings.Default.SharepointDomain;
            string SPPW = SharepointExport.Properties.Settings.Default.SharepointPassword;

            Console.WriteLine("Working Dir: " + workingDir);
            Console.WriteLine("Sharepoint Base: " + SPBaseUrl);


            Console.WriteLine("Preparing Working Dir");

            if (Directory.Exists(Path.Combine(workingDir, "Doc")))
            {
                Directory.Delete(Path.Combine(workingDir, "Doc"), true);
                Directory.CreateDirectory(Path.Combine(workingDir, "Doc"));
            }

            Console.WriteLine("Building Credentials");

            ClientContext cc = new ClientContext(SPBaseUrl);

            NetworkCredential cred = new NetworkCredential(SPUser, SPPW, SPDomain);

            cc.Credentials = new System.Net.CredentialCache();
            cc.Credentials = cred;

            Console.WriteLine();
            Console.WriteLine("Fetching Dokuments to Import");

            List<DokumentInfo> infos = LoadDocInfos();

            if(infos.Count < 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No Dokuments !!!");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Cleaning CRM Docs");

            CleanCRMDocs();

            Console.WriteLine();
            Console.WriteLine("Reading Files From Sharepoint");
            workingDir = Path.Combine(workingDir,"Doc");

            foreach(DokumentInfo info in infos)
            {
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Cyan;

                try
                {

                    string docWorkingDir = Path.Combine(workingDir, info.ContractId.ToString());
                    string filePath = Path.Combine(docWorkingDir, info.Filename);

                    Console.WriteLine(docWorkingDir);

                    if (!Directory.Exists(docWorkingDir))
                        Directory.CreateDirectory(docWorkingDir);

                    Console.WriteLine(filePath);

                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);

                    Console.WriteLine(info.WebDir);

                    string webUrl = "/" + info.WebDir + "/" + info.Filename;

                    Console.WriteLine(webUrl);

                    FileInformation fi = Microsoft.SharePoint.Client.File.OpenBinaryDirect(cc, webUrl);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("OK");
                    Console.ForegroundColor = ConsoleColor.Cyan;

                    using (var fileStream = System.IO.File.Create(filePath))
                    {
                        fi.Stream.CopyTo(fileStream);
                    }

                    Console.WriteLine("OK++");
                    Console.ForegroundColor = ConsoleColor.Cyan;

                    byte[] b = System.IO.File.ReadAllBytes(filePath);
                    Console.WriteLine("File loaded");
                    Console.WriteLine("Store File in DB");

                    string fileId = info.ContractId + "_Dokumente_Datei" + Guid.NewGuid();

                    Console.WriteLine("FileId: " + fileId);

                    using (SqlConnection conn = new SqlConnection(SharepointExport.Properties.Settings.Default.ConnectionString))
                    {
                        conn.Open();

                        string sql = "INSERT INTO Dokumente (L_Id, Dateiname, Datei) Values (@lid, @n, @d)";

                        SqlCommand cmd = new SqlCommand(sql, conn);

                        cmd.Parameters.AddWithValue("@n", info.Filename);
                        cmd.Parameters.AddWithValue("@d", fileId);
                        cmd.Parameters.AddWithValue("@lid", info.ContractId);

                        cmd.ExecuteNonQuery();


                        sql = @"INSERT INTO Files (DownloadId, Filename, FileSize, [File], TimeStamp, SourceTable, SourceColumn)
                                VALUES(@di, @fn, @fs, @f, GETDATE(),'Dokumente','Datei')";

                        cmd = new SqlCommand(sql, conn);

                        cmd.Parameters.AddWithValue("@di", fileId);
                        cmd.Parameters.AddWithValue("@fn", info.Filename);
                        cmd.Parameters.AddWithValue("@fs", b.Length);
                        cmd.Parameters.AddWithValue("@f", b);

                        cmd.ExecuteNonQuery();

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("File stored in database");

                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        public static void CleanCRMDocs()
        {
            using (SqlConnection conn = new SqlConnection(SharepointExport.Properties.Settings.Default.ConnectionString))
            {
                conn.Open();

                string sql = @" DELETE Dokumente;
                                DELETE Files";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.ExecuteNonQuery();
            }
        }

            public static List<DokumentInfo> LoadDocInfos()
        {
            List<DokumentInfo> infos = new List<DokumentInfo>();

            using (SqlConnection conn = new SqlConnection(SharepointExport.Properties.Settings.Default.ConnectionString))
            {
                conn.Open();

                string sql = @"
                                    SELECT 
                                    DISTINCT
                                    U.ID,
                                    DOC.DirNameComplete,
                                    DOC.FileName

                                    FROM Uebersicht as U
		                                    INNER JOIN
			                                    (

			                                    SELECT 
			                                    DISTINCT
			                                    CD.ContractId,
			                                    D.DirNameComplete,
			                                    D.FileName

			                                    FROM Import_Contract_Docs as CD
				                                    LEFT JOIN Import_Doc_Names as D
					                                    ON CD.DirName = D.DirName
			                                    WHERE D.Id IS NOT NULL
			                                    ) as DOC
			                                    ON U.ContractId = DOC.ContractId

                                    ORDER BY U.ID";

                SqlCommand cmd = new SqlCommand(sql, conn);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        DokumentInfo di = new DokumentInfo(Convert.ToInt64(dr["ID"]), dr["DirNameComplete"].ToString(), dr["Filename"].ToString());
                        di.DokInfo();
                        infos.Add(di);
                    }
                }
            }


            return infos;
        }
    }
}
