using System;
using System.Collections.Generic;

namespace DiscordWebhook
{
    public class WebhookMessage
    {
        public string content { get; set; }
        public WebhookMessage(string Content)
        {
            content = Content;
        }
    }
}
