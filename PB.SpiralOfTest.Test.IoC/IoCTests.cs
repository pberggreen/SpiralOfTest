using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PB.SpiralOfTest.Access.Guest;
using PB.SpiralOfTest.Infrastructure.ServiceLocator;
using System;

namespace PB.SpiralOfTest.Test.IoCTests
{
    [TestClass]
    public class IoCTests
    {
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ResolveType_UnRegisteredType_ReturnsInterface()
        {
            IoC.ResolveType<IGuestAccess>();
        }

        [TestMethod]
        public void ResolveType_RegisteredType_ReturnsInterface()
        {
            IoC.RegisterType<IGuestAccess, GuestAccess>();
            var instance = IoC.Resolve<IGuestAccess>();
            var registeredType = IoC.ResolveType<IGuestAccess>();
            Assert.AreEqual(typeof(GuestAccess), registeredType);
            Assert.IsFalse(IoC.IsInstanceRegistered<IGuestAccess>());
        }

        [TestMethod]
        public void ResolveType_RegisteredInstance_ReturnsTypeOfInstance()
        {
            var guestAccess = new GuestAccess();
            IoC.RegisterInstance<IGuestAccess>(guestAccess);
            var instance = IoC.Resolve<IGuestAccess>();
            var registeredType = IoC.ResolveType<IGuestAccess>();
            Assert.AreEqual(typeof(IGuestAccess), registeredType);
            Assert.IsTrue(IoC.IsInstanceRegistered<IGuestAccess>());
        }

        [TestMethod]
        public void ResolveType_RegisteredMoq_ReturnsMoqType()
        {
            var guestAccessMock = new Mock<IGuestAccess>();
            IoC.RegisterInstance<IGuestAccess>(guestAccessMock.Object);
            var registeredType = IoC.ResolveType<IGuestAccess>();
            Assert.AreEqual(typeof(IGuestAccess), registeredType);
            Assert.IsTrue(IoC.IsInstanceRegistered<IGuestAccess>());
        }

        [TestMethod]
        public void Resolve_RegisteredMoq_ReturnsMoqType()
        {
            var guestAccessMock = new Mock<IGuestAccess>();
            IoC.RegisterInstance<IGuestAccess>(guestAccessMock.Object);
            var instance = IoC.Resolve<IGuestAccess>();
            Assert.AreEqual(typeof(IGuestAccess), instance);
        }

    }
}
