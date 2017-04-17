using AzureNotificationHub.Converters;
using AzureNotificationHub.Models;
using AzureNotificationHub.Security;
using Newtonsoft.Json;
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

        #region Registration
        public async Task<List<RegistrationDescription>> ReadAllRegistrationsWithTag(string tag)
        {
            HttpClient hc = GetClient($"tags/{tag}/registrations");

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


        public async Task<RegistrationDescription> CreateOrUpdateRegistration(RegistrationDescription registration)
        {
            if (string.IsNullOrEmpty(registration.RegistrationId) || string.IsNullOrWhiteSpace(registration.RegistrationId))
            {
                return await CreateRegistration(registration);
            }
            else
            {
                return await UpdateRegistration(registration);
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

        public async Task<RegistrationDescription> UpdateRegistration(RegistrationDescription registration)
        {
            HttpClient hc = GetClient($"registrations/{registration.RegistrationId}");

            try
            {
                HttpResponseMessage response = await hc.PutAsync(string.Empty, new StringContent(registration.SerializeAsEntry(), Encoding.UTF8, "application/atom+xml"));
                response.EnsureSuccessStatusCode();

                return registration.Deserialize(await response.Content.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                throw (new Exception("Error on service call", e));
            }
        }

        public async Task<bool> DeleteRegistration(string registrationId)
        {
            HttpClient hc = GetClient($"registrations/{registrationId}");
            hc.DefaultRequestHeaders.Add("If-Match", "*");

            try
            {
                HttpResponseMessage response = await hc.DeleteAsync(string.Empty);
                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (Exception e)
            {
                throw (new Exception("Error on service call", e));
            }
        }

        public async Task<bool> DeleteRegistration(RegistrationDescription registration)
        {
            return await DeleteRegistration(registration.RegistrationId);
        }
        #endregion

        #region Send Notification
        public async Task<string> SendGcmNativeNotification(GcmNativeNotification notification, string tag = null)
        {
            HttpClient hc = GetClient($"messages");
            hc.DefaultRequestHeaders.Add("ServiceBusNotification-Format", "gcm");

            if (tag != null)
            {
                hc.DefaultRequestHeaders.Add("ServiceBusNotification-Tags", tag);
            }

            return await SendNativeNotification(hc, notification);
        }

        public async Task<string> SendApnsNativeNotification(ApnsNativeNotification notification, string tag = null)
        {
            HttpClient hc = GetClient($"messages");
            hc.DefaultRequestHeaders.Add("ServiceBusNotification-Format", "apple");

            if (tag != null)
            {
                hc.DefaultRequestHeaders.Add("ServiceBusNotification-Tags", tag);
            }

            return await SendNativeNotification(hc, notification);
        }

        private async Task<string> SendNativeNotification(HttpClient hc, NativeNotification notification)
        {
            try
            {
                HttpResponseMessage response = await hc.PostAsync(string.Empty, new StringContent(JsonConvert.SerializeObject(notification), Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();
                return (response.Headers.Location?.ToString().Replace("messages", ""));
            }
            catch (Exception e)
            {
                throw (new Exception("Error on service call", e));
            }
        }
        #endregion
    }
}
