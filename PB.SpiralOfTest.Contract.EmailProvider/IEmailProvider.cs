using System.ServiceModel;

namespace PB.SpiralOfTest.Contract.EmailProvider
{
    [ServiceContract]
    public interface IEmailProvider
    {
        [OperationContract(IsOneWay = true)]
        void SendEmail(string receiverEmail, string subject, string body);
    }

}
