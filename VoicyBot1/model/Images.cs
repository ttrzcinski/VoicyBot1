using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;

namespace VoicyBot1.model
{
    public class Images
    {
        public async Task<HttpResponseMessage> ShowImage([Microsoft.AspNetCore.Mvc.FromBody] Activity activity, string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return null;
            message = message.Trim().ToLower();

            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            Activity reply = activity.CreateReply(message);
            reply.Recipient = activity.From;
            reply.Type = "message";
            reply.Attachments = new List<Attachment>();
            if (message.Equals("author") || message.Equals("who made you"))
            {
                reply.Attachments.Add(new Attachment()
                {
                    ContentUrl = "https://avatars2.githubusercontent.com/u/12435750?s=460&v=4",
                    ContentType = "image/png",
                    Name = "ttrzcinski.png"
                });
            }
            else
            {
                reply.Attachments.Add(new Attachment()
                {
                    ContentUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/1/1f/Creative-Tail-Animal-duck.svg/128px-Creative-Tail-Animal-duck.svg.png",
                    ContentType = "image/png",
                    Name = "robot.png"
                });
            }

            await connector.Conversations.ReplyToActivityAsync(reply);
            return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
        }

    }
}
