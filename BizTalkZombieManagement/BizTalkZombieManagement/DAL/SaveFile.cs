using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BizTalkZombieManagement.Dal
{
    public static class SaveFile
    {
        public static void SaveToFile(Guid MessageInstanceId, String Message,String Path)
        {
            byte[] messagebyte = Encoding.UTF8.GetBytes(Message);
            using (FileStream fs =File.Create(System.IO.Path.Combine(Path, MessageInstanceId+".xml")))
            {
                foreach(byte bt in messagebyte)
                    fs.WriteByte(bt);
            }
            
        }
    }
}
