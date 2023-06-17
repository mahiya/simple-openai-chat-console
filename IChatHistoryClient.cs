using Azure.AI.OpenAI;

namespace OpenAIChatConsole
{
    interface IChatHistoryClient
    {
        List<ChatMessage> GetChatMessages(string settionId);
        void AddChatMessage(string sessionId, ChatRole role, string content);
    }
}
