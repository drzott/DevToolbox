using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace Threading
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Process> running = new List<Process>();

            int p = Threading.Properties.Settings.Default.Files.Count;

            foreach (string xpath in Threading.Properties.Settings.Default.Files)
            {
                if(!File.Exists(xpath))
                    continue;

                try{

                    ProcessStartInfo psi = new ProcessStartInfo(xpath);
                    psi.WindowStyle = ProcessWindowStyle.Hidden;

                    Process ps = new Process();
                    ps.StartInfo = psi;
                    ps.Start();

                    running.Add(ps);

                    Console.WriteLine(string.Format("{0} gestartet", xpath));

                }catch(Exception ex){
                    Console.WriteLine(ex);
                }


                while(running.Count(x => !(x.HasExited)) >= Threading.Properties.Settings.Default.MaxImport)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("Waiting to start next process");
                }
            }
            

            while (running.Count(x => !(x.HasExited)) > 0)
            {
                Console.WriteLine(string.Format("Running Processes {0} / {1}", running.Count(x => !(x.HasExited)), p));
                Thread.Sleep(2500);
            }

            Console.WriteLine("************* Fertig **************");
        }
    }
}
