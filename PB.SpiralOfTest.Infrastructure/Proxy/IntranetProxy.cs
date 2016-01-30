using PB.SpiralOfTest.Infrastructure.Helpers;
using System;
using System.Configuration;
using System.ServiceModel;

namespace PB.SpiralOfTest.Infrastructure.Proxy
{
    public class IntranetProxy<T> : CustomProxyBase<T> where T: class
    {
        protected override ChannelFactory<T> CreateFactory(TimeSpan timeout, long maxMessageSize)  //TODO: Expose these to the client?
        {
            var hostName = ConfigurationManager.AppSettings["HostName"];
            var binding = BindingHelpers.Intranet.Binding(maxMessageSize, timeout, DebugTimeout);
            var baseAddress = BindingHelpers.Intranet.CreateAddress(hostName);
            var address = new EndpointAddress(BindingHelpers.CreateAddress(baseAddress, EnforceEndpointName));
            return new ChannelFactory<T>(binding, address);
        }

    }
}
