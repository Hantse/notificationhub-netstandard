using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureNotificationHub.Models
{
    public class InstallationTemplate
    {
        public InstallationTemplate() { }

        //
        // Summary:
        //     Gets or sets a template body for notification payload which may contain placeholders
        //     to be filled in with actual data during the send operation
        [JsonProperty(PropertyName = "body")]
        public string Body { get; set; }
        //
        // Summary:
        //     Gets or set collection of headers applicable for MPNS-targeted notifications
        [JsonProperty(PropertyName = "headers", NullValueHandling = NullValueHandling.Ignore)]
        public IDictionary<string, string> Headers { get; set; }
        //
        // Summary:
        //     Gets or sets expiry applicable for APNS-targeted notifications
        [JsonProperty(PropertyName = "expiry", NullValueHandling = NullValueHandling.Ignore)]
        public string Expiry { get; set; }
        //
        // Summary:
        //     Gets or sets collection of tags for particular template. Ususaly only one tag
        //     (template name) should be here.
        [JsonProperty(PropertyName = "tags", NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> Tags { get; set; }
    }
}
