using System.ServiceModel;
using System;
using System.ServiceModel.Channels;
namespace Microsoft.BizTalk.Adapter.Wcf.Runtime
{
    /// <summary>
    /// BizTalk Hack, the original class that implement this interface is not complete and we cannot herits from the original interface,
    /// So I recreate it for sending message without any tag
    /// </summary>
    [ServiceContract(Namespace = "http://www.microsoft.com/biztalk/2006/r2/wcf-adapter")]
    public interface ITwoWayAsyncVoid
    {

        // Methods

        [OperationContract(AsyncPattern = true, IsOneWay = false, Action = "*", ReplyAction = "*")]

        IAsyncResult BeginTwoWayMethod(System.ServiceModel.Channels.Message message, AsyncCallback callback, object state);

        [OperationContract(IsOneWay = false, Action = "BizTalkSubmit")]

        void BizTalkSubmit(System.ServiceModel.Channels.Message message);

        void EndTwoWayMethod(IAsyncResult result);

    }

}