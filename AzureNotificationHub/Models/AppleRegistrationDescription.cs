using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace AzureNotificationHub.Models
{
    public class AppleRegistrationDescription : RegistrationDescription
    {
        public AppleRegistrationDescription()
        {

        }

        public AppleRegistrationDescription(string eTag, string expirationTime, string registrationId, string tags) : base(eTag, expirationTime, registrationId, tags)
        {

        }

        public AppleRegistrationDescription(string eTag, string expirationTime, string registrationId, string tags, string deviceToken) : this(eTag, expirationTime, registrationId, tags)
        {
            DeviceToken = deviceToken;
        }

        public string DeviceToken { get; set; }

        public override RegistrationDescription Deserialize(IEnumerable<XElement> xml)
        {
            base.Deserialize(xml);
            DeviceToken = xml.FirstOrDefault(f => f.Name.LocalName == "DeviceToken")?.Value;
            return this;
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

            XmlElement appleRegistrationDescription = doc.CreateElement("AppleRegistrationDescription", "http://schemas.microsoft.com/netservices/2010/10/servicebus/connect");
            appleRegistrationDescription.SetAttribute("xmlns:i", "http://www.w3.org/2001/XMLSchema-instance");
            content.AppendChild(appleRegistrationDescription);

            XmlElement tags = doc.CreateElement("Tags");
            tags.InnerXml = Tags;
            appleRegistrationDescription.AppendChild(tags);

            XmlElement deviceToken = doc.CreateElement("DeviceToken");
            deviceToken.InnerXml = DeviceToken;
            appleRegistrationDescription.AppendChild(deviceToken);

            return doc.OuterXml.Replace("xmlns=\"\"", "");
        }
    }
}
