﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using VoicyBot1.backend;
using VoicyBot1.model;

namespace VoicyBot1
{
    /// <summary>
    /// Represents a bot that processes incoming activities.
    /// For each user interaction, an instance of this class is created and the OnTurnAsync method is called.
    /// This is a Transient lifetime service.  Transient lifetime services are created
    /// each time they're requested. For each Activity received, a new instance of this
    /// class is created. Objects that are expensive to construct, or have a lifetime
    /// beyond the single turn, should be carefully managed.
    /// For example, the <see cref="MemoryStorage"/> object and associated
    /// <see cref="IStatePropertyAccessor{T}"/> object are created with a singleton lifetime.
    /// </summary>
    /// <seealso cref="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.1"/>
    public class VoicyBot1Bot : IBot
    {
        private readonly VoicyBot1Accessors _accessors;
        private readonly ILogger _logger;

        private readonly QuestionsAboutTime _questionsAboutTime;
        private readonly Retorts _retorts;
        private readonly D20 _d20;
        private readonly Images _images;
        private readonly Translation _translation;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="accessors">A class containing <see cref="IStatePropertyAccessor{T}"/> used to manage state.</param>
        /// <param name="loggerFactory">A <see cref="ILoggerFactory"/> that is hooked to the Azure App Service provider.</param>
        /// <seealso cref="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-2.1#windows-eventlog-provider"/>
        public VoicyBot1Bot(VoicyBot1Accessors accessors, ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            _logger = loggerFactory.CreateLogger<VoicyBot1Bot>();
            _logger.LogTrace("Turn start.");
            _accessors = accessors ?? throw new ArgumentNullException(nameof(accessors));

            _questionsAboutTime = new QuestionsAboutTime();
            _retorts = new Retorts(loggerFactory);
            _d20 = new D20();
            _translation = new Translation();
            _images = new Images();
        }

        /// <summary>
        /// Every conversation turn for our Echo Bot will call this method.
        /// There are no dialogs used, since it's "single turn" processing, meaning a single
        /// request and response.
        /// </summary>
        /// <param name="turnContext">A <see cref="ITurnContext"/> containing all the data needed
        /// for processing this conversation turn. </param>
        /// <param name="cancellationToken">(Optional) A <see cref="CancellationToken"/> that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A <see cref="Task"/> that represents the work queued to execute.</returns>
        /// <seealso cref="BotStateSet"/>
        /// <seealso cref="ConversationState"/>
        /// <seealso cref="IMiddleware"/>
        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (turnContext == null)
            {
                throw new ArgumentNullException(nameof(turnContext));
            }

            // Handle Message activity type, which is the main activity type for shown within a conversational interface
            // Message activities may contain text, speech, interactive cards, and binary or unknown attachments.
            // see https://aka.ms/about-bot-activity-message to learn more about the message and other activity types
            if (turnContext.Activity.Type is ActivityTypes.Message)
            {
                // Get the conversation state from the turn context.
                var state = await _accessors.CounterState.GetAsync(turnContext, () => new CounterState());

                // Bump the turn count for this conversation.
                state.TurnCount++;

                // Set the property using the accessor.
                await _accessors.CounterState.SetAsync(turnContext, state);

                // Save the new turn count into the conversation state.
                //await _accessors.ConversationState.SaveChangesAsync(turnContext);

                // Echo back to the user whatever they typed.
                var requestContent = turnContext.Activity.Text.ToLower().Trim();

                // Start with those numbers
                if (UtilRequest.IsNumber(requestContent))
                {
                    // Take the input from the user and create the appropriate response.
                    var reply = Images.ProcessInput(turnContext);

                    // Respond to the user.
                    await turnContext.SendActivityAsync(reply, cancellationToken);

                    await Images.DisplayOptionsAsync(turnContext, cancellationToken);
                    return;
                }

                // Start with checking questions about the time
                string responseMessage = _questionsAboutTime.Respond(requestContent);

                // Start operating with D20
                if (responseMessage == null)
                {
                    responseMessage = _d20.Respond(requestContent);
                }

                // Start operating with those retorts
                if (responseMessage == null)
                {
                    responseMessage = _retorts.Respond(requestContent);
                }
                // Start checking tranalation
                //responseMessage = responseMessage == null ? _translation.Translate(requestContent) : null;
                // If answer was given
                if (responseMessage != null)
                {
                    await turnContext.SendActivityAsync(responseMessage);
                    return;
                }
                // Start operating with images
                if (responseMessage == null && _images.IsToProcess(requestContent))
                {
                    await _images.ShowImage(turnContext.Activity, requestContent);
                    return;
                }
                if (responseMessage == null)
                {
                    await turnContext.SendActivityAsync(responseMessage ?? $"Turn {state.TurnCount}: You sent '{turnContext.Activity.Text}'\n");
                    return;
                }
            }
            else if (turnContext.Activity.Type == ActivityTypes.ConversationUpdate)
            {
                if (turnContext.Activity.MembersAdded != null)
                {
                    // Send a welcome message to the user and tell them what actions they may perform to use this bot
                    await SendWelcomeMessageAsync(turnContext, cancellationToken);
                }
            }
            else
            {
                await turnContext.SendActivityAsync($"{turnContext.Activity.Type} event detected");
            }
        }

        /// <summary>
        /// Greet the user and give them instructions on how to interact with the bot.
        /// </summary>
        /// <param name="turnContext">Provides the <see cref="ITurnContext"/> for the turn of the bot.</param>
        /// <param name="cancellationToken" >(Optional) A <see cref="CancellationToken"/> that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A <see cref="Task"/> representing the operation result of the operation.</returns>
        private static async Task SendWelcomeMessageAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in turnContext.Activity.MembersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(
                        $"Welcome dear {member.Name}, I'm VoicyBot.\n" +
                        $" This bot will introduce you to Attachments." +
                        $" Please select an option",
                        cancellationToken: cancellationToken);
                    await Images.DisplayOptionsAsync(turnContext, cancellationToken);
                }
            }
        }
    }
}
