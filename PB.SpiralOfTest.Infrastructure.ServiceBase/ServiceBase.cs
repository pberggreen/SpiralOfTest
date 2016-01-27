using System;
using PB.SpiralOfTest.Common;

namespace PB.SpiralOfTest.Infrastructure.Service
{
    public abstract class ServiceBase
    {
        //public static IServiceFactory ServiceFactory { get; set; }

        //static ServiceBase()
        //{
        //    ServiceFactory = IoC.Resolve<IServiceFactory>();
        //}

        //private static IServiceFactory ServiceFactory => IoC.Resolve<IServiceFactory>();

        static ServiceBase()
        {
            //if (!IoC.IsRegistered<IServiceFactory>())
            //    IoC.RegisterType<IServiceFactory, InProcServiceFactory>();
        }

        //public SessionContext Session
        //{
        //    get
        //    {
        //        return Header<SessionContext>.GetIncoming(); 
        //    }
        //}

        public static T GetProxy<T>() where T : class
        {
            // Must create an InProc proxy 
            // unless there is a mock defined.
            // Perhaps we should also be able to use the poco.
            //return IoC.Resolve<T>();

            var serviceFactory = IoC.IsRegistered<IServiceFactory<T>>() 
                ? IoC.Resolve<IServiceFactory<T>>() 
                : new InProcServiceFactory<T>();

            return serviceFactory.CreateService();
        }

        //public void Dispose()
        //{
        //}
    }
}
