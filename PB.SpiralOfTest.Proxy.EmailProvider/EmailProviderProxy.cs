using PB.SpiralOfTest.Contract.EmailProvider;
using PB.SpiralOfTest.Infrastructure.Proxy;

namespace PB.SpiralOfTest.Proxy.EmailProvider
{
    public class EmailProviderProxy : ServiceBusProxy<IEmailProvider>, IEmailProvider
    {
        // TODO: Change ServiceBusProxy to use namespace as queue name instead of using a custom one. Problem: How do we handle the debug queue? Perhaps a helper method that returns another queue name if #debug

        public EmailProviderProxy(string serviceBusConnectionString, string endpointSuffix) 
            : base(serviceBusConnectionString, endpointSuffix)
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
