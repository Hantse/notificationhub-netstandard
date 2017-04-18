using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureNotificationHub.Models
{
    public class ApnsNativeNotification : Notification
    {
        [JsonProperty("aps")]
        public dynamic Aps { get; set; }
    }
}
