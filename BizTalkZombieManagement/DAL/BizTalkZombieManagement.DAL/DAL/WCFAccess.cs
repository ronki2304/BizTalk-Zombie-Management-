using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.IO;
using System.Xml;
using System.ServiceModel;

namespace BizTalkZombieManagement.DAL
{
    public class WCFAccess
    {
        ChannelFactory<Microsoft.BizTalk.Adapter.Wcf.Runtime.ITwoWayAsyncVoid> factory;

        public WCFAccess(String EndPointName="BTSEndPoint")
        {
            factory = new ChannelFactory<Microsoft.BizTalk.Adapter.Wcf.Runtime.ITwoWayAsyncVoid>(EndPointName);
        }
        public void sendMessage (String message)
        {
            
            var client = factory.CreateChannel();

            //create the new message to send over channel
            System.ServiceModel.Channels.Message msg = System.ServiceModel.Channels.Message.CreateMessage(System.ServiceModel.Channels.MessageVersion.Default
                , "BeginTwoWayMethod"
                , new XmlTextReader(new StringReader(message)));

            //only async method exist
            IAsyncResult res = client.BeginTwoWayMethod(msg, null, null);


            //Blocks current thread until until AsyncWaitHandle receives a 
            res.AsyncWaitHandle.WaitOne();


            //End the call once the wait handle signal is received from BizTalk

            client.EndTwoWayMethod(res);
            ((IClientChannel)client).Close();
        }
    }
}
