using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace AzureNotificationHub.Security
{
    public class Token
    {
        public static string CreateToken(string resourceUri, string keyName, string key)
        {
            TimeSpan sinceEpoch = DateTime.UtcNow - new DateTime(1970, 1, 1);
            var expiry = Convert.ToString((int)sinceEpoch.TotalSeconds + (3600 * 24)); //EXPIRES in 1h 
            string stringToSign = Uri.EscapeDataString(resourceUri) + "\n" + expiry;
            HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));

            var signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)));
            var sasToken = String.Format(CultureInfo.InvariantCulture,
            "SharedAccessSignature sr={0}&sig={1}&se={2}&skn={3}",
                Uri.EscapeDataString(resourceUri), Uri.EscapeDataString(signature), expiry, keyName);

            return sasToken;
        }

        public static (string, string, string) ParseConnectionString(string cx)
        {
            String[] parts = cx.Split(';');
            string serviceName = parts[0].Split('.')[0];
            return (serviceName.Substring(14), parts[1].Substring(20), parts[2].Substring(16));
        }
    }
}
