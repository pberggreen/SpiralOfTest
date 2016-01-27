using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace PB.SpiralOfTest.Infrastructure.Host
{
    public abstract class CustomServiceHostBase : ServiceHost
    {
        public CustomServiceHostBase(Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        { }

        //protected virtual void Host_Closed(object sender, EventArgs a);

        //protected string ServiceName { get; }

        protected virtual long MaxReceiveMessageSize => 65536;
        protected virtual TimeSpan DefaultDebugTimeout => TimeSpan.FromHours(1);
        protected virtual TimeSpan DefaultConnectivityTimeout => TimeSpan.FromMinutes(1);

        //protected void LoadConfiguration();

        protected override void ApplyConfiguration()
        {
            ApplyServiceConfiguration();
            ApplyEndpoints();
        }

        protected virtual void ApplyServiceConfiguration()
        { }

        protected virtual void ApplyEndpoints()
        {
            // TODO: Maybe always add MEX endpoint
        }

        //protected virtual void ApplyConfigOverriddenEndpoints();

        protected static string EnforceEndpointName(Type interfaceType)
        {
            return interfaceType.FullName.Replace("Contract", "Service");
        }

        protected EndpointAddress EnforceStandardAddress(Type contractType, EndpointAddress endpointAddress)
        {
            return endpointAddress;
        }

        //protected Type GetContract(string name, bool validate = true);

        /// <summary>
        /// Find all contracts of the specific type (eg. intranet)
        /// </summary>
        protected abstract IEnumerable<Type> GetContracts();

    }
}
