using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizTalkZombieManagement.Contracts.CustomInterfaces;
using BizTalkZombieManagement.Dal.Transport;

namespace BizTalkZombieManagement.Business.Transport
{
    public class FileLogic : ITransportLayer
    {
        public FileLogic()
        {
        }

        private static String getFilePath()
        {
            return ConfigParameter.FilePath;
        }
        public void SendMessage(string message, Guid messageInstanceId)
        {
            FileLayerAccess.SaveToFile(messageInstanceId, message, getFilePath());
        }

        public static Boolean IsValidPathFolder(String path)
        {
            return FileLayerAccess.IsValidPathFolder(path);
        }
    }
}
