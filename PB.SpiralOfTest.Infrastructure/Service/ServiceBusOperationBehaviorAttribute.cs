using System;
using System.ServiceModel.Description;

namespace PB.SpiralOfTest.Infrastructure.Service
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ServiceBusOperationBehaviorAttribute : CustomOperationBehaviorAttribute, IOperationBehavior
    {
        internal override void InstallCustomInvoker(OperationDescription operationDescription, System.ServiceModel.Dispatcher.DispatchOperation dispatchOperation)
        {
        }

    }
}
