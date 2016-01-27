using System;
using System.ServiceModel;
using PB.SpiralOfTest.Manager.Party;
using ServiceModelEx;
using PB.SpiralOfTest.Infrastructure.Host;

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

            IntranetServiceHost<PartyManager> partyHost = null;
            try
            {
                partyHost = new IntranetServiceHost<PartyManager>();
                partyHost.Open();
                System.Console.WriteLine("{0}.Main():  Party ServiceHost opened OK.", m_ThisName);

                // more services
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("{0}.Main():  host.Open() Threw exception!\n     {1}", m_ThisName, ex);
            }
            System.Console.WriteLine("\n{0}.Main():  Press ENTER to EXIT.", m_ThisName);
            System.Console.ReadLine();
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
