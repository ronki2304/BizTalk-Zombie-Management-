using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BizTalkZombieManagement.Dal
{
    public static class SaveFile
    {
        public static void SaveToFile(Guid messageInstanceId, String message,String path)
        {
            byte[] messagebyte = Encoding.UTF8.GetBytes(message);
            using (FileStream fs =File.Create(System.IO.Path.Combine(path, messageInstanceId+".xml")))
            {
                foreach(byte bt in messagebyte)
                    fs.WriteByte(bt);
            }
            
        }
    }
}
