using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;

using FluentFTP;

namespace SFTP_Upload
{
    class Program
    {
        static void Main(string[] args)
        {

            string host = string.Empty;
            string user = string.Empty;
            string pw = string.Empty;


            using (FtpClient client = new FtpClient(host,21,user,pw))
            {
                client.EncryptionMode = FtpEncryptionMode.Explicit;
                client.SslProtocols = System.Security.Authentication.SslProtocols.Tls;
                client.ValidateCertificate += new FtpSslValidation(OnValidateCertificate);

                client.Connect();

                client.UploadFile(@"C:\DEV\Work\export.csv", "export.csv", FtpRemoteExists.Overwrite);
            }

            void OnValidateCertificate(FtpClient control, FtpSslValidationEventArgs e)
            {

                // Zertifikatsvalidierung für Testzwecke deaktiviert immer akzeptieren
                var cert = new X509Certificate(e.Certificate);
                e.Accept = true;
                Console.WriteLine("Zertifikat validiert");
            }
        }
    }
}
