using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureNotificationHub.Models
{
    public class GcmNativeNotification : Notification
    {
        [JsonProperty("collapse_key")]
        private string CollapseKey { get; set; }

        [JsonProperty("time_to_live")]
        private int TimeToLive { get; set; }

        [JsonProperty("delay_while_idle")]
        private bool DelayWhileIdle { get; set; }

        [JsonProperty("data")]
        public dynamic Data { get; set; }
    }
}
