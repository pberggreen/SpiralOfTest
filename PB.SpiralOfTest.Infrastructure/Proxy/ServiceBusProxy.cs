﻿using System;
using System.ServiceModel;
using Microsoft.ServiceBus;
using PB.SpiralOfTest.Infrastructure.Helpers;

namespace PB.SpiralOfTest.Infrastructure.Proxy
{
    /// <summary>
    /// Base class for proxies calling services via the Azure service bus
    /// </summary>
    public abstract class ServiceBusProxy<TServiceContract> : CustomProxyBase<TServiceContract> where TServiceContract : class
    {
        protected string _serviceBusConnectionString;

        protected ServiceBusProxy(string serviceBusConnectionString)
        {
            _serviceBusConnectionString = serviceBusConnectionString;
        }

        protected override string EnforceEndpointName
        {
            get
            {
                var endpointName = typeof(TServiceContract).Name;
#if DEBUG
                endpointName += "-debug";
#endif
                return endpointName;
            }
        }

        protected override ChannelFactory<TServiceContract> CreateFactory(TimeSpan timeout, long maxMessageSize)
        {
            var connectionStringParts = _serviceBusConnectionString.Split(';');
            var endpoint =
                connectionStringParts[0].Substring(connectionStringParts[0].IndexOf("=", StringComparison.Ordinal) + 1);
            var sharedAccessKeyName =
                connectionStringParts[1].Substring(connectionStringParts[1].IndexOf("=", StringComparison.Ordinal) + 1);
            var sharedAccessKey =
                connectionStringParts[2].Substring(connectionStringParts[2].IndexOf("=", StringComparison.Ordinal) + 1);

            var endpointName = EnforceEndpointName;
            var address = new EndpointAddress(BindingHelpers.CreateAddress(new Uri(endpoint), endpointName));
            var binding = BindingHelpers.ServiceBus.Binding(DefaultMaxMessageSize, DefaultTimeout);

            var channelFactory = new ChannelFactory<TServiceContract>(binding, address);
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
