using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;
using VoicyBot1.backend;
using Microsoft.Bot.Builder;
using System.Threading;
using System.IO;
using System.Linq;
using System.Net;

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
            var entered = message.TrimStart().ToLower().Substring("show-image|".Length).Trim();
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

        /// <summary>
        /// Creates an <see cref="Attachment"/> to be sent from the bot to the user from a HTTP URL.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the operation result.</returns>
        public static Attachment GetInternetAttachment()
        {
            // ContentUrl must be HTTPS.
            return new Attachment
            {
                Name = @"Resources\imgs\rick1.jpg",
                ContentType = "image/jpg",
                ContentUrl = "https://i.kym-cdn.com/photos/images/original/000/369/488/61d.jpeg",
            };
        }

        /// <summary>
        /// Creates an <see cref="Attachment"/> to be sent from the bot to the user from an uploaded file.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the operation result.</returns>
        public static async Task<Attachment> GetUploadedAttachmentAsync(string serviceUrl, string conversationId)
        {
            if (string.IsNullOrWhiteSpace(serviceUrl))
            {
                throw new ArgumentNullException(nameof(serviceUrl));
            }

            if (string.IsNullOrWhiteSpace(conversationId))
            {
                throw new ArgumentNullException(nameof(conversationId));
            }

            var imagePath = Path.Combine(Environment.CurrentDirectory, @"Resources\imgs\rick1.jpg");

            // Create a connector client to use to upload the image.
            using (var connector = new ConnectorClient(new Uri(serviceUrl)))
            {
                var attachments = new Attachments(connector);
                var response = await attachments.Client.Conversations.UploadAttachmentAsync(
                    conversationId,
                    new AttachmentData
                    {
                        Name = @"Resources\imgs\rick1.jpg",
                        OriginalBase64 = File.ReadAllBytes(imagePath),
                        Type = "image/jpg",
                    });

                var attachmentUri = attachments.GetAttachmentUri(response.Id);

                return new Attachment
                {
                    Name = @"Resources\imgs\rick1.jpg",
                    ContentType = "image/jpg",
                    ContentUrl = attachmentUri,
                };
            }
        }

        /// <summary>
        /// Creates an inline attachment sent from the bot to the user using a base64 string.
        /// </summary>
        /// <returns>An <see cref="Attachment"/> to be displayed to the user.</returns>
        /// <remarks>
        /// Using a base64 string to send an attachment will not work on all channels.
        /// Additionally, some channels will only allow certain file types to be sent this way.
        /// For example a .png file may work but a .pdf file may not on some channels.
        /// Please consult the channel documentation for specifics.
        /// </remarks>
        public static Attachment GetInlineAttachment()
        {
            var imagePath = Path.Combine(Environment.CurrentDirectory, @"Resources\imgs\rick1.jpg");
            var imageData = Convert.ToBase64String(File.ReadAllBytes(imagePath));

            return new Attachment
            {
                Name = @"Resources\imgs\rick1.jpg",
                ContentType = "image/jpg",
                ContentUrl = $"data:image/jpg;base64,{imageData}",
            };
        }

        /// <summary>
        /// Handle attachments uploaded by users. The bot receives an <see cref="Attachment"/> in an <see cref="Activity"/>.
        /// The activity has a <see cref="IList{T}"/> of attachments.
        /// </summary>
        /// <remarks>
        /// Not all channels allow users to upload files. Some channels have restrictions
        /// on file type, size, and other attributes. Consult the documentation for the channel for
        /// more information. For example Skype's limits are here
        /// <see ref="https://support.skype.com/en/faq/FA34644/skype-file-sharing-file-types-size-and-time-limits"/>.
        /// </remarks>
        private static void HandleIncomingAttachment(IMessageActivity activity, IMessageActivity reply)
        {
            foreach (var file in activity.Attachments)
            {
                // Determine where the file is hosted.
                var remoteFileUrl = file.ContentUrl;

                // Save the attachment to the system temp directory.
                var localFileName = Path.Combine(Path.GetTempPath(), file.Name);

                // Download the actual attachment
                using (var webClient = new WebClient())
                {
                    webClient.DownloadFile(remoteFileUrl, localFileName);
                }

                reply.Text = $"Attachment \"{activity.Attachments[0].Name}\"" +
                             $" has been received and saved to \"{localFileName}\"";
            }
        }

        /// <summary>
        /// Adds an attachment to the 'reply' parameter that is passed in.
        /// </summary>
        public static void HandleOutgoingAttachment(IMessageActivity activity, IMessageActivity reply)
        {
            // Look at the user input, and figure out what kind of attachment to send.
            switch (activity.Text)
            {
                case "1":
                    reply.Text = "This is an inline attachment.";
                    reply.Attachments = new List<Attachment>() { GetInlineAttachment() };
                    break;
                case "2":
                    reply.Text = "This is an attachment from a HTTP URL.";
                    reply.Attachments = new List<Attachment>() { GetInternetAttachment() };
                    break;
                case "3":
                    reply.Text = "This is an uploaded attachment.";

                    // Get the uploaded attachment.
                    var uploadedAttachment = GetUploadedAttachmentAsync(reply.ServiceUrl, reply.Conversation.Id).Result;
                    reply.Attachments = new List<Attachment>() { uploadedAttachment };
                    break;
                default:
                    // The user did not enter input that this bot was built to handle.
                    reply.Text = "Your input was not recognized please try again.";
                    break;
            }
        }

        /// <summary>
        /// Given the input from the message <see cref="Activity"/>, create the response.
        /// </summary>
        /// <param name="turnContext">Provides the <see cref="ITurnContext"/> for the turn of the bot.</param>
        /// <returns>An <see cref="Activity"/> to send as a response.</returns>
        public static Activity ProcessInput(ITurnContext turnContext)
        {
            var activity = turnContext.Activity;
            var reply = activity.CreateReply();

            if (activity.Attachments != null && activity.Attachments.Any())
            {
                // We know the user is sending an attachment as there is at least one item
                // in the Attachments list.
                HandleIncomingAttachment(activity, reply);
            }
            else
            {
                // Send at attachment to the user.
                HandleOutgoingAttachment(activity, reply);
            }

            return reply;
        }

        /// <summary>
        ///  Displays a <see cref="HeroCard"/> with options for the user to select.
        /// </summary>
        /// <param name="turnContext">Provides the <see cref="ITurnContext"/> for the turn of the bot.</param>
        /// <param name="cancellationToken" >(Optional) A <see cref="CancellationToken"/> that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A <see cref="Task"/> representing the operation result of the operation.</returns>
        public static async Task DisplayOptionsAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var reply = turnContext.Activity.CreateReply();

            // Create a HeroCard with options for the user to interact with the bot.
            var card = new HeroCard
            {
                Text = "You can upload an image or select one of the following choices",
                Buttons = new List<CardAction>
                {
                    // Note that some channels require different values to be used in order to get buttons to display text.
                    // In this code the emulator is accounted for with the 'title' parameter, but in other channels you may
                    // need to provide a value for other parameters like 'text' or 'displayText'.
                    new CardAction(ActionTypes.ImBack, title: "1. Inline Attachment", value: "1"),
                    new CardAction(ActionTypes.ImBack, title: "2. Internet Attachment", value: "2"),
                    new CardAction(ActionTypes.ImBack, title: "3. Uploaded Attachment", value: "3"),
                },
            };

            // Add the card to our reply.
            reply.Attachments = new List<Attachment>() { card.ToAttachment() };

            await turnContext.SendActivityAsync(reply, cancellationToken);
        }
    }
}
