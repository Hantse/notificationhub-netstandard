using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace AzureNotificationHub.Models
{
    public class GcmRegistrationDescription : RegistrationDescription
    {
        public GcmRegistrationDescription()
        {

        }

        public GcmRegistrationDescription(string eTag, string expirationTime, string registrationId, string tags) : base(eTag, expirationTime, registrationId, tags)
        {

        }

        public GcmRegistrationDescription(string eTag, string expirationTime, string registrationId, string tags, string gcmRegistrationId) : this(eTag, expirationTime, registrationId, tags)
        {
            GcmRegistrationId = gcmRegistrationId;
        }

        public string GcmRegistrationId { get; set; }

        public override RegistrationDescription Deserialize(IEnumerable<XElement> xml)
        {
            base.Deserialize(xml);
            GcmRegistrationId = xml.FirstOrDefault(f => f.Name.LocalName == "GcmRegistrationId")?.Value;
            return this;
        }

        public override RegistrationDescription Deserialize(string xml)
        {
            XDocument doc = XDocument.Parse(xml);

            return Deserialize(doc.Elements().FirstOrDefault(f => f.Name.LocalName == "entry").Elements().FirstOrDefault(f => f.Name.LocalName == "content").Descendants());
        }

        public override string SerializeAsEntry()
        {
            XmlDocument doc = new XmlDocument();

            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);

            XmlElement entry = doc.CreateElement(string.Empty, "entry", "http://www.w3.org/2005/Atom");
            doc.AppendChild(entry);

            XmlElement content = doc.CreateElement("content");
            content.SetAttribute("type", "application/xml");
            entry.AppendChild(content);

            XmlElement gcmRegistrationDescription = doc.CreateElement("GcmRegistrationDescription", "http://schemas.microsoft.com/netservices/2010/10/servicebus/connect");
            gcmRegistrationDescription.SetAttribute("xmlns:i", "http://www.w3.org/2001/XMLSchema-instance");
            content.AppendChild(gcmRegistrationDescription);

            XmlElement tags = doc.CreateElement("Tags");
            tags.InnerXml = Tags;
            gcmRegistrationDescription.AppendChild(tags);

            XmlElement gcmRegistrationId = doc.CreateElement("GcmRegistrationId");
            gcmRegistrationId.InnerXml = GcmRegistrationId;
            gcmRegistrationDescription.AppendChild(gcmRegistrationId);

            return doc.OuterXml.Replace("xmlns=\"\"", "");
        }
    }
}
