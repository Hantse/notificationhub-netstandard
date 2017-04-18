using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace AzureNotificationHub.Models
{
    public class RegistrationDescription
    {
        public RegistrationDescription()
        {

        }

        public RegistrationDescription(string eTag, string expirationTime, string registrationId, string tags)
        {
            ETag = eTag;
            ExpirationTime = expirationTime;
            RegistrationId = registrationId;
            Tags = tags;
        }

        public string ETag { get; set; }
        public string ExpirationTime { get; set; }
        public string RegistrationId { get; set; }
        public string Tags { get; set; }

        public string[] TagsAsList
        {
            get
            {
                return Tags.Split(',');
            }
        }

        public virtual RegistrationDescription Deserialize(IEnumerable<XElement> xml)
        {
            ETag = xml.FirstOrDefault(f => f.Name.LocalName == "ETag")?.Value;
            ExpirationTime = xml.FirstOrDefault(f => f.Name.LocalName == "ExpirationTime")?.Value;
            RegistrationId = xml.FirstOrDefault(f => f.Name.LocalName == "RegistrationId")?.Value;
            Tags = xml.FirstOrDefault(f => f.Name.LocalName == "Tags")?.Value;

            return this;
        }

        public void addTag(string tag)
        {
            if (tag.IndexOfAny(new char[] { '*', '&', '#', '\'', '"', ')', '(' }) > -1)
                throw new Exception("Invalid tag"); 

            if(Tags.Length > 0)
            {
                Tags += ",";
            }

            Tags += tag;
        }

        public virtual RegistrationDescription Deserialize(string xml)
        {
            XDocument doc = XDocument.Parse(xml);

            return Deserialize(doc.Elements().FirstOrDefault(f => f.Name.LocalName == "entry").Elements().FirstOrDefault(f => f.Name.LocalName == "content").Descendants());
        }

        public virtual string SerializeAsEntry()
        {
            throw new NotImplementedException();
        }
    }
}
