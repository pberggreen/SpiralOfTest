using System;
using System.ServiceModel;

namespace PB.SpiralOfTest.Infrastructure.Proxy
{
    /// <summary>
    /// Base class for all wcf proxy classes. 
    /// </summary>
    public abstract class CustomProxyBase<TServiceContract> : IProxy<TServiceContract> where TServiceContract : class
    {
        protected virtual TimeSpan DefaultTimeout => TimeSpan.FromMinutes(1);

        protected virtual TimeSpan DebugTimeout => TimeSpan.FromHours(1);

        protected virtual long DefaultMaxMessageSize => 65536;

        protected abstract ChannelFactory<TServiceContract> CreateFactory(TimeSpan timeout, long maxMessageSize);

        protected virtual string EnforceEndpointName
        {
            get
            {
                return typeof(TServiceContract).FullName.Replace("Contract", "Service");
            }
        }

        private TServiceContract _channel;

        public TServiceContract Channel
        {
            get
            {
                if (_channel == null)
                {
                    var channelFactory = CreateFactory(DefaultTimeout, DefaultMaxMessageSize);
                    _channel = channelFactory.CreateChannel();
                }
                return _channel;
            }
        }

        public void Call(Action<TServiceContract> action)
        {
            try
            {
                action(Channel);
            }
            catch
            {
                Abort();
                throw;
            }
        }

        public TResponse Call<TResponse>(Func<TServiceContract, TResponse> action)
        {
            try
            {
                return action(Channel);
            }
            catch
            {
                Abort();
                throw;
            }
        }

        public void Close()
        {
            var proxy = Channel as ICommunicationObject;
            if (proxy != null && proxy.State != CommunicationState.Faulted)
            {
                proxy.Close();
            }
        }

        public void Abort()
        {
            var proxy = Channel as ICommunicationObject;
            if (proxy != null && proxy.State != CommunicationState.Faulted)
            {
                proxy.Abort();
            }
        }

        public void Dispose()
        {
            Close();
        }
    }
}
