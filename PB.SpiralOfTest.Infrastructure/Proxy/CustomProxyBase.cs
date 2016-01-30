using System;
using System.ServiceModel;

namespace PB.SpiralOfTest.Infrastructure.Proxy
{
    public abstract class CustomProxyBase<T> where T : class
    {
        protected virtual TimeSpan DefaultTimeout => TimeSpan.FromMinutes(1);

        protected virtual TimeSpan DebugTimeout => TimeSpan.FromHours(1);

        protected virtual long DefaultMaxMessageSize => 65536;

        protected abstract ChannelFactory<T> CreateFactory(TimeSpan timeout, long maxMessageSize);

        //protected abstract EndpointAddress CreateAddress(string baseAddress);

        //protected abstract Binding CreateBinding();

        protected virtual string EnforceEndpointName
        {
            get
            {
                return typeof(T).FullName.Replace("Contract", "Service");
            }
        }

        private ChannelFactory<T> _channelFactory;

        public ChannelFactory<T> ChannelFactory
        {
            get
            {
                if (_channelFactory == null)
                {
                    _channelFactory = CreateFactory(DefaultTimeout, DefaultMaxMessageSize);
                }
                return _channelFactory;
            }
        }

        // TODO: Perhaps it is not a good idea to create and destroy a channel for each call?
        public void Call(Action<T> action)
        {
            var channel = ChannelFactory.CreateChannel();
            try
            {
                action(channel);
            }
            finally
            {
                var proxy = channel as ICommunicationObject;
                proxy?.Close();
            }
        }

    }
}
