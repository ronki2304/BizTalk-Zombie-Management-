using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizTalkZombieManagement.Contracts.Interface;
using BizTalkZombieManagement.Dal.Transport;

namespace BizTalkZombieManagement.Business.Transport
{
    public class FileLogic : ItransportLayer
    {
        public FileLogic()
        {
        }

        private String getFilePath()
        {
            return ConfigParameter.FilePath;
        }
        public void SendMessage(string message, Guid messageInstanceID)
        {
            FileLayerAccess.SaveToFile(messageInstanceID, message, getFilePath());
        }
    }
}
