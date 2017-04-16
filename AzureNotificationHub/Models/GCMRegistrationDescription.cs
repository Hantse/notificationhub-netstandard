using System;
using System.Collections.Generic;
using System.Text;

namespace AzureNotificationHub.Models
{
    public class GCMRegistrationDescription : RegistrationDescription
    {
        public string GcmRegistrationId { get; set; }

        public GCMRegistrationDescription(string eTag, string expirationTime, string registrationId, string tags) : base (eTag, expirationTime, registrationId, tags)
        {
            
        }

        public GCMRegistrationDescription(string eTag, string expirationTime, string registrationId, string tags, string gcmRegistrationId) : this(eTag, expirationTime, registrationId, tags)
        {
            GcmRegistrationId = gcmRegistrationId;
        }
    }
}
