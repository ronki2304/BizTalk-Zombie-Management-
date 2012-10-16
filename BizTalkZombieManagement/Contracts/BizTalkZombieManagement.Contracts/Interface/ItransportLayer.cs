using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BizTalkZombieManagement.Contracts.CustomInterfaces
{
    public interface ITransportLayer
    {
        void SendMessage(String message, Guid messageInstanceId);
    }
}
