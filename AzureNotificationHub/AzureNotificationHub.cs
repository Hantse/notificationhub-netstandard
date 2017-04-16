using AzureNotificationHub.Converters;
using AzureNotificationHub.Models;
using AzureNotificationHub.Security;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AzureNotificationHub
{
    public class AzureNotificationHubClient
    {
        public string SAS { get; set; }

        public string ServiceUrl { get; set; }

        public string SASKey { get; set; }

        public string SASKeyName { get; set; }

        public AzureNotificationHubClient(string _sAS, string _serviceName)
        {
            try
            {
                (string, string, string) parsedConnection = Token.ParseConnectionString(_sAS);
                SAS = _sAS;
                ServiceUrl = $"https://{parsedConnection.Item1}.servicebus.windows.net/{_serviceName}";
                SASKeyName = parsedConnection.Item2;
                SASKey = parsedConnection.Item3;
            }
            catch (Exception e)
            {
                throw (new Exception("Invalid SAS or ServiceURL", e));
            }
        }

        private HttpClient GetClient(string url)
        {
            HttpClient hc = new HttpClient();

            hc.BaseAddress = new Uri($"{ServiceUrl}/{url}/?api-version=2015-01");
            hc.DefaultRequestHeaders.Clear();
            hc.DefaultRequestHeaders.Add("Authorization", Token.CreateToken(ServiceUrl, SASKeyName, SASKey));
            return hc;
        }

        public async Task<List<RegistrationDescription>> ReadAllRegistrations()
        {
            HttpClient hc = GetClient("registrations");

            try
            {
                HttpResponseMessage response = await hc.GetAsync(string.Empty);
                response.EnsureSuccessStatusCode();
                return RegistrationList.Convert(await response.Content.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                throw (new Exception("Error on service call", e));
            }
        }

        public async Task<RegistrationDescription> CreateRegistration(RegistrationDescription registration)
        {
            HttpClient hc = GetClient("registrations");

            try
            {
                HttpResponseMessage response = await hc.PostAsync(string.Empty, new StringContent(registration.SerializeAsEntry(), Encoding.UTF8, "application/atom+xml"));
                response.EnsureSuccessStatusCode();

                return registration.Deserialize(await response.Content.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                throw (new Exception("Error on service call", e));
            }
        }
    }
}
