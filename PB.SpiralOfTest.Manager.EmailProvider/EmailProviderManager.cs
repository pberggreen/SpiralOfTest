using System.Diagnostics;
using PB.SpiralOfTest.Contract.EmailProvider;
using PB.SpiralOfTest.Infrastructure.Service;

namespace PB.SpiralOfTest.Manager.EmailProvider
{
    [ServiceBusServiceBehavior]
    public class EmailProviderManager : IEmailProvider
    {
        [ServiceBusOperationBehavior]
        public void SendEmail(string receiverEmail, string subject, string body)
        {
            Debug.WriteLine("EmailProviderManager.SendEmail");
        }
    }
}
