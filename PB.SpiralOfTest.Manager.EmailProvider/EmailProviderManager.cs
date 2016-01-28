using System.Diagnostics;
using PB.SpiralOfTest.Contract.EmailProvider;

namespace PB.SpiralOfTest.Manager.EmailProvider
{
    public class EmailProviderManager : IEmailProvider
    {
        public void SendEmail(string receiverEmail, string subject, string body)
        {
            Debug.WriteLine("EmailProviderManager.SendEmail");
        }
    }
}
