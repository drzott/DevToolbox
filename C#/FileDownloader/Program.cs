using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FileDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            WebClient wc = new WebClient();

            int i = 0;

            foreach(string s in FileDownloader.Properties.Settings.Default.Links)
            {
                i++;
                wc.DownloadFile(s, i + ".download");

            }
        }
    }
}
