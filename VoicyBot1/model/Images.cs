using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;
using VoicyBot1.backend;

namespace VoicyBot1.model
{
    public class Images
    {
        private UtilRequest _utilRequest;

        private void AssureNN_UtilRequest()
        {
            if (_utilRequest == null)
                _utilRequest = UtilRequest.Instance;
        }

        public async Task<HttpResponseMessage> ShowImage([Microsoft.AspNetCore.Mvc.FromBody] Activity activity, string message)
        {
            // Check, if line is empty
            if (string.IsNullOrWhiteSpace(message)) return null;
            // Trim message and 
            string entered = message.Trim().ToLower();
            // Check, if message contains a command show image
            if (!entered.StartsWith("show-image|", StringComparison.Ordinal)) return null;
            // Leave only passed url or phrase
            entered = entered.Substring("show-image|".Length);
            // Assert presence of util to process requests
            AssureNN_UtilRequest();

            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            Activity reply = activity.CreateReply(entered);
            reply.Recipient = activity.From;
            reply.Type = "message";
            reply.Attachments = new List<Attachment>();
            if (entered.Equals("author") || entered.Equals("who made you"))
            {
                // Show author's github avatar
                reply.Attachments.Add(new Attachment()
                {
                    ContentUrl = "https://avatars2.githubusercontent.com/u/12435750?s=460&v=4",
                    ContentType = "image/png",
                    Name = "ttrzcinski.png"
                });
            }
            else if (UtilRequest.IsURL(entered))
            {
                // Show requested ur.l's image
                reply.Attachments.Add(new Attachment()
                {
                    ContentUrl = entered,
                    ContentType = "image/png",
                    Name = "downloaded_from_url.png"
                });
                // TODO show first image with given phrase
            }
            else
            {
                // Show default image
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
