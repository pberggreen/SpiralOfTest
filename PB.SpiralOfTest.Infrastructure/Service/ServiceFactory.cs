using System;
using System.Reflection;
using ServiceModelEx;
using PB.SpiralOfTest.Infrastructure.ServiceLocator;

namespace PB.SpiralOfTest.Infrastructure.Service
{
    public interface IServiceFactory<T> where T : class
    {
        T CreateService();
    }

    public class PocoServiceFactory<T> : IServiceFactory<T> where T : class
    {
        public T CreateService()
        {
            return IoC.Resolve<T>();
        }
    }

    public class InProcServiceFactory<T> : IServiceFactory<T> where T : class
    {
        private readonly MethodInfo _genericInProcFactoryDef;

        public InProcServiceFactory()
        {
            _genericInProcFactoryDef = typeof(InProcFactory).GetMethod("CreateInstance", Type.EmptyTypes).GetGenericMethodDefinition();
        }
        public T CreateService()
       { 
            var poco = IoC.Resolve<T>();
            //if (IoC.IsInstanceRegistered<T>())
            //{
            //    return poco; 
            //}

            // Create InProc service
            var serviceType = poco.GetType();
            var createInstance = _genericInProcFactoryDef.MakeGenericMethod(serviceType, typeof(T));
            var inProc = (T)createInstance.Invoke(null, new object[0]);
            return inProc;
        }
    }

    public class ProxyFactory<T> : IServiceFactory<T> where T : class
    {
        public T CreateService()
        {
            var proxy = IoC.Resolve<T>();
            // TODO: Create proxy
            return proxy; 
        }
    }
}
