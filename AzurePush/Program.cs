using AzureNotificationHub;
using AzureNotificationHub.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Xml;

namespace AzurePush
{
    class Program
    {

        static void Main(string[] args)
        {

            AzureNotificationHubClient ans = new AzureNotificationHubClient(
                "Endpoint=sb://nsafeliohackathon.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=/zNdq/OZFlu3Vh5uRS0SprP9CPSBEci+AlL5tB0jMIw=", 
                "afelioHackathon"
            );

            var a = ans.ReadAllRegistrations().Result;

            var v = ans.CreateRegistration(new GcmRegistrationDescription()
            {
                GcmRegistrationId = "fFrO_1cEFA4:APA91bFM62iVOs6EsjB0qeWzdXjw0c2RAZABTB3k4-OkcX7QOLTFi7tp2DMPReQZnG3tgZBmnMytgh_sjnxfcftwTmOPluH4KpbH0sYO0hGpmArppcK1J94K-s4zXeL6CPe2CPi3kyHt",
                Tags = "done"
            }).Result;
        }
    }
}