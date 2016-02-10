using PB.SpiralOfTest.Infrastructure.Helpers;
using System;
using System.Linq;
using System.ServiceModel;

namespace PB.SpiralOfTest.Infrastructure.Host
{
    /// <summary>
    /// Service host for intranet (net.tcp) services
    /// </summary>
    public class IntranetServiceHost : CustomServiceHostBase
    {
        public IntranetServiceHost(Type serviceType, string hostName) 
            : base(serviceType, GetBaseAddresses(hostName))
        {
        }

        private static Uri[] GetBaseAddresses(string hostName)
        {
            return new Uri[] { BindingHelpers.Intranet.CreateAddress(hostName) };
        }

        protected override void ApplyEndpoints()
        {
            base.ApplyEndpoints();
            ApplyIntranetEndpoints();

            //this.SetSecurityBehavior(ServiceSecurity.Internet, "SignedByCA", true, "MyApplication");
        }

        protected void ApplyIntranetEndpoints()
        {
            if (!Description.Endpoints.Any(e => e.Binding is NetTcpBinding))
            {
                var baseAddress = BaseAddresses.First();
                
                foreach (var contractType in GetContracts())
                {
                    var endpointName = EnforceEndpointName(contractType);
                    var address = BindingHelpers.CreateAddress(baseAddress, endpointName);
                    AddServiceEndpoint(contractType, BindingHelpers.Intranet.Binding(MaxReceiveMessageSize, DefaultConnectivityTimeout, DefaultDebugTimeout), address);
                }
            }
        }

    }
}
