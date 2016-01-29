using Microsoft.VisualStudio.TestTools.UnitTesting;
using PB.SpiralOfTest.Access.EmailProvider;
using PB.SpiralOfTest.Access.EmailTemplate;
using PB.SpiralOfTest.Infrastructure.ServiceLocator;
using PB.SpiralOfTest.Manager.Party;
using PB.SpiralOfTest.Contract.Party;

namespace PB.SpiralOfTest.Test
{
    /// <summary>
    /// Summary description for PartyManagerTests
    /// </summary>
    [TestClass]
    public class PartyManagerTests
    {
        public PartyManagerTests()
        {
            IoC.RegisterType<IPartyManager, PartyManager>();
            //IoC.RegisterType<IEmailSenderEngine, EmailSenderEngine>();
            //IoC.RegisterType<IGuestAccess, GuestAccess>();
            IoC.RegisterType<IEmailProviderAccess, EmailProviderAccess>();
            IoC.RegisterType<IEmailTemplateAccess, EmailTemplateAccess>();
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        #region Additional test attributes

        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //

        #endregion

        //[TestMethod]
        //public void SendInvitations()
        //{
        //    var partyId = Guid.NewGuid();
        //    var templateName = "Birthday";
        //    var guest1Name = "Guest1";
        //    var guest1Email = "guest1@trackman.test";

        //    var guests = new List<Guest>
        //    {
        //        new Guest()
        //        {
        //            Id = Guid.NewGuid(),
        //            Name = guest1Name,
        //            Email = guest1Email
        //        }
        //    };

        //    var emailTemplate = new EmailTemplate
        //    {
        //        Name = templateName,
        //        Subject = "Invitation",
        //        Body = "You're invited to my birthday party"
        //    };

        //    var guestAccessMock = new Mock<IGuestAccess>();
        //    guestAccessMock.Setup(m => m.GetGuests(partyId)).Returns(guests);
        //    //IoC.RegisterInstance<IGuestAccess>(guestAccessMock.Object);
        //    var guestAccessServiceFactoryMock = new Mock<IServiceFactory<IGuestAccess>>();
        //    guestAccessServiceFactoryMock.Setup(m => m.CreateService()).Returns(guestAccessMock.Object);
        //    IoC.RegisterInstance<IServiceFactory<IGuestAccess>>(guestAccessServiceFactoryMock.Object);

        //    //var emailTemplateAccessMock = new Mock<IEmailTemplateAccess>();
        //    //emailTemplateAccessMock.Setup(m => m.GetEmailTemplate(templateName)).Returns(emailTemplate);
        //    //IoC.RegisterInstance<IEmailTemplateAccess>(emailTemplateAccessMock.Object);

        //    //var emailEngineMock = new Mock<IEmailSenderEngine>();
        //    //emailEngineMock.Setup(m => m.SendEmail(templateName, guest1Email));
        //    //IoC.RegisterInstance<IEmailSenderEngine>(emailEngineMock.Object);

        //    //IoC.RegisterType<IServiceFactory<IEmailTemplateAccess>, PocoServiceFactory<IEmailTemplateAccess>>();
        //    IoC.RegisterType<IServiceFactory<IEmailSenderEngine>, InProcServiceFactory<IEmailSenderEngine>>();
        //    var partyManager = ServiceBase.GetProxy<IPartyManager>();
        //    FeatureContext.Current = new GoldContext();
        //    partyManager.SendInvitations(templateName, partyId);
        //}

        [TestMethod]
        public void TextContext()
        {
            Assert.IsTrue(FeatureContext.Current is BasicContext);
        }
    }
}

