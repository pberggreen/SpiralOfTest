using Microsoft.ServiceBus.Messaging;
using System;
using System.Linq;
using System.ServiceModel.Description;
using Microsoft.ServiceBus;
using PB.SpiralOfTest.Infrastructure.Helpers;

namespace PB.SpiralOfTest.Infrastructure.Host
{
    public class ServiceBusServiceHost : CustomServiceHostBase
    {
        // TODO: Bør ikke være static. Se nedenstående hønen og ægget problem
        private static string _serviceBusConnectionString;  
        private static string _endpointSuffix;

        public ServiceBusServiceHost(Type serviceType, string serviceBusConnectionString, string endpointSuffix = null) 
            : base(serviceType, GetBaseAddresses(serviceBusConnectionString, endpointSuffix))  // TODO: Hønen og ægget problem: Denne constructor kalder ultimativt AddEndpointBehavior, som har brug for serviceBusConnectionString, som stadig er null :-(
        {
            //_serviceBusConnectionString = serviceBusConnectionString;
        }

        private static Uri[] GetBaseAddresses(string serviceBusConnectionString, string endpointSuffix)
        {
            _serviceBusConnectionString = serviceBusConnectionString;  // Hack
            _endpointSuffix = endpointSuffix;
            var endpoint = GetEndpoint(serviceBusConnectionString);
            var uriBuilder = new UriBuilder(endpoint);
            return new Uri[] { uriBuilder.Uri };
        }

        protected override void ApplyEndpoints()
        {
            base.ApplyEndpoints();
            ApplyServiceBusEndpoints();
        }

        protected override string EnforceEndpointName(Type interfaceType)
        {
            return interfaceType.Name;
        }

        protected void ApplyServiceBusEndpoints()
        {
            if (!Description.Endpoints.Any(e => e.Binding is NetMessagingBinding))
            {
                var baseAddress = BaseAddresses.First();

                foreach (var contractType in GetContracts())
                {
                    var endpointName = EnforceEndpointName(contractType);
                    if (!string.IsNullOrEmpty(_endpointSuffix))
                        endpointName += "-" + _endpointSuffix;
                    var address = BindingHelpers.CreateAddress(baseAddress, endpointName);
                    var binding = BindingHelpers.ServiceBus.Binding(MaxReceiveMessageSize, DefaultConnectivityTimeout,
                        DefaultDebugTimeout);
                    var endpoint = AddServiceEndpoint(contractType, binding, address);
                    AddEndpointBehavior(endpoint);
                }
            }
        }

        private void AddEndpointBehavior(ServiceEndpoint endpoint)
        {
            var accessKeyName = GetAccessKeyName(_serviceBusConnectionString);
            var accessKey = GetAccessKey(_serviceBusConnectionString);
            var endpointBehaviour = new TransportClientEndpointBehavior
            {
                TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(accessKeyName, accessKey)
            };
            endpoint.EndpointBehaviors.Add(endpointBehaviour);
        }

        private static string GetEndpoint(string serviceBusConnectionString)
        {
            var connectionStringParts = serviceBusConnectionString.Split(';');
            var endpoint = connectionStringParts[0].Substring(connectionStringParts[0].IndexOf("=", StringComparison.Ordinal) + 1);
            return endpoint;
        }

        private static string GetAccessKeyName(string serviceBusConnectionString)
        {
            var connectionStringParts = serviceBusConnectionString.Split(';');
            return connectionStringParts[1].Substring(connectionStringParts[1].IndexOf("=", StringComparison.Ordinal) + 1);
        }

        private static string GetAccessKey(string serviceBusConnectionString)
        {
            var connectionStringParts = serviceBusConnectionString.Split(';');
            return connectionStringParts[2].Substring(connectionStringParts[2].IndexOf("=", StringComparison.Ordinal) + 1);
        }

    }
}
