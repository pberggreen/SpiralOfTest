using System;

namespace PB.SpiralOfTest.Infrastructure.Proxy
{
    public interface IProxy<T> : IDisposable where T : class
    {
        void Call(Action<T> action);
    }
}
