using System;

namespace PB.SpiralOfTest.Infrastructure.Service
{
    public interface IService : IDisposable
    {
        SessionContext Session { get; }
    }

}
