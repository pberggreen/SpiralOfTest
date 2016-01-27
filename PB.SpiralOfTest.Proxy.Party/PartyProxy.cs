using System;
using PB.SpiralOfTest.Contract.Party;
using PB.SpiralOfTest.Infrastructure.Proxy;

namespace PB.SpiralOfTest.Proxy.Party
{
    public class PartyProxy: IntranetProxy<IPartyManager>, IPartyManager
    {
        public void SendInvitations(string templateName, Guid partyId)
        {
            Call(channel => {
                channel.SendInvitations(templateName, partyId);
            });
        }

    }
}
