using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace PB.SpiralOfTest.Infrastructure.Host
{
    // ServiceType is the manager type - it can implement several service contracts
    public class IntranetServiceHost : CustomServiceHostBase
    {
        public IntranetServiceHost(Type serviceType, string hostName) 
            : base(serviceType, GetBaseAddresses(hostName))
        {
        }

        private static Uri[] GetBaseAddresses(string hostName)
        {
            // Remove schema if present
            var index = hostName.IndexOf("//");
            if (index != -1)
            {
                hostName = hostName.Substring(index + 2);
            }
            hostName = "net.tcp://" + hostName;
            var uriBuilder = new UriBuilder(hostName);
            return new Uri[] { uriBuilder.Uri };
        }

        protected override void ApplyEndpoints()
        {
            base.ApplyEndpoints();
            ApplyIntranetEndpoints();
        }

        protected void ApplyIntranetEndpoints()
        {
            if (!Description.Endpoints.Any(e => e.Binding is NetTcpBinding))
            {
                var baseAddress = BaseAddresses.First();
                
                foreach (var contractType in GetContracts())
                {
                    var endpointName = EnforceEndpointName(contractType);
                    var address = CreateAddress(baseAddress, endpointName);
                    AddServiceEndpoint(contractType, DefaultIntranetBinding(), address);
                }
            }
        }

        protected override IEnumerable<Type> GetContracts()
        {
            // TODO: Find all contracts implemented by the manager of type T - Contracts with "Intranet" service attribute
            return Description.ServiceType.GetInterfaces(); 
        }

        // TODO: Move this logic to a helper class - so that it can be shared with the proxy base class
        protected Uri CreateAddress(Uri baseAddress, string endpointName)
        {
            var uriBuilder = new UriBuilder(baseAddress);
            uriBuilder.Path += endpointName;
            return uriBuilder.Uri;
        }

        private NetTcpBinding DefaultIntranetBinding()
        {
            // TODO: This binding can be moved to a BindingHelper class to ensure that the server and client is using the same binding
            return new NetTcpBinding
            {
                TransactionFlow = true,
                ReliableSession = {Enabled = true}
            };
        }

        #region Intranet BindingHelper stuff 

        static NetTcpBinding _intranetBinding;
        static NetTcpBinding DefaultBinding()
        {
            return new NetTcpBinding(SecurityMode.Transport, true);
        }

        public static NetTcpBinding Binding(long maxReceivedMessageSize, TimeSpan timeout, TimeSpan debugTimeout)
        {
            if (_intranetBinding == null)
            {
                try
                {
                    // Try to create binding from the config file
                    _intranetBinding = new NetTcpBinding("Intranet");
                }
                catch
                {
                    _intranetBinding = DefaultBinding();
                }
            }

            EnforceBindingPolicies(_intranetBinding, maxReceivedMessageSize, timeout, debugTimeout);

            return _intranetBinding;
        }

        private static void EnforceBindingPolicies(NetTcpBinding binding, long maxReceivedMessageSize, TimeSpan timeout, TimeSpan debugTimeout)
        {
            binding.MaxReceivedMessageSize = maxReceivedMessageSize;
#if DEBUG
            binding.ReceiveTimeout = debugTimeout;
#else
            binding.ReceiveTimeout = timeout;
#endif
        }

        #endregion Intranet BindingHelper stuff 

    }
}
