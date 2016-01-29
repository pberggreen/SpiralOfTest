using PB.SpiralOfTest.Infrastructure.Service;
using System;
using System.ServiceModel;

namespace PB.SpiralOfTest.Contract.Party
{
    [ServiceContract]
    public interface IPartyManager
    {
        [OperationContract]
        void SendInvitations(string templateName, Guid partyId);
    }

}
