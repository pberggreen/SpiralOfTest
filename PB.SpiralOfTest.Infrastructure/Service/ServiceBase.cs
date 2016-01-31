using PB.SpiralOfTest.Infrastructure.Headers;
using PB.SpiralOfTest.Infrastructure.Proxy;
using PB.SpiralOfTest.Infrastructure.ServiceLocator;

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

        //TODO: Check if this is works
        public SessionContext Session
        {
            get
            {
                return Header<SessionContext>.GetIncoming();
            }
        }

        public TraceContext Trace
        {
            get
            {
                return Header<TraceContext>.GetIncoming();
            }
        }

        public static T GetProxy<T>() where T : class
        {
            // Must create an InProc proxy 
            // unless there is a mock defined.
            // Perhaps we should also be able to use the poco.
            //return IoC.Resolve<T>();

            //var serviceFactory = IoC.IsRegistered<IServiceFactory<T>>() 
            //    ? IoC.Resolve<IServiceFactory<T>>() 
            //    : new InProcServiceFactory<T>();

            //return serviceFactory.CreateService();

            var proxy = IoC.Resolve<T>();
            if (proxy is ServiceBase)
            {
                // Call InProc
                // TODO: No need to use the factory
                return new InProcServiceFactory<T>().CreateService();
            }
            else
            {
                // Call POCO or mock
                return proxy;
            }
        }

        //public void Dispose()
        //{
        //}
    }
}
