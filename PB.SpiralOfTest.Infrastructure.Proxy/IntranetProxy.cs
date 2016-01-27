using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace PB.SpiralOfTest.Infrastructure.Proxy
{
    public class IntranetProxy<T> : CustomProxyBase<T> where T: class
    {
        protected override ChannelFactory<T> CreateFactory(TimeSpan timeout, long messageSize)
        {
            throw new NotImplementedException();
        }

        protected override EndpointAddress CreateAddress(Uri baseAddress)
        {
            throw new NotImplementedException();
        }

        protected override Binding CreateBinding()
        {
            throw new NotImplementedException();
        }
    }
}
