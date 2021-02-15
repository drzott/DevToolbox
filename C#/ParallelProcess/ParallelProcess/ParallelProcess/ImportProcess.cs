using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParallelProcess
{
    public class ImportProcess
    {
        public ProcessType PType = ProcessType.APP;
        public int Level = 0;
        public string ProcessName = string.Empty;


        public ImportProcess(int level, ProcessType pt, string pName)
        {
            PType = pt;
            Level = level;
            ProcessName = pName;
        }

        public string GetDescription()
        {
            return string.Format("{0} {1} {2}", Level, PType, ProcessName);
        }
    }
}
