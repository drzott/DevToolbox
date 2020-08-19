using System;
using System.Collections;
using System.Reflection;
using Microsoft.SharePoint.Client;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Net.Mail;
using System.Web;

namespace SharePoint2Desktop
{
    
    class Program
    {
        static void Main(string[] args)
        {
           
            String savePath = @"C:\Temp\SharePointExport\";
            // Wo speichern?
            String webFullUrl = "http://companyweb/";
            // Url des SharePoint
            String listTitle = "Testdokumente";
            // Name der Liste
            string folderName = "";
            // Name des Ordners
            
            ClientContext cc = new ClientContext(webFullUrl);

            cc.Credentials = System.Net.CredentialCache.DefaultCredentials;



            FileInformation fileInfoxxxx = Microsoft.SharePoint.Client.File.OpenBinaryDirect(cc, "/Testdokumente/TEST.docx");


            var fileNamexxx = Path.Combine(@"C:\DEV\Work\Test.docx");
            using (var fileStream = System.IO.File.Create(fileNamexxx))
            {
                fileInfoxxxx.Stream.CopyTo(fileStream);
            }




            List list = cc.Web.Lists.GetByTitle(listTitle);
            var folder = list.RootFolder;
            cc.Load(folder);
            cc.ExecuteQuery();

            if (!folder.IsPropertyAvailable("Folders"))
            {
                cc.Web.Context.Load(folder, f => f.Folders);
                cc.Web.Context.ExecuteQuery();
            }
            cc.ExecuteQuery();

            var subfolder = folder.Folders.GetByUrl(folderName);
            cc.Load(subfolder);
            cc.ExecuteQuery();

            //if (!subfolder.IsPropertyAvailable("Files"))
            //{
            //    cc.Web.Context.Load(subfolder, s => s.Files);
            //    cc.Web.Context.ExecuteQuery();
            //}
            foreach (var file in folder.Files)
            {
                Console.Write(file.Name+" => ");

                if (!cc.Web.IsPropertyAvailable("ServerRelativeUrl"))
                {
                    cc.Web.Context.Load(cc.Web, c => c.ServerRelativeUrl);
                    cc.Web.Context.ExecuteQuery();
                }

                var FilePath = cc.Web.ServerRelativeUrl + "/" + "pit_vertrag" + "/" + folderName + "/";
                // pit_vertrag muss durch den logischen Namen der Liste ausgetauscht werden.

                FileInformation fileInfo = Microsoft.SharePoint.Client.File.OpenBinaryDirect(cc, FilePath + file.Name);

                var fileName = Path.Combine(savePath, file.Name);
                using (var fileStream = System.IO.File.Create(fileName))
                {
                    fileInfo.Stream.CopyTo(fileStream);
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Fertig.");
                Console.ForegroundColor = ConsoleColor.Gray; 
            }

            Console.WriteLine("Ende");
            Console.ReadKey();
        }
    }
}