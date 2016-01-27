using System;
using System.Diagnostics;
using System.ServiceModel;
using PB.SpiralOfTest.Access.EmailProvider;
using PB.SpiralOfTest.Access.EmailTemplate;
using PB.SpiralOfTest.Infrastructure.Service;

namespace PB.SpiralOfTest.Engine.EmailSender
{
    [ServiceContract]
    public interface IEmailSenderEngine
    {
        [OperationContract]
        void SendEmail(string templateName, string emailAddress);
    }

    public class EmailSenderEngine : ServiceBase, IEmailSenderEngine
    {
        public void SendEmail(string templateName, string emailAddress)
        {
            Debug.WriteLine("EmailSender.SendEmail({0}, {1})", templateName, emailAddress);

            var emailTemplateAccess = GetProxy<IEmailTemplateAccess>();
            var template = emailTemplateAccess.GetEmailTemplate(templateName);

            var emailSender = GetProxy<IEmailProviderAccess>();
            emailSender.SendEmail(emailAddress, template.Subject, template.Body);
        }
    }

}
