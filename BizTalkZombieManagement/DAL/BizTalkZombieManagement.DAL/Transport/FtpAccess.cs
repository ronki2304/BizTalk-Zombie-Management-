using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace BizTalkZombieManagement.Dal.Transport
{
    public static class FtpAccess
    {

        public static void PutMessage()
        {
            //FileInfo fileInf = new FileInfo(@"D:\BizTalk Test\msgZombie1.xml");
            // string uri = "ftp://192.168.3.42" + "/dump/" + fileInf.Name;
            // FtpWebRequest reqFTP;

            // // Create FtpWebRequest object from the Uri provided
            // reqFTP = (FtpWebRequest)FtpWebRequest.Create(uri);

            // reqFTP.Credentials = new NetworkCredential("tester", "tester");
            // // By default KeepAlive is true, where the control connection
            // // is not closed after a command is executed.
            // reqFTP.KeepAlive = false;

            // // Specify the command to be executed.
            // reqFTP.Method = WebRequestMethods.Ftp.UploadFileWithUniqueName;

            // // Specify the data transfer type.
            // reqFTP.UseBinary = true;

            // // Notify the server about the size of the uploaded file
            // reqFTP.ContentLength = fileInf.Length;

            // // The buffer size is set to 2kb
            // int buffLength = 2048;
            // byte[] buff = new byte[buffLength];
            // int contentLen;

            // // Opens a file stream (System.IO.FileStream) to read the file
            // // to be uploaded
            // FileStream fs = fileInf.OpenRead();

            // try
            // {
            //     // Stream to which the file to be upload is written
            //     Stream strm = reqFTP.GetRequestStream();

            //     // Read from the file stream 2kb at a time
            //     contentLen = fs.Read(buff, 0, buffLength);

            //     // Till Stream content ends
            //     while (contentLen != 0)
            //     {
            //         // Write Content from the file stream to the FTP Upload
            //         // Stream
            //         strm.Write(buff, 0, contentLen);
            //         contentLen = fs.Read(buff, 0, buffLength);
            //     }

            //     // Close the file stream and the Request Stream
            //     strm.Close();
            //     fs.Close();

            // }


            // catch (Exception ex)
            // {
            //     Console.WriteLine(ex.ToString());
            // }

        }
    }
}
