using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharepointExport
{
    public class DokumentInfo
    {
        public long ContractId { get; set; }
        public string WebDir { get; set; }
        public string  Filename { get; set; }


        public DokumentInfo(long contractId, string webDir, string filename)
        {
            ContractId = contractId;
            WebDir = webDir;
            Filename = filename;
        }



        public void DokInfo()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(ContractId + "\t\t" + WebDir + "\t\t" + Filename);
            Console.ResetColor();
        }
    }
}
