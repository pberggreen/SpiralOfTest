using System;
using System.Runtime.Serialization;

namespace PB.SpiralOfTest.Infrastructure.Headers
{
    [DataContract]
    public class SessionContext
    {
        [DataMember]
        public Guid SessionId { get; internal set; }
    }
}
