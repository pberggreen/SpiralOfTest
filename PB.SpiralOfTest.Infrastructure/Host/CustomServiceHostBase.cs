using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace PB.SpiralOfTest.Infrastructure.Host
{
    public abstract class CustomServiceHostBase : ServiceHost
    {
        protected CustomServiceHostBase(Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
        }

        public virtual void Host_Closed(object sender, EventArgs a)
        {
            Close();
        }

        //protected string ServiceName { get; }

        protected virtual long MaxReceiveMessageSize => 65536;
        protected virtual TimeSpan DefaultDebugTimeout => TimeSpan.FromHours(1);
        protected virtual TimeSpan DefaultConnectivityTimeout => TimeSpan.FromMinutes(1);

        protected TimeSpan Timeout
        {
            get
            {
#if DEBUG
                return DefaultDebugTimeout;
#else
                return DefaultConnectivityTimeout;
#endif
            }
        }
    
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

        protected virtual string EnforceEndpointName(Type interfaceType)
        {
            return interfaceType.FullName.Replace("Contract", "Service");
        }

        protected EndpointAddress EnforceStandardAddress(Type contractType, EndpointAddress endpointAddress)
        {
            return endpointAddress;
        }

        //protected Type GetContract(string name, bool validate = true);

        /// <summary>
        /// Find all contracts implemented by the service
        /// </summary>
        protected virtual IEnumerable<Type> GetContracts()
        {
            return GetContracts(typeof (ServiceContractAttribute));
        }

        /// <summary>
        /// Find all service contracts implemented by the service with a specific service attribute 
        /// </summary>
        protected virtual IEnumerable<Type> GetContracts(Type serviceContractAttributeType)
        {
            var interfaces = Description.ServiceType.GetInterfaces();

            var contracts = new List<Type>();
            foreach (Type type in interfaces)
            {
                if (type.GetCustomAttributes(serviceContractAttributeType, false).Any())
                {
                    contracts.Add(type);
                }
            }
            return contracts.ToArray();
        }

    }
}
