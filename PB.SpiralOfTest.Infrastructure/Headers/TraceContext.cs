using System;
using System.Runtime.Serialization;

namespace PB.SpiralOfTest.Infrastructure.Headers
{
    [DataContract]
    public class TraceContext
    {
        [DataMember]
        public Guid CallChainId { get; internal set; }

        [DataMember]
        public ulong SequenceNumber { get; internal set; }

        [DataMember]
        public DateTimeOffset ClientTimestamp { get; internal set; }

        public TraceContext()
        {
            CallChainId = Guid.NewGuid();
            SequenceNumber = 1;
            ClientTimestamp = DateTimeOffset.UtcNow;
        }
    }
}
