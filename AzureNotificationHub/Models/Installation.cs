using AzureNotificationHub.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureNotificationHub.Models
{
    public class Installation
    {
        public Installation() { }

        //
        // Summary:
        //     Get or sets unique identifier for the installation
        [JsonProperty(PropertyName = "installationId")]
        public string InstallationId { get; set; }
        //
        // Summary:
        //     Gets or set registration id, token or URI obtained from platform-specific notification
        //     service
        [JsonProperty(PropertyName = "pushChannel")]
        public string PushChannel { get; set; }
        //
        // Summary:
        //     Gets if installation is expired or not
        [JsonProperty(PropertyName = "pushChannelExpired")]
        public bool? PushChannelExpired { get; set; }
        //
        // Summary:
        //     Gets or sets notification platform for the installation
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(PropertyName = "platform")]
        public NotificationPlatform Platform { get; set; }
        //
        // Summary:
        //     Gets expiration for the installation
        [JsonProperty(PropertyName = "expirationTime")]
        public DateTime? ExpirationTime { get; set; }
        //
        // Summary:
        //     Gets or sets collection of tags
        [JsonProperty(PropertyName = "tags", NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> Tags { get; set; }
        //
        // Summary:
        //     Gets or sets collection of push variables
        [JsonProperty(PropertyName = "pushVariables", NullValueHandling = NullValueHandling.Ignore)]
        public IDictionary<string, string> PushVariables { get; set; }
        //
        // Summary:
        //     Gets or sets collection of templates
        [JsonProperty(PropertyName = "templates", NullValueHandling = NullValueHandling.Ignore)]
        public IDictionary<string, InstallationTemplate> Templates { get; set; }
        //
        // Summary:
        //     Gets or sets collection of secondary tiles for WNS
        [JsonProperty(PropertyName = "secondaryTiles", NullValueHandling = NullValueHandling.Ignore)]
        public IDictionary<string, WnsSecondaryTile> SecondaryTiles { get; set; }
    }
}
