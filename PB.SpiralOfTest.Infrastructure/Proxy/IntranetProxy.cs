using PB.SpiralOfTest.Infrastructure.Helpers;
using System;
using System.ServiceModel;

namespace PB.SpiralOfTest.Infrastructure.Proxy
{
    /// <summary>
    /// Base class for proxies to intranet services called via net.tcp
    /// </summary>
    public class IntranetProxy<TServiceContract> : CustomProxyBase<TServiceContract> where TServiceContract : class
    {
        private readonly string _hostName;

        protected IntranetProxy(string hostName)
        {
            _hostName = hostName;
        }

        protected override ChannelFactory<TServiceContract> CreateFactory(TimeSpan timeout, long maxMessageSize)
        {
            var binding = BindingHelpers.Intranet.Binding(maxMessageSize, timeout, DebugTimeout);
            var baseAddress = BindingHelpers.Intranet.CreateAddress(_hostName);
            var address = new EndpointAddress(BindingHelpers.CreateAddress(baseAddress, EnforceEndpointName));
            var channelFactory = new ChannelFactory<TServiceContract>(binding, address);
            //channelFactory.SetCredentials("SignedByCA");
            return channelFactory;
        }

    }
}
