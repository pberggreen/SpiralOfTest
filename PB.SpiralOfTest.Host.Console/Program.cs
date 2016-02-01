using System;
using System.ServiceModel;
using PB.SpiralOfTest.Manager.Party;
using System.Configuration;
using PB.SpiralOfTest.Infrastructure.Host;
using PB.SpiralOfTest.Manager.EmailProvider;

namespace PB.SpiralOfTest.Host.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            string m_ThisName = "ConsoleServiceHost";
            System.Console.WriteLine();
            System.Console.Title = m_ThisName;
            System.Console.WriteLine("Service Host for the PartyManager");

            ServiceBusServiceHost serviceBusHost = null;
            var serviceBusConnectionString = "Endpoint=sb://sv16test2.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ierC00HcBBmyHE22NIBZw9t2otXsRD9/QW0K55Iknvg=";
            try
            {
                serviceBusHost = new ServiceBusServiceHost(typeof(EmailProviderManager), serviceBusConnectionString, "debug");
                serviceBusHost.Open();
                AppDomain.CurrentDomain.ProcessExit += serviceBusHost.Host_Closed;
                System.Console.WriteLine("{0}.Main():  ServiceBusHost opened OK.", m_ThisName);

                // more services
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("{0}.Main():  host.Open() Threw exception!\n     {1}", m_ThisName, ex);
            }

            IntranetServiceHost partyHost = null;
            var hostName = ConfigurationManager.AppSettings["HostName"];
            try
            {
                partyHost = new IntranetServiceHost(typeof(PartyManager), hostName);
                partyHost.Open();
                AppDomain.CurrentDomain.ProcessExit += partyHost.Host_Closed;
                System.Console.WriteLine("{0}.Main():  Party ServiceHost opened OK.", m_ThisName);

                // more services
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("{0}.Main():  host.Open() Threw exception!\n     {1}", m_ThisName, ex);
            }
            System.Console.WriteLine("\n{0}.Main():  Press ENTER to EXIT.", m_ThisName);
            System.Console.ReadLine();
            CloseOrAbortHost(serviceBusHost);
            CloseOrAbortHost(partyHost);
            System.Console.WriteLine("\n{0}.Main(): Exiting......", m_ThisName);

        }

        private static void CloseOrAbortHost(ICommunicationObject host)
        {
            if (host != null)
            {
                if (host.State != CommunicationState.Faulted)
                {
                    host.Close();
                }
                else
                {
                    host.Abort();
                }
            }
        }
    }
}
