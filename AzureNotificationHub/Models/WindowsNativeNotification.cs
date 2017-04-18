using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace AzureNotificationHub.Models
{
    public class WindowsNativeNotification : Notification
    {
        public string PayLoad { get; set; }

        public WindowsNativeNotification()
        {

        }


        public WindowsNativeNotification(string payLoad)
        {
            PayLoad = payLoad;
        }

        public WindowsNativeNotification(XmlDocument payLoad)
        {
            PayLoad = payLoad.OuterXml;
        }
    }
}
