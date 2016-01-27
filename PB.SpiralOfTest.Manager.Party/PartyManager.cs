using System;
using System.ServiceModel;
using PB.SpiralOfTest.Access.Guest;
using PB.SpiralOfTest.Common;
using PB.SpiralOfTest.Engine.EmailSender;
using PB.SpiralOfTest.Infrastructure.Service;

namespace PB.SpiralOfTest.Manager.Party
{
    [ServiceContract]
    public interface IPartyManager
    {
        [OperationContract]
        void SendInvitations(string templateName, Guid partyId);
    }

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
