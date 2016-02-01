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

        protected TimeSpan Timeout
        {
            get
            {
#if DEBUG
                return DebugTimeout;
#else
                return DefaultTimeout;
#endif
            }
        }

        protected virtual string EnforceEndpointName
        {
            get
            {
                return typeof(TServiceContract).FullName.Replace("Contract", "Service");
            }
        }

        public TServiceContract Channel { get; private set; }

        protected CustomProxyBase()
        {
            var channelFactory = CreateFactory(DefaultTimeout, DefaultMaxMessageSize);
            Channel = channelFactory.CreateChannel();
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

        public R Call<R>(Func<T, R> action) 
        {
            try
            {
                return action(Channel);
            }
            finally
            {
                var comObj = Channel as ICommunicationObject;
                try
                {
                    if (comObj.State != CommunicationState.Faulted)
                        comObj.Close();
                }
                catch
                {
                    comObj.Abort(); 
                }
            }
        }

        public void Close()
        {
            var proxy = Channel as ICommunicationObject;
            proxy?.Close();
        }

        public void Abort()
        {
            var proxy = Channel as ICommunicationObject;
            proxy?.Abort();
        }

        public void Dispose()
        {
            Close();
        }
    }
}
