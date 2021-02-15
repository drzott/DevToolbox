using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParallelProcess
{
    public class ProcessList
    {
        public List<ImportProcess> Processes = new List<ImportProcess>();

        public List<int> GetLevels()
        {
            List<int> l = new List<int>();

            foreach(ImportProcess ip in Processes)
            {
                if (!l.Contains(ip.Level))
                    l.Add(ip.Level);
            }

            l.Sort();

            return l;
        }

        public List<ImportProcess> GetProcessesPerLevel(int level)
        {
            List<ImportProcess> l = new List<ImportProcess>();

            foreach(ImportProcess ip in Processes)
            {
                if (ip.Level == level)
                    l.Add(ip);
            }

            return l;
        }
    }
}
