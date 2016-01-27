using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;

namespace PB.SpiralOfTest.Infrastructure.Host
{
    // T is the manager type - it can implement several service contracts
    public class IntranetServiceHost<T> : CustomServiceHostBase
    {
        public IntranetServiceHost() 
            : base(typeof(T), GetBaseAddresses())
        {
        }

        private static Uri[] GetBaseAddresses()
        {
            var baseAddress = ConfigurationManager.AppSettings["BaseAddress"];

            // Remove schema if present
            var index = baseAddress.IndexOf("//");
            if (index != -1)
            {
                baseAddress = baseAddress.Substring(index + 2);
            }
            baseAddress = "net.tcp://" + baseAddress;
            var uriBuilder = new UriBuilder(baseAddress);
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
                //var baseAddress = ConfigurationManager.AppSettings["BaseAddress"];
                //var endpointAddress = new EndpointAddress(baseAddress);
                var baseAddress = BaseAddresses.First().AbsoluteUri;
                
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
            // TODO: Find all contracts implemented by the manager of type T
            return typeof(T).GetInterfaces(); 
        }

        // TODO: Move this logic to a helper class - so that it can be shared with the proxy base class
        protected Uri CreateAddress(string baseAddress, string endpointName)
        {
            // Remove schema if present
            var index = baseAddress.IndexOf("//");
            if (index != -1)
            {
                baseAddress = baseAddress.Substring(index + 2);
            }
            baseAddress = "net.tcp://" + baseAddress;
            var uriBuilder = new UriBuilder(baseAddress);
            uriBuilder.Path += endpointName;
            return uriBuilder.Uri;
        }

        private NetTcpBinding DefaultIntranetBinding()
        {
            // TODO: This binding can be moved to a BindingHelper class to ensure that the server and client is using the same binding
            var binding = new NetTcpBinding();
            binding.TransactionFlow = true;
            binding.ReliableSession.Enabled = true;
            return binding;
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
