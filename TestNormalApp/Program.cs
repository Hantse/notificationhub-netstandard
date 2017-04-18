using Microsoft.Azure.NotificationHubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestNormalApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Installation i = new Installation();

            NotificationHubClient hc = NotificationHubClient.CreateClientFromConnectionString("", "");
        }
    }
}
