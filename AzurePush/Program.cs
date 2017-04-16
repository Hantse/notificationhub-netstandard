using AzureNotificationHub;
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
        }
    }
}