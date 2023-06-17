using Azure;
using Azure.AI.OpenAI;
using Azure.Identity;

namespace OpenAIChatConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();

            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile("local.settings.json", true)
                .Build();

            var appConfig = new AppConfiguration(config);

            // SignalR
            builder.Services.AddSignalR();

            // AppConfiguration
            builder.Services.AddSingleton(provider => appConfig);

            // OpenAIClient
            builder.Services.AddSingleton(provider =>
            {
                var endpoint = new Uri($"https://{appConfig.OpenAIServiceName}.openai.azure.com/");
                return string.IsNullOrEmpty(appConfig.OpenAIServiceKey)
                     ? new OpenAIClient(endpoint, new DefaultAzureCredential())
                     : new OpenAIClient(endpoint, new AzureKeyCredential(appConfig.OpenAIServiceKey));
            });

            // IChatHistoryClient
            builder.Services.AddSingleton<IChatHistoryClient>(provider => new InMemoryChatHistoryClient());


            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chat");
            });

            app.Run();
        }
    }
}
