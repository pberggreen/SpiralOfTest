using System;
using System.Diagnostics;
using System.ServiceModel;

namespace PB.SpiralOfTest.Access.EmailTemplate
{
    [ServiceContract]
    public interface IEmailTemplateAccess
    {
        [OperationContract]
        EmailTemplate GetEmailTemplate(string templateName);
    }

    public class EmailTemplateAccess : IEmailTemplateAccess
    {
        public EmailTemplate GetEmailTemplate(string templateName)
        {
            Debug.WriteLine($"EmailTemplateAccess.GetEmailTemplate({templateName})");
            return new EmailTemplate();
        }
    }

    public class EmailTemplate
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
    }

}
