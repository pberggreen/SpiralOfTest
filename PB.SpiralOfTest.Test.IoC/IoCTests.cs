using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PB.SpiralOfTest.Access.Guest;

namespace PB.SpiralOfTest.Test.IoC
{
    [TestClass]
    public class IoCTests
    {
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void ResolveType_UnRegisteredType_ReturnsInterface()
        {
            Common.IoC.ResolveType<IGuestAccess>();
        }

        [TestMethod]
        public void ResolveType_RegisteredType_ReturnsInterface()
        {
            Common.IoC.RegisterType<IGuestAccess, GuestAccess>();
            var instance = Common.IoC.Resolve<IGuestAccess>();
            var registeredType = Common.IoC.ResolveType<IGuestAccess>();
            Assert.AreEqual(typeof(GuestAccess), registeredType);
            Assert.IsFalse(Common.IoC.IsInstanceRegistered<IGuestAccess>());
        }

        [TestMethod]
        public void ResolveType_RegisteredInstance_ReturnsTypeOfInstance()
        {
            var guestAccess = new GuestAccess();
            Common.IoC.RegisterInstance<IGuestAccess>(guestAccess);
            var instance = Common.IoC.Resolve<IGuestAccess>();
            var registeredType = Common.IoC.ResolveType<IGuestAccess>();
            Assert.AreEqual(typeof(IGuestAccess), registeredType);
            Assert.IsTrue(Common.IoC.IsInstanceRegistered<IGuestAccess>());
        }

        [TestMethod]
        public void ResolveType_RegisteredMoq_ReturnsMoqType()
        {
            var guestAccessMock = new Mock<IGuestAccess>();
            Common.IoC.RegisterInstance<IGuestAccess>(guestAccessMock.Object);
            var registeredType = Common.IoC.ResolveType<IGuestAccess>();
            Assert.AreEqual(typeof(IGuestAccess), registeredType);
            Assert.IsTrue(Common.IoC.IsInstanceRegistered<IGuestAccess>());
        }

        [TestMethod]
        public void Resolve_RegisteredMoq_ReturnsMoqType()
        {
            var guestAccessMock = new Mock<IGuestAccess>();
            Common.IoC.RegisterInstance<IGuestAccess>(guestAccessMock.Object);
            var instance = Common.IoC.Resolve<IGuestAccess>();
            Assert.AreEqual(typeof(IGuestAccess), instance);
        }

    }
}
