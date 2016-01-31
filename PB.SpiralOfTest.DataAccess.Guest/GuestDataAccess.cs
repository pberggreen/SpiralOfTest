using PB.SpiralOfTest.Infrastructure.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;

namespace PB.SpiralOfTest.Access.Guest
{
    [ServiceContract]
    public interface IGuestAccess
    {
        [OperationContract]
        List<Guest> GetGuests(Guid partyId);
    }

    public class GuestAccess : ServiceBase, IGuestAccess
    {
        public List<Guest> GetGuests(Guid partyId)
        {
            Debug.WriteLine("GuestAccess.GetGuests({0})", partyId);
            return new List<Guest>();
        }
    }

    public class Guest
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
            
    }

}
