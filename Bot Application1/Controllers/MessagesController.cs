using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Utilities;

using Newtonsoft.Json;

namespace Bot_Application1
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<Message> Post([FromBody]Message message)
        {
            if (message.Type == "Message")
            {
                // calculate something for us to return
                //int length = (message.Text ?? string.Empty).Length;
                var counter = message.GetBotPerUserInConversationData<int>("counter");

                // save our new counter by adding it to the outgoing message
                //
                Message replyMessage = message;
                if (message.Text == "reset")
                {
                    counter = 0;
                    replyMessage = message.CreateReplyMessage($"{++counter} Counter has been reseted");
                    replyMessage.SetBotPerUserInConversationData("counter", counter);
                }
                else
                {
                    replyMessage = message.CreateReplyMessage($"{++counter} You said:{message.Text}");
                    replyMessage.SetBotPerUserInConversationData("counter", counter);
                }
                
                //return message.CreateReplyMessage("Hey, how can I help you?","en");
                //return null;
                return replyMessage;

            }
            else
            {
                return HandleSystemMessage(message);
            }
        }

        private Message HandleSystemMessage(Message message)
        {
            if (message.Type == "Ping")
            {
                Message reply = message.CreateReplyMessage();
                reply.Type = "Ping";
                return reply;
            }
            else if (message.Type == "DeleteUserData")
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
                return message.CreateReplyMessage("User deleted from Conversation!");
            }
            else if (message.Type == "BotAddedToConversation")
            {
                return message.CreateReplyMessage("Hello BOT!");
            }
            else if (message.Type == "BotRemovedFromConversation")
            {
                return message.CreateReplyMessage("Bot Left Conversation!");
            }
            else if (message.Type == "UserAddedToConversation")
            {
                return message.CreateReplyMessage("Hello User!");
            }
            else if (message.Type == "UserRemovedFromConversation")
            {
                return message.CreateReplyMessage("User Out!");
            }
            else if (message.Type == "EndOfConversation")
            {
                return message.CreateReplyMessage("Conversaton Ended!");
            }
            
            return null;
        }
    }
}