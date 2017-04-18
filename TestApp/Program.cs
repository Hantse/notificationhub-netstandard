using AzureNotificationHub;
using System;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            AzureNotificationHubClient ans = new AzureNotificationHubClient(
                "AccesKey",
                "HubName"
            );

        }
    }
}