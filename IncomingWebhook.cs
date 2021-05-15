namespace DiscordWebhook
{
    // https://discord.com/developers/docs/resources/webhook#webhook-object-example-incoming-webhook
    public class User
        {
            public string id { get; set; }
            public string username { get; set; }
            public string avatar { get; set; }
            public string discriminator { get; set; }
            public int public_flags { get; set; }
            public bool bot { get; set; }
        }

        public class ChannelWebhook
        {
            public int type { get; set; }
            public string id { get; set; }
            public string name { get; set; }
            public object avatar { get; set; }
            public string channel_id { get; set; }
            public string guild_id { get; set; }
            public string application_id { get; set; }
            public string token { get; set; }
            public User user { get; set; }
        }
    
}
