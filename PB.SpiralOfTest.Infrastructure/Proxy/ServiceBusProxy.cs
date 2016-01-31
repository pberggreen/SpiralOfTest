using System;
using System.ServiceModel;
using Microsoft.ServiceBus;
using PB.SpiralOfTest.Infrastructure.Helpers;

namespace PB.SpiralOfTest.Infrastructure.Proxy
{
    /// <summary>
    /// Proxy class for calling services via the Azure service bus
    /// </summary>
    public abstract class ServiceBusProxy<T> : CustomProxyBase<T> where T : class
    {
        protected string ServiceBusConnectionString { get; set; }

        protected string QueueName { get; set; }

        protected ServiceBusProxy(string serviceBusConnectionString, string queueName)
        {
            ServiceBusConnectionString = serviceBusConnectionString;
            QueueName = queueName;
        }

        protected override ChannelFactory<T> CreateFactory(TimeSpan timeout, long maxMessageSize)
        {
            var connectionStringParts = ServiceBusConnectionString.Split(';');
            var endpoint =
                connectionStringParts[0].Substring(connectionStringParts[0].IndexOf("=", StringComparison.Ordinal) + 1);
            var sharedAccessKeyName =
                connectionStringParts[1].Substring(connectionStringParts[1].IndexOf("=", StringComparison.Ordinal) + 1);
            var sharedAccessKey =
                connectionStringParts[2].Substring(connectionStringParts[2].IndexOf("=", StringComparison.Ordinal) + 1);

            var address = new EndpointAddress(BindingHelpers.CreateAddress(new Uri(endpoint), QueueName));
            var binding = BindingHelpers.ServiceBus.Binding(DefaultMaxMessageSize, DefaultTimeout, DebugTimeout);

            var channelFactory = new ChannelFactory<T>(binding, address);
            var endpointBehavior = new TransportClientEndpointBehavior
            {
                TokenProvider =
                    TokenProvider.CreateSharedAccessSignatureTokenProvider(sharedAccessKeyName, sharedAccessKey)
            };
            channelFactory.Endpoint.Behaviors.Add(endpointBehavior);
            return channelFactory;
        }

    }
}
