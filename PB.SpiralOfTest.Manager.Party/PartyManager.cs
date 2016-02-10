using System;
using PB.SpiralOfTest.Access.Guest;
using PB.SpiralOfTest.Engine.EmailSender;
using PB.SpiralOfTest.Infrastructure.Service;
using PB.SpiralOfTest.Contract.Party;
using System.Diagnostics;
using PB.SpiralOfTest.Infrastructure.ServiceLocator;

namespace PB.SpiralOfTest.Manager.Party
{
    [IntranetServiceBehavior(MaxMessageSize = 50000000, Timeout = "00:10:00")]
    public class PartyManager : ServiceBase, IPartyManager
    {
        static PartyManager()
        {
            // Default services
            if (!IoC.IsRegistered<IGuestAccess>())
                IoC.RegisterType<IGuestAccess, GuestAccess>();
            if (!IoC.IsRegistered<IEmailSenderEngine>())
                IoC.RegisterType<IEmailSenderEngine, EmailSenderEngine>();    
        }

        public void SendInvitations(string templateName, Guid partyId)
        {
            Debug.WriteLine("PartyManager.SendInvitations");
            //var seqNumber = Trace.SequenceNumber;
            //var sessionId = Session.SessionId;

            var guestAccess = GetProxy<IGuestAccess>();
            var guests = guestAccess.GetGuests(partyId);

            var emailEngine = GetProxy<IEmailSenderEngine>();
            foreach (var guest in guests)
            {
                emailEngine.SendEmail(templateName, guest.Email);
            }
        }
    }
}
