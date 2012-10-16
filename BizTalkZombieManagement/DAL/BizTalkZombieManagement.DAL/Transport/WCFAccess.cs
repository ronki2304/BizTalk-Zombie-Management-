#region using
using System;
using System.IO;
using System.ServiceModel;
using System.Xml;
#endregion
namespace BizTalkZombieManagement.Dal.Transport
{
    public class WcfAccess
    {
        ChannelFactory<Microsoft.BizTalk.Adapter.Wcf.Runtime.ITwoWayAsyncVoid> factory;
        Microsoft.BizTalk.Adapter.Wcf.Runtime.ITwoWayAsyncVoid client = null;
        public WcfAccess(String endPointName)
        {
           factory = new ChannelFactory<Microsoft.BizTalk.Adapter.Wcf.Runtime.ITwoWayAsyncVoid>(endPointName);
            client = factory.CreateChannel();
        }
        public void SendMessage (String message)
        {
            //create the new message to send over channel
            using (System.ServiceModel.Channels.Message msg = System.ServiceModel.Channels.Message.CreateMessage(System.ServiceModel.Channels.MessageVersion.Default
                , "BeginTwoWayMethod"
                , new XmlTextReader(new StringReader(message))))
            {
                //only async method exist
                IAsyncResult res = client.BeginTwoWayMethod(msg, null, null);


                //Blocks current thread until until AsyncWaitHandle receives a 
                res.AsyncWaitHandle.WaitOne();


                //End the call once the wait handle signal is received from BizTalk
                client.EndTwoWayMethod(res);
            }
        }

        ~WcfAccess()
        {
            ((IClientChannel)client).Close();
        }

   
    }
}
