using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading;
using System.Diagnostics;
using System.IO;


namespace ParallelProcess
{
    class Program
    {
        static void Main(string[] args)
        {
            /* ************************************************************************************
             * ************************************************************************************
             *                                    Parallel Process                                *    
             *                         
             * ************************************************************************************
             * ************************************************************************************
            
             Anwendungen werden je nach Stufe in Gruppen gestartet und ausgeführt

            Konfig:

            Stufe | Typ (SQL oder APP) | Je nach Typ entweder SQL Query oder Pfad zur EXE

            Beispiel:

            1|SQL|EXEC SP_IMPORT
            1|APP|C:\DEV\TEXT.exe

            */

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine("Parallele Verarbeitung gestartet");
            Console.WriteLine("Lese Konfiguration");
            Console.WriteLine();

            ProcessList pl = ReadConfiguration();


            foreach(ImportProcess ip in pl.Processes)
            {
                Console.WriteLine(ip.GetDescription());
            }

            Console.WriteLine();

            foreach(int level in pl.GetLevels())
            {
                List<Process> p = new List<Process>();
                List<Thread> t = new List<Thread>();

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(string.Format("Starte Prozesse Stufe: {0}", level));
                Console.ForegroundColor = ConsoleColor.Cyan;

                foreach(ImportProcess x in pl.GetProcessesPerLevel(level))
                {
                    switch (x.PType)
                    {
                        case ProcessType.APP:
                            {
                                if (!File.Exists(x.ProcessName))
                                    continue;

                                try
                                {
                                    Console.WriteLine(string.Format("Starting: {0}", x.GetDescription()));
                                    ProcessStartInfo psi = new ProcessStartInfo(x.ProcessName);
                                    //psi.WindowStyle = ProcessWindowStyle.Hidden;

                                    Process ps = new Process();
                                    ps.StartInfo = psi;
                                    ps.Start();

                                    p.Add(ps);

                                }catch(Exception ex)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine(ex.ToString());
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                }

                                break;
                            }

                        case ProcessType.SQL:
                            {
                                Thread sql = new Thread(new ParameterizedThreadStart(SQLProcess));
                                //sql.IsBackground = true;
                                sql.Start(x.ProcessName);
                                t.Add(sql);

                                break;
                            }

                        default: break;
                    }

                    while ((p.Count(a => !(a.HasExited)) + t.Count(b => b.ThreadState == System.Threading.ThreadState.Running)) >= ParallelProcess.Properties.Settings.Default.MaxProcess)
                    {
                        Thread.Sleep(1000);
                        Console.WriteLine("Waiting to start next process");
                    }
                }

                while (((p.Count(x => !(x.HasExited))) + (t.Count(b => b.ThreadState == System.Threading.ThreadState.Running)))   > 0)
                {
                    Console.WriteLine(string.Format("Running Processes {0} / {1}", p.Count(x => !(x.HasExited)) + t.Count(b => b.ThreadState == System.Threading.ThreadState.Running), p.Count() + t.Count()));
                    Thread.Sleep(2500);
                }
            }
        }

        public static void SQLProcess(object sql)
        {
            Console.WriteLine("SQL Start: " + sql.ToString());

            try
            {
                using (SqlConnection conn = new SqlConnection(ParallelProcess.Properties.Settings.Default.ConnectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(sql.ToString(), conn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.ExecuteNonQuery();
                    }
                }

            }catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());
                Console.ForegroundColor = ConsoleColor.Cyan;
            }
        }

        private static ProcessList ReadConfiguration()
        {
            ProcessList pl = new ProcessList();

            foreach(string s in ParallelProcess.Properties.Settings.Default.Prozesse)
            {
                string[] split = s.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                if (split.Count() != 3)
                    continue;

                if (string.IsNullOrEmpty(split[0]))
                    continue;

                if (string.IsNullOrEmpty(split[1]))
                    continue;

                if (string.IsNullOrEmpty(split[2]))
                    continue;


                int level = 0;

                try
                {
                    level = Convert.ToInt32(split[0]);

                }
                catch (Exception)
                {
                    continue;
                }

                ProcessType pt = ProcessType.APP;

                switch (split[1])
                {
                    case "APP": { pt = ProcessType.APP; break; }
                    case "SQL": { pt = ProcessType.SQL; break; }
                    default: continue;

                }

                pl.Processes.Add(new ImportProcess(level, pt, split[2]));
            }

            return pl;
        }
    }
}
