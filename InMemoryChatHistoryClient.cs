using Azure.AI.OpenAI;

namespace OpenAIChatConsole
{
    class InMemoryChatHistoryClient : IChatHistoryClient
    {
        const int HistoryTake = 10;
        readonly Dictionary<string, List<ChatMessage>> _history = new Dictionary<string, List<ChatMessage>>();

        public List<ChatMessage> GetChatMessages(string sessionId)
        {
            return _history.ContainsKey(sessionId) 
                ? _history[sessionId].TakeLast(HistoryTake).ToList() 
                : new List<ChatMessage>();
        }

        public void AddChatMessage(string sessionId, ChatRole role, string content)
        {
            if(!_history.ContainsKey(sessionId))
                _history.Add(sessionId, new List<ChatMessage>());
            _history[sessionId].Add(new ChatMessage(role, content));
        }
    }
}
