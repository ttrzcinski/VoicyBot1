﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.Bot.Builder;
using VoicyBot1.backend.states;

namespace VoicyBot1
{
    /// <summary>
    /// This class is created as a Singleton and passed into the IBot-derived constructor.
    ///  - See <see cref="VoicyBot1"/> constructor for how that is injected.
    ///  - See the Startup.cs file for more details on creating the Singleton that gets
    ///    injected into the constructor.
    /// </summary>
    public class VoicyBot1Accessors
    {
        /// <summary>
        /// Initializes a new instance of <see cref="VoicyBot1Accessors"/> class.
        /// Contains the <see cref="ConversationState"/> and associated <see cref="IStatePropertyAccessor{T}"/>.
        /// </summary>
        /// <param name="conversationState">The state object that stores the counter.</param>
        /// <param name="userState">the state object that stres user state.</param>
        public VoicyBot1Accessors(ConversationState conversationState, UserState userState)
        {
            ConversationState = conversationState ?? throw new ArgumentNullException(nameof(conversationState));
            UserState = userState ?? throw new ArgumentNullException(nameof(userState));
        }

        /// <summary>
        /// Gets the accessor name for the user profile property accessor.
        /// </summary>
        /// <value>The accessor name for the user profile property accessor.</value>
        /// <remarks>Accessors require a unique name.</remarks>
        public static string UserProfileName { get; } = "UserProfile";

        /// <summary>
        /// Gets the accessor name for the conversation data property accessor.
        /// </summary>
        /// <value>The accessor name for the conversation data property accessor.</value>
        /// <remarks>Accessors require a unique name.</remarks>
        public static string ConversationDataName { get; } = "ConversationData";

        /// <summary>
        /// Gets the <see cref="IStatePropertyAccessor{T}"/> name used for the <see cref="CounterState"/> accessor.
        /// </summary>
        /// <remarks>Accessors require a unique name.</remarks>
        /// <value>The accessor name for the counter accessor.</value>
        public static string CounterStateName { get; } = $"{nameof(VoicyBot1Accessors)}.CounterState";

        /// <summary>
        /// Gets or sets the <see cref="IStatePropertyAccessor{T}"/> for CounterState.
        /// </summary>
        /// <value>
        /// The accessor stores the turn count for the conversation.
        /// </value>
        public IStatePropertyAccessor<CounterState> CounterState { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IStatePropertyAccessor{T}"/> for the user profile property.
        /// </summary>
        /// <value>
        /// The accessor for the user profile property.
        /// </value>
        public IStatePropertyAccessor<UserProfile> UserProfileAccessor { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IStatePropertyAccessor{T}"/> for the conversation data property.
        /// </summary>
        /// <value>
        /// The accessor for the conversation data property.
        /// </value>
        public IStatePropertyAccessor<ConversationData> ConversationDataAccessor { get; set; }

        /// <summary>
        /// Gets the <see cref="ConversationState"/> object for the conversation.
        /// </summary>
        /// <value>The <see cref="ConversationState"/> object.</value>
        public ConversationState ConversationState { get; }

        /// <summary>
        /// Gets the <see cref="UserState"/> object for the bot.
        /// </summary>
        /// <value>The <see cref="UserState"/> object.</value>
        public UserState UserState { get; }
    }
}
