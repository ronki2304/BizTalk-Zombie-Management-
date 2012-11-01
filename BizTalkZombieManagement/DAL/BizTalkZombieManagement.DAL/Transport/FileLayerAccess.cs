using System;
using System.IO;
using System.Linq;
using System.Text;

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
        public static void SaveToFile(Guid messageInstanceId, String message, String path)
        {
            byte[] messagebyte = Encoding.UTF8.GetBytes(message);
            //write each byte
            using (FileStream fs = File.Create(System.IO.Path.Combine(path, messageInstanceId + ".xml")))
            {
                foreach (byte bt in messagebyte)
                    fs.WriteByte(bt);

                fs.Flush();
            }

        }

        /// <summary>
        /// check that current path is valid
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Boolean IsValidPathFolder(String path)
        {
            return !String.IsNullOrEmpty(path) && !path.Any(it => Path.GetInvalidPathChars().Contains(it)) && Directory.Exists(path);
        }
    }
}
