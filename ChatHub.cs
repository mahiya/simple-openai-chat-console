using Azure.AI.OpenAI;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace OpenAIChatConsole
{
    class ChatHub : Hub
    {
        const string ReceiverMethodName = "receiveMessage";

        readonly OpenAIClient _openAIClient;
        readonly string _openAIDeployName;
        readonly string _openAISystemMessage;
        readonly int _openAIMaxTokens;
        readonly IChatHistoryClient _historyClient;

        public ChatHub(AppConfiguration config, OpenAIClient openAIClient, IChatHistoryClient historyClient)
        {
            _openAIClient = openAIClient;
            _openAIDeployName = config.OpenAIServiceDeployName;
            _openAISystemMessage = config.OpenAIServiceSystemMessage;
            _openAIMaxTokens = config.OpenAIServiceMaxToken;
            _historyClient = historyClient;
        }

        [HubMethodName("sendMessage")]
        public async Task GetChatCompletionAsync(string content)
        {
            // 会話履歴に受け取ったユーザメッセージを追加する
            _historyClient.AddChatMessage(Context.ConnectionId, ChatRole.User, content);

            // 会話履歴を取得する
            var history = _historyClient.GetChatMessages(Context.ConnectionId);

            // 会話履歴の先頭にシステムメッセージを追加する
            history.Insert(0, new ChatMessage(ChatRole.System, _openAISystemMessage));

            // Chat Completion (Streaming) を取得する
            var options = new ChatCompletionsOptions { MaxTokens = _openAIMaxTokens };
            foreach (var chat in history) options.Messages.Add(chat);
            var resp = await _openAIClient.GetChatCompletionsStreamingAsync(_openAIDeployName, options);

            // ストリームで受け取る Chat Completion を逐次送信者に返す
            var completion = "";
            await foreach (var choice in resp.Value.GetChoicesStreaming())
            {
                await foreach (var message in choice.GetMessageStreaming())
                {
                    if (string.IsNullOrEmpty(message.Content)) continue;
                    await Clients.Caller.SendAsync(ReceiverMethodName, new CompletionResponse
                    {
                        Content = message.Content,
                        Finish = false,
                    });
                    completion += message.Content;
                }
            }
            
            // アシスタントメッセージ全体を会話履歴に追加する
            _historyClient.AddChatMessage(Context.ConnectionId, ChatRole.Assistant, completion);

            // Chat Completion のストリームが完了したことを送信者に通知する
            await Clients.Caller.SendAsync(ReceiverMethodName, new CompletionResponse
            {
                Content = "",
                Finish = true,
            });
        }

        class CompletionResponse
        {
            [JsonProperty("content")]
            public string? Content { get; set; }

            [JsonProperty("finish")]
            public bool Finish { get; set; }
        }
    }
}
