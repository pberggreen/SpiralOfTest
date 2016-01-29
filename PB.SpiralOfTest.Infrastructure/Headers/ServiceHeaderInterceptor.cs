using PB.SpiralOfTest.Infrastructure.Helpers;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace PB.SpiralOfTest.Infrastructure.Headers
{
    internal class ServiceHeaderInterceptor: IDispatchMessageInspector
    {
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            // DESIGN NOTE: No header mgmt. here - the IncomingMessageHeaders collection is used as the service's header cache
            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            // DESIGN NOTE: Reply is actually null for one-way calls
            if (reply != null)
            {
                HeaderHelpers.CopyCommonHeadersTo(reply.Headers);
            }
        }
    }
}
