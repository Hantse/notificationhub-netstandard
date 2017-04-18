using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace AzureNotificationHub.Models
{
    public class WnsRegistrationDescription : RegistrationDescription
    {
        public string ChannelUri { get; set; }

        public WnsRegistrationDescription()
        {

        }

        public WnsRegistrationDescription(string eTag, string expirationTime, string registrationId, string tags) : base(eTag, expirationTime, registrationId, tags)
        {

        }

        public WnsRegistrationDescription(string eTag, string expirationTime, string registrationId, string tags, string channelUri) : base(eTag, expirationTime, registrationId, tags)
        {
            ChannelUri = channelUri;
        }

        public override RegistrationDescription Deserialize(IEnumerable<XElement> xml)
        {
            base.Deserialize(xml);
            ChannelUri = xml.FirstOrDefault(f => f.Name.LocalName == "ChannelUri")?.Value;
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

            XmlElement windowsRegistrationDescription = doc.CreateElement("WindowsRegistrationDescription ", "http://schemas.microsoft.com/netservices/2010/10/servicebus/connect");
            windowsRegistrationDescription.SetAttribute("xmlns:i", "http://www.w3.org/2001/XMLSchema-instance");
            content.AppendChild(windowsRegistrationDescription);

            XmlElement tags = doc.CreateElement("Tags");
            tags.InnerXml = Tags;
            windowsRegistrationDescription.AppendChild(tags);

            XmlElement windowsChannelUri = doc.CreateElement("ChannelUri");
            windowsChannelUri.InnerXml = ChannelUri;
            windowsRegistrationDescription.AppendChild(windowsChannelUri);

            return doc.OuterXml.Replace("xmlns=\"\"", "");
        }
    }
}
