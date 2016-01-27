using System;
using System.ServiceModel;
using PB.SpiralOfTest.Manager.Party;

namespace PB.SpiralOfTest.Proxy.Party
{
    public class PartyProxy: ClientBase<IPartyManager>, IPartyManager
    {
        public PartyProxy(string endpointName) : base(endpointName)
        {
        }

        public void SendInvitations(string templateName, Guid partyId)
        {
            try
            {
                Channel.SendInvitations(templateName, partyId);
            }
            catch (Exception)
            {
                Abort();
                throw;
            }
        }
    }
}
