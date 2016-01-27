using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace PB.SpiralOfTest.Infrastructure.Proxy
{
    public abstract class CustomProxyBase<T> where T : class
    {
        protected virtual TimeSpan DefaultTimeout => TimeSpan.FromMinutes(1);

        protected virtual long DefaultMaxMessageSize => 65536;

        protected abstract ChannelFactory<T> CreateFactory(TimeSpan timeout, long messageSize);

        protected abstract EndpointAddress CreateAddress(Uri baseAddress);

        protected abstract Binding CreateBinding();

    }
}
