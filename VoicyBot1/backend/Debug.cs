using Microsoft.Bot.Builder;
using VoicyBot1.backend.states;

namespace VoicyBot1.backend
{
    public class Debug
    {
        private ConversationData _conversationData;
        private UserProfile _userProfile;
        private VoicyBot1Accessors _accessors;

        public Debug(ConversationData conversationData, UserProfile userProfile, VoicyBot1Accessors accessors)
        {
            _conversationData = conversationData;
            _userProfile = userProfile;
            _accessors = accessors;
        }

        public async void ShowMetaDataAsync(ITurnContext turnContext)
        {
            // Add message details to the conversation data.
            _conversationData.Timestamp = turnContext.Activity.Timestamp.ToString();
            _conversationData.ChannelId = turnContext.Activity.ChannelId;
            // Display state data.
            await turnContext.SendActivityAsync($"{_userProfile.Name} sent: {turnContext.Activity.Text}");
            await turnContext.SendActivityAsync($"Message received at: {_conversationData.Timestamp}");
            await turnContext.SendActivityAsync($"Message received from: {_conversationData.ChannelId}");

            // Update conversation state and save changes.
            await _accessors.ConversationDataAccessor.SetAsync(turnContext, _conversationData);
            await _accessors.ConversationState.SaveChangesAsync(turnContext);
        }
    }
}
