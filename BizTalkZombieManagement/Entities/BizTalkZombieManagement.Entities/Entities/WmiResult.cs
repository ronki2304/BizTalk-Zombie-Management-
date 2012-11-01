using System;

namespace BizTalkZombieManagement.Entities.Entities
{
    public class WmiResult
    {
        public String MessageType { get; set; }
        public Guid InstanceId { get; set; }
        public Guid MessageInstanceId { get; set; }
    }
}
