using System;

namespace BizTalkZombieManagement.Contracts.CustomInterfaces
{
    public interface ITransportLayer
    {
        void SendMessage(String message, Guid messageInstanceId);
    }
}
