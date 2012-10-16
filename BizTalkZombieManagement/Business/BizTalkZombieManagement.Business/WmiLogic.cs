using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using BizTalkZombieManagement.Entities.ConstantName;
using BizTalkZombieManagement.Entities.Entities;
using BizTalkZombieManagement.Dal;

namespace BizTalkZombieManagement.Business
{
    public class WmiLogic
    {
        #region member
        private List<WmiResult> _ListZombieMessage;
        public Boolean MessageFound { get; private set; }
        #endregion

        public WmiLogic()
        {
            _ListZombieMessage = new List<WmiResult>();
            MessageFound = false;
        }


        #region member
        /// <summary>
        /// return the number of message
        /// </summary>
        public Int32 Count
        {
            get
            {
                return _ListZombieMessage.Count();
            }
        }
        /// <summary>
        /// retrieve a list of message GUID
        /// </summary>
        public IEnumerable<Guid> ListMessageId
        {
            get
            {
                return _ListZombieMessage.Select(p => p.MessageInstanceId);
            }
        }
        #endregion

    
    
        #region method
        /// <summary>
        /// retrieve all zombie message for one biztalk orchestration service instance
        /// </summary>
        /// <param name="serviceInstanceId"></param>
        public void GetZombieMessage(Guid serviceInstanceId)
        {
            _ListZombieMessage = WmiAccess.GetZombieMessage(serviceInstanceId);

            for (int i = 0; i < _ListZombieMessage.Count; i++)
            {
                //check if the message type is not a system message type
                if (BTArtifactLogic.IsSystemSchema(_ListZombieMessage[i].MessageType))
                {
                    //if not so add to the list
                    _ListZombieMessage.RemoveAt(i);
                    i--;
                }
            }
            MessageFound = _ListZombieMessage.Count==0?false:true;
        }
        #endregion

        #region static method
        public static void TerminateOrchestration(Guid serviceInstanceId)
        {
            WmiAccess.TerminateOrchestration(serviceInstanceId);
        }
        #endregion
    }

}
