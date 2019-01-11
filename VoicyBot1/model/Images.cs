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

        /// <summary>
        /// Checks, if given line can be processed.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool IsToProcess(string message) => 
            string.IsNullOrWhiteSpace(message) ? false : message.Trim().ToLower().StartsWith("show-image|", StringComparison.Ordinal);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> ShowImage([Microsoft.AspNetCore.Mvc.FromBody] Activity activity, string message)
        {
            // Check, if line can be processed
            if (!IsToProcess(message)) return null;
            // Leave only passed url or phrase
            var entered = message.Trim().ToLower().Substring("show-image|".Length);
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
