using System.Diagnostics;
using System.ServiceModel;
using PB.SpiralOfTest.Infrastructure.Service;

namespace PB.SpiralOfTest.Access.EmailProvider
{
    [ServiceContract]
    public interface IEmailProviderAccess
    {
        [OperationContract]
        void SendEmail(string receiverEmail, string subject, string body);
    }

    public class EmailProviderAccess : ServiceBase, IEmailProviderAccess
    {
        public void SendEmail(string receiverEmail, string subject, string body)
        {
            // TODO: Use email provider proxy to call email provider through the service bus
            // TODO: Add a circuit breaker pattern (https://timross.wordpress.com/2008/02/17/implementing-the-circuit-breaker-pattern-in-c-part-2/)
            // TODO: Allthough a service bus call may not be the most logical case to use a circuit breaker
            Debug.WriteLine("EmailProviderAccess.SendEmail({0}, {1}, {2})", receiverEmail, subject, body);
        }
    }

}
