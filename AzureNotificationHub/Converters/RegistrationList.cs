using AzureNotificationHub.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

                    if (contentValues.Any(a => a.Name.LocalName == "GcmRegistrationDescription"))
                    {
                        converted.Add(new GcmRegistrationDescription().Deserialize(contentValues));
                    }
                    else if (contentValues.Any(a => a.Name.LocalName == "GcmTemplateRegistrationDescription"))
                    {
                        converted.Add(new GcmTemplateRegistrationDescription().Deserialize(contentValues));
                    }
                    else if (contentValues.Any(a => a.Name.LocalName == "AppleRegistrationDescription"))
                    {
                        converted.Add(new AppleRegistrationDescription().Deserialize(contentValues));
                    }
                    else if (contentValues.Any(a => a.Name.LocalName == "AppleTemplateRegistrationDescription"))
                    {
                        converted.Add(new AppleTemplateRegistrationDescription().Deserialize(contentValues));
                    }
                    else if (contentValues.Any(a => a.Name.LocalName == "AdmRegistrationDescription"))
                    {
                        converted.Add(new AdmRegistrationDescription().Deserialize(contentValues));
                    }
                    else if (contentValues.Any(a => a.Name.LocalName == "AdmTemplateRegistrationDescription"))
                    {
                        converted.Add(new AdmTemplateRegistrationDescription().Deserialize(contentValues));
                    }
                    else if (contentValues.Any(a => a.Name.LocalName == "BaiduTemplateRegistrationDescription"))
                    {

                    }
                    else if (contentValues.Any(a => a.Name.LocalName == "BaiduRegistrationDescription"))
                    {

                    }
                    else if (contentValues.Any(a => a.Name.LocalName == "MpnsRegistrationDescription"))
                    {
                        converted.Add(new MpnsRegistrationDescription().Deserialize(contentValues));
                    }
                    else if (contentValues.Any(a => a.Name.LocalName == "MpnsTemplateRegistrationDescription"))
                    {
                        converted.Add(new MpnsTemplateRegistrationDescription().Deserialize(contentValues));
                    }
                    else if (contentValues.Any(a => a.Name.LocalName == "WindowsRegistrationDescription"))
                    {
                        converted.Add(new WnsRegistrationDescription().Deserialize(contentValues));
                    }
                    else if (contentValues.Any(a => a.Name.LocalName == "WindowsTemplateRegistrationDescription"))
                    {
                        converted.Add(new WnsTemplateRegistrationDescription().Deserialize(contentValues));
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error, template type not found.");
            }


            return converted;
        }

    }
}
