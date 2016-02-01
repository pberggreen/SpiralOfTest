using System;
using System.ServiceModel;

namespace PB.SpiralOfTest.Infrastructure.Proxy
{
    public abstract class CustomProxyBase<T> : IProxy<T> where T : class
    {
        protected virtual TimeSpan DefaultTimeout => TimeSpan.FromMinutes(1);

        protected virtual TimeSpan DebugTimeout => TimeSpan.FromHours(1);

        protected virtual long DefaultMaxMessageSize => 65536;

        protected abstract ChannelFactory<T> CreateFactory(TimeSpan timeout, long maxMessageSize);

        //protected abstract EndpointAddress CreateAddress(string baseAddress);

        //protected abstract Binding CreateBinding();

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
                return typeof(T).FullName.Replace("Contract", "Service");
            }
        }

        public T Channel { get; private set; }

        public CustomProxyBase()
        {
            var channelFactory = CreateFactory(DefaultTimeout, DefaultMaxMessageSize);
            Channel = channelFactory.CreateChannel();
        }

        public void Call(Action<T> action)
        {
            try
            {
                action(Channel);
            }
            catch (Exception ex)
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
