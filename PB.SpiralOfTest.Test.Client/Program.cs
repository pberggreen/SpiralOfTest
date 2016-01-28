using PB.SpiralOfTest.Proxy.Party;
using System;
using PB.SpiralOfTest.Contract.EmailProvider;
using PB.SpiralOfTest.Proxy.EmailProvider;

namespace PB.SpiralOfTest.Test.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press I to send invitations");
            Console.WriteLine("Press E to send email directly");
            Console.WriteLine("Press X to exit");
            while (true)
            {
                var input = Console.ReadLine();
                if (input == "I")
                {
                    Console.WriteLine("Sending invitations");
                    var partyManager = new PartyProxy();
                    var partyId = Guid.NewGuid();
                    partyManager.SendInvitations("Birthday", partyId);
                    Console.WriteLine("Invitations sent");
                }
                if (input == "E")
                {
                    Console.WriteLine("Sending email");
                    var serviceBusConnectionString =
                        "Endpoint=sb://sv16test2.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ierC00HcBBmyHE22NIBZw9t2otXsRD9/QW0K55Iknvg=";
                    var queueName = typeof(IEmailProvider).Name;
                    var emailProviderProxy = new EmailProviderProxy(serviceBusConnectionString, queueName);
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
