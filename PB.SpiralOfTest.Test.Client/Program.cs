using PB.SpiralOfTest.Proxy.Party;
using System;

namespace PB.SpiralOfTest.Test.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press Enter to send invitations");
            Console.ReadLine();
            Console.WriteLine("Sending invitations");
            var partyManager = new PartyProxy();
            var partyId = Guid.NewGuid();
            partyManager.SendInvitations("Birthday", partyId);
            Console.ReadLine();
        }
    }
}
