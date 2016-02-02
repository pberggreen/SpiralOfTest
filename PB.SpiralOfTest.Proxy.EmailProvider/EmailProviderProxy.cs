using PB.SpiralOfTest.Contract.EmailProvider;
using PB.SpiralOfTest.Infrastructure.Proxy;

namespace PB.SpiralOfTest.Proxy.EmailProvider
{
    public class EmailProviderProxy : ServiceBusProxy<IEmailProvider>, IEmailProvider
    {
        public EmailProviderProxy(string serviceBusConnectionString) 
            : base(serviceBusConnectionString)
        {
        }

        public void SendEmail(string receiverEmail, string subject, string body)
        {
            Call(channel =>
            {
                channel.SendEmail(receiverEmail, subject, body);
            });
        }
    }
}
