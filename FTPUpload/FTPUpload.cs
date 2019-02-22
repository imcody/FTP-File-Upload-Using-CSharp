using System;
using System.IO;
using System.Net;

namespace FTPUpload
{
    public class FTPUpload
    {
        public static string ServerName = "ftp://YourFtpServerName";
        public static string userName = "FTPUsername";
        public static string password = "FTPPassword";
        public static string filePath = "Full Path of the file that you want to upload, In case if you just name the file then do tweak the code below.";
        //Please feel free to make the variables dynamic.
        static void Main(string[] args)
        {
            #region Creating Directory where the file will be uploaded.
            System.Net.FtpWebRequest ftp_web_request = null;
            System.Net.FtpWebResponse ftp_web_response = null;

            string ftp_path = ServerName + "/NewFolder";

            try
            {
                ftp_web_request = (FtpWebRequest)WebRequest.Create(ftp_path);
                ftp_web_request.Method = WebRequestMethods.Ftp.MakeDirectory;
                ftp_web_request.Credentials = new NetworkCredential(userName, password);

                ftp_web_response = (FtpWebResponse)ftp_web_request.GetResponse();

                string ftp_response = ftp_web_response.StatusDescription;
                string status_code = Convert.ToString(ftp_web_response.StatusCode);

                ftp_web_response.Close();

            }
            catch (Exception Ex)
            {
                string status = Convert.ToString(Ex);

                Console.WriteLine("Failed to create folder.");
                return;
            } 
            #endregion
            #region Uploading File to the directory created
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create(new Uri(String.Format("{0}/{1}", ftp_path, Path.GetFileName(filePath))));
            req.Method = WebRequestMethods.Ftp.UploadFile;
            req.Credentials = new NetworkCredential(userName, password);
            Stream ftpStream = req.GetRequestStream();
            FileStream fs = File.OpenRead(filePath);
            byte[] buffer = new byte[1024];
            double total = (double)fs.Length;
            int byteRead = 0;
            do
            {
                byteRead = fs.Read(buffer, 0, 1024);
                ftpStream.Write(buffer, 0, byteRead);
            }
            while (byteRead != 0);
            fs.Close();
            ftpStream.Close();
            #endregion
        }
    }
}
