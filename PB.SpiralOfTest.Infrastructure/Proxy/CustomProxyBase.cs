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
            CloseOrAbortServiceChannel(Channel as ICommunicationObject);
            _channel = null;
        }

        public void Abort()
        {
            CloseOrAbortServiceChannel(Channel as ICommunicationObject);
            _channel = null;
        }

        public void Dispose()
        {
            Close();
        }

        /// <summary>
        /// Close or abort the channel
        /// </summary>
        /// <remarks>
        /// Taken from:
        /// https://devzone.channeladam.com/articles/2014/09/how-to-easily-call-wcf-service-properly/
        /// </remarks>
        private void CloseOrAbortServiceChannel(ICommunicationObject communicationObject)
        {
            bool isClosed = false;

            if (communicationObject == null || communicationObject.State == CommunicationState.Closed)
            {
                return;
            }

            try
            {
                if (communicationObject.State != CommunicationState.Faulted)
                {
                    communicationObject.Close();
                    isClosed = true;
                }
            }
            catch (CommunicationException)
            {
                // Catch this expected exception so it is not propagated further.
                // Perhaps write this exception out to log file for gathering statistics...
            }
            catch (TimeoutException)
            {
                // Catch this expected exception so it is not propagated further.
                // Perhaps write this exception out to log file for gathering statistics...
            }
            catch (Exception)
            {
                // An unexpected exception that we don't know how to handle.
                // Perhaps write this exception out to log file for support purposes...
                throw;
            }
            finally
            {
                // If State was Faulted or any exception occurred while doing the Close(), then do an Abort()
                if (!isClosed)
                {
                    AbortServiceChannel(communicationObject);
                }
            }
        }


        private static void AbortServiceChannel(ICommunicationObject communicationObject)
        {
            try
            {
                communicationObject.Abort();
            }
            catch (Exception)
            {
                // An unexpected exception that we don't know how to handle.
                // If we are in this situation:
                // - we should NOT retry the Abort() because it has already failed and there is nothing to suggest it could be successful next time
                // - the abort may have partially succeeded
                // - the actual service call may have been successful
                //
                // The only thing we can do is hope that the channel's resources have been released.
                // Do not rethrow this exception because the actual service operation call might have succeeded
                // and an exception closing the channel should not stop the client doing whatever it does next.
                //
                // Perhaps write this exception out to log file for gathering statistics and support purposes...
            }
        }
    }
}
