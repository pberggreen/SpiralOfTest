using Microsoft.ServiceBus.Messaging;
using System;
using System.Linq;
using System.ServiceModel.Description;
using Microsoft.ServiceBus;

namespace PB.SpiralOfTest.Infrastructure.Host
{
    public class ServiceBusServiceHost : CustomServiceHostBase
    {
        private static string _serviceBusConnectionString;  // TODO: Bør ikke være static. Se nedenstående hønen og ægget problem

        //TODO: Tilføje mulighed for at tilføje en "environment" tekst til kø-navnet ("Debug") 
        public ServiceBusServiceHost(Type serviceType, string serviceBusConnectionString) 
            : base(serviceType, GetBaseAddresses(serviceBusConnectionString))  // TODO: Hønen og ægget problem: Denne constructor kalder ultimativt AddEndpointBehavior, som har brug for serviceBusConnectionString, som stadig er null :-(
        {
            //_serviceBusConnectionString = serviceBusConnectionString;
        }

        private static Uri[] GetBaseAddresses(string serviceBusConnectionString)
        {
            _serviceBusConnectionString = serviceBusConnectionString;  // Hack
            var endpoint = GetEndpoint(serviceBusConnectionString);
            var uriBuilder = new UriBuilder(endpoint);
            return new Uri[] { uriBuilder.Uri };
        }

        //protected override IEnumerable<Type> GetContracts()
        //{
        //    return GetContracts(typeof(ServiceBusServiceBehaviorAttribute));   // Wrong: ServiceBusServiceBehaviorAttribute is applied to the server class - not the interface
        //}

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
                    var address = CreateAddress(baseAddress, endpointName);
                    var endpoint = AddServiceEndpoint(contractType, DefaultServiceBusBinding(), address);
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

        protected Uri CreateAddress(Uri baseAddress, string endpointName)
        {
            var uriBuilder = new UriBuilder(baseAddress);
            uriBuilder.Path += endpointName;
            return uriBuilder.Uri;
        }

        private NetMessagingBinding DefaultServiceBusBinding()
        {
            // TODO: This binding can be moved to a BindingHelper class to ensure that the server and client is using the same binding
            return new NetMessagingBinding
            {
                OpenTimeout = Timeout,
                CloseTimeout = Timeout,
                SendTimeout = Timeout,
                ReceiveTimeout = Timeout,
                TransportSettings = { BatchFlushInterval = TimeSpan.FromSeconds(1) }
            };
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
