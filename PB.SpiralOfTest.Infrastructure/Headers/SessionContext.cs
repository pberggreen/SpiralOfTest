using System;
using System.Runtime.Serialization;

namespace PB.SpiralOfTest.Infrastructure.Headers
{
    [DataContract]
    public class SessionContext
    {
        [DataMember]
        Guid SessionId { get; set; }
    }
}
