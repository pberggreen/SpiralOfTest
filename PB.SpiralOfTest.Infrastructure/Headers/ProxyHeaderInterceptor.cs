using PB.SpiralOfTest.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace PB.SpiralOfTest.Infrastructure.Headers
{
    /*
    internal class ProxyHeaderInterceptor : IEndpointBehavior, IClientMessageInspector
    {
        object IClientMessageInspector.BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            if (OperationContext.Current == null)
                HeaderHelpers.AddCommonHeaders(request.Headers);
            else
                HeaderHelpers.CopyCommonHeadersTo(request.Headers);
        }

        void IClientMessageInspector.AfterReceiveReply(ref Message reply, object correlationState)
        {
            if (reply != null)
            {
                if (HeaderHelpers.CanCopyHeaders())
                    HeaderHelpers.CopyCommonHeadersFrom(reply.Headers);

            }
        }

        public void Validate(ServiceEndpoint endpoint)
        {
            throw new NotImplementedException();
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            throw new NotImplementedException();
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            throw new NotImplementedException();
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            throw new NotImplementedException();
        }
    }
    */
}
