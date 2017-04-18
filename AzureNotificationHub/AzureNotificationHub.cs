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
        /// <summary>
        /// Hub SharedKey
        /// </summary>
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

        #region HubManagement
        // TODO Create hub
        public async Task<bool> CreateHub()
        {
            return true;
        }
        // TODO Create hub
        public async Task<bool> ReadHub()
        {
            return true;
        }
        // TODO Update hub
        public async Task<bool> UpdateHub()
        {
            return true;
        }
        // TODO Update hub
        public async Task<bool> DeleteHub()
        {
            return true;
        }
        #endregion

        #region Namespace PNS Credentials
        /// <summary>
        /// The namespace level Push Notification Services (PNS) APIs are designed for large apps that span across multiple hubs. They enable developers to easily read and update unified PNS settings for multiple hubs under the same namespace at once. When PNS credentials have been set at namespace level using this endpoint, all hubs in the namespace will use the credentials provided with the namespace settings. Hub creations in the namespace with PNS credentials won’t be allowed.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CreateOrUpdatePNSCredentials()
        {
            return true;
        }

        /// <summary>
        /// The namespace level Push Notification Services (PNS) APIs are designed for large apps that span across multiple hubs. They enable developers to easily read and update unified PNS settings for multiple hubs under the same namespace at once. When PNS credentials have been set at namespace level using this endpoint, all hubs in the namespace will use the credentials provided with the namespace settings. Hub creations in the namespace with PNS credentials won’t be allowed. When PNS credentials are set at hub level first, this endpoint will no longer be available.
        /// This topic is a reference for getting the unified PNS credentials for a namespace.When PNS credentials are set at hub level first, this endpoint will no longer be available.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ReadPNSCredentials()
        {
            return true;
        }
        #endregion

        #region Registration
        /// <summary>
        /// Retrieves all registrations with a specific tag.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Retrieves all registrations.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Creates or updates a registration in the specified location.
        /// </summary>
        /// <param name="registration"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates a new registration. This method generates a registration ID, which you can subsequently use to retrieve, update, and delete this registration.
        /// </summary>
        /// <param name="registration"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Updates an existing registration.
        /// </summary>
        /// <param name="registration"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Deletes a registration.
        /// </summary>
        /// <param name="registrationId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Deletes a registration.
        /// </summary>
        /// <param name="registration"></param>
        /// <returns></returns>
        public async Task<bool> DeleteRegistration(RegistrationDescription registration)
        {
            return await DeleteRegistration(registration.RegistrationId);
        }
        #endregion

        #region Installation
        /// <summary>
        /// Creates or overwrites an installation.
        /// </summary>
        /// <returns></returns>
        public async Task<string> CreateOrOverwriteInstallation(Installation installation)
        {
            HttpClient hc = GetClient($"installations/{installation?.InstallationId}");

            try
            {
                HttpResponseMessage response = await hc.PutAsync(string.Empty, new StringContent(JsonConvert.SerializeObject(installation), Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();
                return (response.Headers.Location?.ToString().Replace($"{ServiceUrl}/installations/", ""));
            }
            catch (Exception e)
            {
                throw (new Exception("Error on service call", e));
            }
        }
        /// <summary>
        /// Retrieves installations based on Installation ID.
        /// </summary>
        /// <returns></returns>
        public async Task<Installation> ReadInstallation(string installationId)
        {
            HttpClient hc = GetClient($"installations/{installationId}");

            try
            {
                HttpResponseMessage response = await hc.GetAsync(string.Empty);
                response.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<Installation>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                throw (new Exception("Error on service call", e));
            }
        }
        /// <summary>
        /// Azure Notification Hubs supports partial updates to an installation using the JSON-Patch standard in RFC6902.
        /// </summary>
        /// <returns></returns>
        public async Task<string> UpdateInstallation()
        {
            return null;
        }
        /// <summary>
        /// Deletes an installation.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> DeleteInstallation(string installationId)
        {
            HttpClient hc = GetClient($"installations/{installationId}");

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

        #endregion

        #region Send Notification

        /// <summary>
        /// Sends a notification directly to a device handle (a valid token as expressed by the Notification type). Users of this API do not need to use Registrations or Installations. Instead, users of this API manage all devices on their own and use Azure Notification Hub solely as a pass through service to communicate with the various Push Notification Services.
        /// In Standard Hub and ID is return.
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="handle"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public async Task<string> DirectSend(Notification notification, string handle, string tag = null)
        {
            HttpClient hc = GetClient($"messages/?direct");
            hc.DefaultRequestHeaders.Add("ServiceBusNotification-DeviceHandle", handle);

            if (tag != null)
            {
                hc.DefaultRequestHeaders.Add("ServiceBusNotification-Tags", tag);
            }

            Type notificationType = notification.GetType();

            if (notificationType == typeof(GcmNativeNotification))
            {
                hc.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                hc.DefaultRequestHeaders.Add("ServiceBusNotification-Format", "gcm");
            }
            else if (notificationType == typeof(ApnsNativeNotification))
            {
                hc.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                hc.DefaultRequestHeaders.Add("ServiceBusNotification-Format", "apple");
            }
            else
            {
                hc.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", " application/xml;charset=utf-8");
            }

            try
            {
                HttpResponseMessage response = await hc.PostAsync(string.Empty, new StringContent(JsonConvert.SerializeObject(notification)));
                response.EnsureSuccessStatusCode();
                return (response.Headers.Location?.ToString().Replace($"{ServiceUrl}/messages/", "").Replace($"?api-version=2015-01", ""));
            }
            catch (Exception e)
            {
                throw (new Exception("Error on service call", e));
            }
        }

        /// <summary>
        /// Sends a GCM native notification through a notification hub.
        /// In Standard Hub and ID is return.
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Sends an APNS native notification through a notification hub.
        /// In Standard Hub and ID is return.
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Sends an MPNS native notification through a notification hub.
        /// </summary>
        /// <returns></returns>
        public async Task<string> SendMpnsNativeNotification(WindowsNativeNotification notification, string tag = null)
        {
            HttpClient hc = GetClient($"messages");
            hc.DefaultRequestHeaders.Add("ServiceBusNotification-Format", "windowsphone");

            if (tag != null)
            {
                hc.DefaultRequestHeaders.Add("ServiceBusNotification-Tags", tag);
            }

            return await SendNativeNotification(hc, notification);
        }

        /// <summary>
        /// Sends an WNS native notification through a notification hub.
        /// </summary>
        /// <returns></returns>
        public async Task<string> SendWindowsNativeNotification(WindowsNativeNotification notification, string tag = null)
        {
            HttpClient hc = GetClient($"messages");
            hc.DefaultRequestHeaders.Add("ServiceBusNotification-Format", "windows");

            if (tag != null)
            {
                hc.DefaultRequestHeaders.Add("ServiceBusNotification-Tags", tag);
            }

            return await SendNativeNotification(hc, notification);
        }

        /// <summary>
        /// Sends a notification to a template registration on a notification hub.
        /// </summary>
        /// <returns></returns>
        public async Task<string> SendTemplateNotification(TemplateNotification notification, string tag = null)
        {
            HttpClient hc = GetClient($"messages");

            if (tag != null)
            {
                hc.DefaultRequestHeaders.Add("ServiceBusNotification-Tags", tag);
            }

            try
            {
                HttpResponseMessage response = await hc.PostAsync(string.Empty, new StringContent(notification.PayLoad, Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();
                return (response.Headers.Location?.ToString().Replace($"{ServiceUrl}/messages/", "").Replace($"?api-version=2015-01", ""));
            }
            catch (Exception e)
            {
                throw (new Exception("Error on service call", e));
            }
        }

        private async Task<string> SendNativeNotification(HttpClient hc, Notification notification)
        {
            HttpResponseMessage response;

            try
            {
                if (notification.GetType() == typeof(WindowsNativeNotification) || notification.GetType() == typeof(MpnsNativeNotification))
                    response = await hc.PostAsync(string.Empty, new StringContent(((WindowsNativeNotification)notification).PayLoad, Encoding.UTF8, "application/xml"));
                else
                    response = await hc.PostAsync(string.Empty, new StringContent(JsonConvert.SerializeObject(notification), Encoding.UTF8, "application/json"));

                response.EnsureSuccessStatusCode();
                return (response.Headers.Location?.ToString().Replace($"{ServiceUrl}/messages/", "").Replace($"?api-version=2015-01", ""));
            }
            catch (Exception e)
            {
                throw (new Exception("Error on service call", e));
            }
        }
        #endregion
    }
}
