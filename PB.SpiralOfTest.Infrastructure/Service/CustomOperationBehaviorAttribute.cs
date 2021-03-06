﻿using System;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace PB.SpiralOfTest.Infrastructure.Service
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class CustomOperationBehaviorAttribute : Attribute, IOperationBehavior
    {
        protected Type OperationInvoker
        { get; set; }

        #region IOperationBehavior

        public void AddBindingParameters(OperationDescription operationDescription, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, System.ServiceModel.Dispatcher.ClientOperation clientOperation)
        {
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, System.ServiceModel.Dispatcher.DispatchOperation dispatchOperation)
        {
            InstallCustomInvoker(operationDescription, dispatchOperation);
        }

        public void Validate(OperationDescription operationDescription)
        {
        }

        #endregion IOperationBehavior

        internal virtual void InstallCustomInvoker(OperationDescription operationDescription, System.ServiceModel.Dispatcher.DispatchOperation dispatchOperation)
        {
            if (OperationInvoker != null)
            {
                dispatchOperation.Invoker = (IOperationInvoker)Activator.CreateInstance(OperationInvoker, dispatchOperation.Invoker);
            }
        }

    }
}
