using System;

namespace PB.SpiralOfTest.Infrastructure.Headers
{
    public interface IService : IDisposable
    {
        SessionContext Session { get; }

        TraceContext Trace { get; }
    }

}
