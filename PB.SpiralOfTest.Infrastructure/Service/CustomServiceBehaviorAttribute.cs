using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace PB.SpiralOfTest.Infrastructure.Service
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class CustomServiceBehaviorAttribute : Attribute, IServiceBehavior
    {
        internal Type OperationBehavior;

        public int MaxMessageSize { get; set; }

        public string Timeout { get; set; }

        #region IServiceBehavior

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            AddOperationAttributes(serviceDescription);
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            EnforceServiceBehavior(serviceDescription, serviceHostBase);
        }

        #endregion IServiceBehavior

        internal virtual void AddOperationAttributes(ServiceDescription serviceDescription)
        {
        }

        private void EnforceServiceBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            ServiceBehaviorAttribute serviceBehavior = (ServiceBehaviorAttribute)Attribute.GetCustomAttribute(typeof(CustomServiceBehaviorAttribute), typeof(CustomOperationBehaviorAttribute));
            if (serviceBehavior != null)
            {
                serviceBehavior.InstanceContextMode = InstanceContextMode.PerCall;
                serviceBehavior.ConcurrencyMode = ConcurrencyMode.Multiple;
                serviceBehavior.UseSynchronizationContext = false;
                serviceBehavior.IncludeExceptionDetailInFaults = true;
            }
        }
    }
}
