using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FileMerger
{
    class Program
    {
        static void Main(string[] args)
        {
            FileMerge fm = new FileMerge(
                FileMerger.Properties.Settings.Default.SourceDirectory,
                FileMerger.Properties.Settings.Default.TargetFile,
                FileMerger.Properties.Settings.Default.ImportFiles
                );
        }
    }
}
