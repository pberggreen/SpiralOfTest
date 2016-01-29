using System;
using System.Runtime.Serialization;

namespace PB.SpiralOfTest.Infrastructure.Service
{
    [DataContract]
    public class SessionContext
    {
        [DataMember]
        public Guid SessionId { get; set; }
    }

}
