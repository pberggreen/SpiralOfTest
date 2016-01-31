using System;
using PB.SpiralOfTest.Contract.Party;
using PB.SpiralOfTest.Infrastructure.Proxy;
using System.ServiceModel;

namespace PB.SpiralOfTest.Proxy.Party
{
    public class PartyProxy: IntranetProxy<IPartyManager>
    {
        /*
        private IPartyManager Channel { get; set; }

        public PartyProxy()
        {
            // TODO: Find out how to use the ChannelFactory correctly and when to create (and destroy) the channel 
            var channelFactory = CreateFactory(Timeout, DefaultMaxMessageSize);
            Channel = ChannelFactory.CreateChannel();
        }

        public void SendInvitations(string templateName, Guid partyId)
        {
            Call(channel => {
                channel.SendInvitations(templateName, partyId);
            });
        }
        */
    }
}
