using PB.SpiralOfTest.Infrastructure.Helpers;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace PB.SpiralOfTest.Infrastructure.Headers
{
    internal class ProxyHeaderInterceptor : IClientMessageInspector //, IEndpointBehavior
    {
        #region IClientMessageInspector 

        object IClientMessageInspector.BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            // Add outgoing headers
            if (OperationContext.Current == null)
                HeaderHelpers.AddCommonHeaders(request.Headers);
            else
                HeaderHelpers.CopyCommonHeadersTo(request.Headers);
            return null;
        }

        void IClientMessageInspector.AfterReceiveReply(ref Message reply, object correlationState)
        {
            if (reply != null)
            {
                if (HeaderHelpers.CanCopyHeaders())
                {
                    HeaderHelpers.CopyCommonHeadersFrom(reply.Headers);
                }
            }
        }

        #endregion IClientMessageInspector 

        #region IEndpointBehavior

        //public void Validate(ServiceEndpoint endpoint)
        //{
        //    throw new NotImplementedException();
        //}

        //public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        //{
        //    throw new NotImplementedException();
        //}

        //public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        //{
        //    throw new NotImplementedException();
        //}

        //public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion IEndpointBehavior
    }
}
