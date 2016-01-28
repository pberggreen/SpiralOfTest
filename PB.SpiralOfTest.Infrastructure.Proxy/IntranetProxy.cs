using System;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace PB.SpiralOfTest.Infrastructure.Proxy
{
    public class IntranetProxy<T> : CustomProxyBase<T> where T: class
    {
        protected override ChannelFactory<T> CreateFactory(TimeSpan timeout, long messageSize)
        {
            var baseAddress = ConfigurationManager.AppSettings["HostName"];  // TODO: BaseAddress or HostName ???
            var binding = CreateBinding();
            var address = CreateAddress(baseAddress);
            return new ChannelFactory<T>(binding, address);
        }

        protected override EndpointAddress CreateAddress(string baseAddress)
        {
            // Remove schema if present
            var index = baseAddress.IndexOf("//");
            if (index != -1)
            {
                baseAddress = baseAddress.Substring(index+2);
            }
            baseAddress = "net.tcp://" + baseAddress;
            var uriBuilder = new UriBuilder(baseAddress);
            uriBuilder.Path += EnforceEndpointName;
            return new EndpointAddress(uriBuilder.Uri);
        }

        protected override Binding CreateBinding()
        {
            var binding = new NetTcpBinding();
            binding.TransactionFlow = true;
            binding.ReliableSession.Enabled = true; 
            return binding;
        }

    }
}
