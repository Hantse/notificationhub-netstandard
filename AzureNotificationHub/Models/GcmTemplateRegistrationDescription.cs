using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AzureNotificationHub.Models
{
    public class GcmTemplateRegistrationDescription : GcmRegistrationDescription
    {
        public GcmTemplateRegistrationDescription()
        {

        }

        public GcmTemplateRegistrationDescription(string eTag, string expirationTime, string registrationId, string tags) : base (eTag, expirationTime, registrationId, tags)
        {
            
        }

        public GcmTemplateRegistrationDescription(string eTag, string expirationTime, string registrationId, string tags, string gcmRegistrationId) : base(eTag, expirationTime, registrationId, tags, gcmRegistrationId)
        {
        }


        public GcmTemplateRegistrationDescription(string eTag, string expirationTime, string registrationId, string tags, string gcmRegistrationId, string bodyTemplate, string templateName) : base(eTag, expirationTime, registrationId, tags, gcmRegistrationId)
        {
            BodyTemplate = bodyTemplate;
            TemplateName = templateName;
        }

        public string BodyTemplate { get; set; }

        public string TemplateName { get; set; }

        public override RegistrationDescription Deserialize(IEnumerable<XElement> xml)
        {
            base.Deserialize(xml);

            BodyTemplate = xml.FirstOrDefault(f => f.Name.LocalName == "BodyTemplate")?.Value;
            TemplateName = xml.FirstOrDefault(f => f.Name.LocalName == "TemplateName")?.Value;

            return this;
        }
    }
}
