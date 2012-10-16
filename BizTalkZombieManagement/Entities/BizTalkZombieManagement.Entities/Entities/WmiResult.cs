using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BizTalkZombieManagement.Entities.Entities
{
    public class WmiResult
    {
        public String MessageType { get; set; }
        public Guid InstanceID { get; set; }
        public Guid MessageInstanceId { get; set; }
    }
}
