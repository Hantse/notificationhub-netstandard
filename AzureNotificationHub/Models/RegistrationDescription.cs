using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
