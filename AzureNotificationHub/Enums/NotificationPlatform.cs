using System;
using System.Collections.Generic;
using System.Text;

namespace AzureNotificationHub.Enums
{
    public enum NotificationPlatform
    {
        Wns = 1,
        //
        // Summary:
        //     APNS Installation Platform
        Apns = 2,
        //
        // Summary:
        //     MPNS Installation Platform
        Mpns = 3,
        //
        // Summary:
        //     GCM Installation Platform
        Gcm = 4,
        //
        // Summary:
        //     ADM Installation Platform
        Adm = 5
    }
}
