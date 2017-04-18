using System;
using System.Collections.Generic;
using System.Text;

namespace AzureNotificationHub.Models
{
    public class TemplateNotification
    {
        public string PayLoad { get; set; }

        public TemplateNotification(string payLoad)
        {
            PayLoad = payLoad;
        }
    }
}
