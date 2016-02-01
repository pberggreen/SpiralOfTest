using System;

namespace PB.SpiralOfTest.Infrastructure.Proxy
{
    /// <summary>
    /// Interface for wcf proxy classes.
    /// A proxy implementing this interface do not have to make "call-through" implementations of all methods in the service contract.
    /// </summary>
    public interface IProxy<out TServiceContract> : IDisposable where TServiceContract : class
    {
        void Call(Action<TServiceContract> action);
    }
}
