using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BizTalkZombieManagement.Dal.Transport
{
    public static class FileLayerAccess
    {
        /// <summary>
        /// Save the message in Xml File
        /// </summary>
        /// <param name="messageInstanceId"> use for the file name</param>
        /// <param name="message"> the content</param>
        /// <param name="path">the directory</param>
        public static void SaveToFile(Guid messageInstanceId, String message,String path)
        {
            byte[] messagebyte = Encoding.UTF8.GetBytes(message);
            //write each byte
            using (FileStream fs =File.Create(System.IO.Path.Combine(path, messageInstanceId+".xml")))
            {
                foreach(byte bt in messagebyte)
                    fs.WriteByte(bt);

                fs.Flush();
            }
            
        }
    }
}
