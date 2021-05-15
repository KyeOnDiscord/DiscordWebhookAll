using System;
using System.Net;
using System.IO;
using RestSharp;
using System.Drawing;
using System.Web.Script.Serialization;
namespace DiscordWebhook
{
    public class Webhook
    {
        private const string APIURL = "https://discord.com/api";
        private static JavaScriptSerializer js = new JavaScriptSerializer();
        /// <summary>
        /// https://discord.com/developers/docs/resources/webhook#create-webhook
        /// Creates a webhook in a channel. Requires a token with "MANAGE_WEBHOOKS" permission
        /// Returns null if token or channel is invalid
        /// </summary>
        /// <param name="token"></param>
        /// <param name="ChannelID"></param>
        public static ChannelWebhook CreateWebhook(string token, ulong ChannelID, string Username, Image avatar = null)
        {
            var client = new RestClient($"{APIURL}/channels/{ChannelID}/webhooks");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", $"Bot {token}");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", js.Serialize(new WebhookBody(Username, avatar)), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            return js.Deserialize<ChannelWebhook>(response.Content);
        }

        /// <summary>
        /// https://discord.com/developers/docs/resources/webhook#get-channel-webhooks
        /// Gets all webhooks in a specified channel. Requires a token with "MANAGE_WEBHOOKS" permission
        /// </summary>
        /// <param name="token"></param>
        /// <param name="ChannelID"></param>
        /// <returns></returns>
        public static ChannelWebhook[] GetChannelWebhooks(string token, ulong ChannelID)
        {
            var client = new RestClient($"{APIURL}/channels/{ChannelID}/webhooks");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bot {token}");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            return js.Deserialize<ChannelWebhook[]>(response.Content);
        }

        /// <summary>
        /// https://discord.com/developers/docs/resources/webhook#get-guild-webhooks
        /// Gets all webhooks in a specified guild. Requires a token with "MANAGE_WEBHOOKS" permission
        /// </summary>
        /// <param name="token"></param>
        /// <param name="ChannelID"></param>
        /// <returns></returns>
        public static ChannelWebhook[] GetGuildWebhooks(string token, ulong GuildID)
        {
            var client = new RestClient($"{APIURL}/guilds/{GuildID}/webhooks");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bot {token}");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            return js.Deserialize<ChannelWebhook[]>(response.Content);
        }

        /// <summary>
        /// https://discord.com/developers/docs/resources/webhook#get-webhook-with-token
        /// Gets a webhook object using a URL
        /// </summary>
        /// <param name="webhookurl"></param>
        /// <returns></returns>
        public static ChannelWebhook GetWebhook(string webhookurl)
        {
            var client = new RestClient($"{webhookurl}");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            return js.Deserialize<ChannelWebhook>(response.Content);
        }


        /// <summary>
        /// https://discord.com/developers/docs/resources/webhook#modify-webhook
        /// Modifies a webhook using a token with "MANAGE_WEBHOOKS" permission.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="WebhookID"></param>
        /// <param name="webhookBody"></param>
        /// <returns></returns>
        public static ChannelWebhook ModifyWebhook(string token, ulong WebhookID, WebhookBody webhookBody)
        {
            var client = new RestClient($"{APIURL}/webhooks/{WebhookID}");
            client.Timeout = -1;
            var request = new RestRequest(Method.PATCH);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", js.Serialize(webhookBody), ParameterType.RequestBody);
            request.AddHeader("Authorization", $"Bot {token}");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            return js.Deserialize<ChannelWebhook>(response.Content);
        }



        /// <summary>
        /// https://discord.com/developers/docs/resources/webhook#modify-webhook-with-token
        /// Modifies a webhook WITHOUT a token and requires the entire webhook URL.
        /// </summary>
        /// <param name="webhookurl"></param>
        /// <param name="webhookBody"></param>
        /// <returns></returns>
        public static ChannelWebhook ModifyWebhook(string webhookurl, WebhookBody webhookBody)
        {
            var client = new RestClient(webhookurl);
            client.Timeout = -1;
            var request = new RestRequest(Method.PATCH);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", js.Serialize(webhookBody), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            return js.Deserialize<ChannelWebhook>(response.Content);
        }


        /// <summary>
        /// https://discord.com/developers/docs/resources/webhook#delete-webhook
        /// Deletes a webhook using a token with "MANAGE_WEBHOOKS" permission.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="WebhookID"></param>
        /// <param name="webhookBody"></param>
        /// <returns></returns>
        public static bool DeleteWebhook(string token, ulong WebhookID)
        {
            var client = new RestClient($"{APIURL}/webhooks/{WebhookID}");
            client.Timeout = -1;
            var request = new RestRequest(Method.DELETE);
            request.AddHeader("Authorization", $"Bot {token}");
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.NoContent)
                return true;
            else
                return false;
        }

        /// <summary>
        /// https://discord.com/developers/docs/resources/webhook#delete-webhook-with-token
        /// Deletes a webhook WITHOUT a token and requires the entire webhook URL.
        /// </summary>
        /// <param name="webhookurl"></param>
        /// <returns></returns>
        public static bool DeleteWebhook(string webhookurl)
        {
            var client = new RestClient(webhookurl);
            client.Timeout = -1;
            var request = new RestRequest(Method.DELETE);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.NoContent)
                return true;
            else
                return false;
        }


        /// <summary>
        /// https://discord.com/developers/docs/resources/webhook#execute-webhook
        /// Sends a webhook with the "content" field
        /// </summary>
        /// <param name="webhookurl"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool SendMessage(string webhookurl, string content)
        {
            var client = new RestClient(webhookurl);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", js.Serialize(new WebhookMessage(content)), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.NoContent)
                return true;
            else
                return false;
        }


        /// <summary>
        /// https://discord.com/developers/docs/resources/webhook#execute-webhook
        /// Sends a webhook which supports raw json
        /// </summary>
        /// <param name="webhookurl"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool SendCustomMsg(string webhookurl, string rawjson)
        {
            var client = new RestClient(webhookurl);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", rawjson, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.NoContent)
                return true;
            else
                return false;
        }







        public static string ImageToBase64(Image image)
        {
                using (MemoryStream _mStream = new MemoryStream())
                {
                    image.Save(_mStream, image.RawFormat);
                    byte[] _imageBytes = _mStream.ToArray();
                    return "data:image/png;base64," + Convert.ToBase64String(_imageBytes);
                }
        }

        public class WebhookBody
        {
            public string name { get; set; }
            public string avatar { get; set; }
            public string channel_id { get; set; }
            public WebhookBody(string Name, Image Avatar)
            {
                name = Name;
                if (Avatar != null)
                avatar = ImageToBase64(Avatar);
            }
            public WebhookBody(string Name = null, Image Avatar = null, string ChannelID = null)
            {
                if (Name != null)
                    name = Name;
                if (Avatar != null)
                    avatar = ImageToBase64(Avatar);
                if (ChannelID != null)
                    channel_id = channel_id;
            }
        }
    }
}
