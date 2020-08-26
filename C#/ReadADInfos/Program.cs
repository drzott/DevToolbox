using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;
using System.IO;
using System.Data.SqlClient;

namespace ReadADInfos
{
    class Program
    {
        public static void Main(string[] args)
        {
            DirectorySearcher se = new DirectorySearcher();
            se.Filter = "(&(objectClass=group))";
            //se.SizeLimit = 0;
            se.PageSize = 100000;

            // groups

            SearchResultCollection results = se.FindAll();

            List<string> groups = new List<string>();

            foreach (SearchResult group in results)
            {
                var entry = new DirectoryEntry(group.Path);
                string shortName = entry.Name.Substring(3, entry.Name.Length - 3);

                if (!string.IsNullOrEmpty(shortName))
                    groups.Add(shortName);
            }

            // users

            se = new DirectorySearcher();

            se.Filter = "(&(objectClass=user))";
            //se.SizeLimit = 0;
            se.PageSize = 100000;

            results = null;         
            results = se.FindAll();

            List<User> users = new List<User>();

            foreach (SearchResult user in results)
            {
                User u = new User(user);
                users.Add(u);
            }


            if (ReadADInfos.Properties.Settings.Default.WriteFiles)
            {

                // Write Groups

                using (StreamWriter w = new StreamWriter(Path.Combine(ReadADInfos.Properties.Settings.Default.ExportDir, "Groups.txt")))
                {
                    foreach (string s in groups)
                    {
                        w.WriteLine(s);
                    }

                    w.Close();
                }

                // Write Users

                using (StreamWriter w = new StreamWriter(Path.Combine(ReadADInfos.Properties.Settings.Default.ExportDir, "Users.txt")))
                {
                    foreach (User u in users)
                    {
                        w.WriteLine(u.SamAccountName);
                        w.WriteLine(u.Firstname);
                        w.WriteLine(u.Lastname);
                        w.WriteLine(u.EMail);
                        w.WriteLine(u.Department);
                        w.WriteLine(u.UAC);
                        w.WriteLine(u.Wildcard);
                        w.WriteLine();

                        foreach (string g in u.Groups)
                        {
                            w.WriteLine(g);
                        }

                        w.WriteLine();

                        foreach (string p in u.AvailableProperties)
                        {
                            w.WriteLine(p);
                        }

                        w.WriteLine();
                        w.WriteLine();
                        w.WriteLine();

                    }

                    w.Close();
                }
            }


            if (ReadADInfos.Properties.Settings.Default.PersistToDatabase)
            {
                CheckDatabase();

                using (SqlConnection conn = new SqlConnection(ReadADInfos.Properties.Settings.Default.ConnectionString))
                {
                    string sql = "INSERT INTO AD_Import (SamAccountName, Firstname, Lastname, EMail, Department, UAC) Values (@s, @f, @l, @e,@d, @u)";

                    conn.Open();

                    foreach (User u in users)
                    {
                        if (string.IsNullOrEmpty(u.SamAccountName))
                            continue;

                        if (ReadADInfos.Properties.Settings.Default.PersistIfEmailAvailable && string.IsNullOrEmpty(u.EMail))
                            continue;

                        if (ReadADInfos.Properties.Settings.Default.PersistIfDepartmentAvailable && string.IsNullOrEmpty(u.Department))
                            continue;

                        SqlCommand cmd = new SqlCommand(sql, conn);

                        cmd.Parameters.AddWithValue("@s", u.SamAccountName);
                        cmd.Parameters.AddWithValue("@f", u.Firstname);
                        cmd.Parameters.AddWithValue("@l", u.Lastname);
                        cmd.Parameters.AddWithValue("@e", u.EMail);
                        cmd.Parameters.AddWithValue("@d", u.Department);
                        cmd.Parameters.AddWithValue("@u", u.UAC);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void CheckDatabase()
        {
            using (SqlConnection conn = new SqlConnection(ReadADInfos.Properties.Settings.Default.ConnectionString))
            {
                string sql = @"IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AD_Import')
                                BEGIN


		                                CREATE TABLE AD_Import(
			                                [Id] [bigint] IDENTITY(1,1) NOT NULL,
			                                [SamAccountname] [varchar](250) NULL,
			                                [FirstName] [varchar](250) NULL,
			                                [LastName] [varchar](250) NULL,
			                                [EMail] [varchar](250) NULL,
			                                [Department] [varchar](250) NULL,
			                                [UAC] [bigint] NULL,
		                                 CONSTRAINT [PK_AD_Import] PRIMARY KEY CLUSTERED 
		                                (
			                                [Id] ASC
		                                )
		                                )

                                END

                                TRUNCATE TABLE AD_Import
                                ";

                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.ExecuteNonQuery();
            }
        }

    }
}
