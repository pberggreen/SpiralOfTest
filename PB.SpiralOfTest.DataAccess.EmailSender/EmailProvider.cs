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
            Debug.WriteLine("EmailProviderAccess.SendEmail({0}, {1}, {2})", receiverEmail, subject, body);
        }
    }

}
