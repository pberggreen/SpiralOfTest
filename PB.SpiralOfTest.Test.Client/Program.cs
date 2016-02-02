using PB.SpiralOfTest.Proxy.Party;
using System;
using PB.SpiralOfTest.Contract.EmailProvider;
using PB.SpiralOfTest.Proxy.EmailProvider;
using PB.SpiralOfTest.Infrastructure.ServiceLocator;
using PB.SpiralOfTest.Infrastructure.Proxy;
using PB.SpiralOfTest.Contract.Party;

namespace PB.SpiralOfTest.Test.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            IoC.RegisterType<IProxy<IPartyManager>, PartyProxy>();   // Do this via an attribute

            Console.WriteLine("Press I to send invitations");
            Console.WriteLine("Press E to send email directly");
            Console.WriteLine("Press X to exit");
            while (true)
            {
                var input = Console.ReadLine().ToUpper();
                if (input == "I")
                {
                    Console.WriteLine("Sending invitations");
                    //using (var partyManager = new PartyProxy())
                    using (var partyManager = IoC.Resolve<IProxy<IPartyManager>>())
                    {
                        var partyId = Guid.NewGuid();
                        partyManager.Call(proxy =>
                        {
                            proxy.SendInvitations("Birthday", partyId);
                        });
                    }
                    Console.WriteLine("Invitations sent");
                }
                if (input == "E")
                {
                    Console.WriteLine("Sending email");
                    var serviceBusConnectionString =
                        "Endpoint=sb://sv16test2.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ierC00HcBBmyHE22NIBZw9t2otXsRD9/QW0K55Iknvg=";
                    var emailProviderProxy = new EmailProviderProxy(serviceBusConnectionString);
                    emailProviderProxy.SendEmail("peter@sv16.dk", "Test", "This is a test");
                    Console.WriteLine("Email sent");
                }
                if (input == "X")
                {
                    return;
                }
            }
        }
    }
}
