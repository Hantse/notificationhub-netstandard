using AzureNotificationHub.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace AzureNotificationHub.Converters
{
    public class RegistrationList
    {
        public static List<RegistrationDescription> Convert(string xml)
        {
            List<RegistrationDescription> converted = new List<RegistrationDescription>();

            try
            {
                XDocument doc = XDocument.Parse(xml);

                IEnumerable<XElement> entries = doc.Root.Elements().Where(a => a.Name.LocalName == "entry");

                foreach (XElement elem in entries)
                {
                    IEnumerable<XElement> contentValues = elem.Elements().FirstOrDefault(f => f.Name.LocalName == "content").Descendants();

                    if (contentValues.Any(a => a.Name.LocalName == "GcmTemplateRegistrationDescription"))
                    {

                        RegistrationDescription desc = new GCMRegistrationDescription(contentValues.FirstOrDefault(f => f.Name.LocalName == "ETag")?.Value,
                            contentValues.FirstOrDefault(f => f.Name.LocalName == "ExpirationTime")?.Value,
                            contentValues.FirstOrDefault(f => f.Name.LocalName == "RegistrationId")?.Value,
                            contentValues.FirstOrDefault(f => f.Name.LocalName == "Tags")?.Value,
                            contentValues.FirstOrDefault(f => f.Name.LocalName == "GcmRegistrationId")?.Value);

                        converted.Add(desc);
                    }
                }
            }
            catch (Exception e)
            {

            }


            return converted;
        }

    }
}
