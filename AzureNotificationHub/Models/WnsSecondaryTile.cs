using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureNotificationHub.Models
{
    public class WnsSecondaryTile
    {
        public WnsSecondaryTile()
        {

        }

        //
        // Summary:
        //     Gets or sets the push channel.
        [JsonProperty(Required = Required.Always, PropertyName = "pushChannel")]
        public string PushChannel { get; set; }
        //
        // Summary:
        //     Gets or sets the push channel expiration property.
        [JsonProperty(PropertyName = "pushChannelExpired")]
        public bool? PushChannelExpired { get; set; }
        //
        // Summary:
        //     Gets or sets the tags.
        [JsonProperty(PropertyName = "tags", NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> Tags { get; set; }
        //
        // Summary:
        //     Gets or sets the dictionary of Templates.
        [JsonProperty(PropertyName = "templates", NullValueHandling = NullValueHandling.Ignore)]
        public IDictionary<string, InstallationTemplate> Templates { get; set; }
    }
}
