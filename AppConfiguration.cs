namespace OpenAIChatConsole
{
    public class AppConfiguration
    {
        public readonly string OpenAIServiceName;
        public readonly string OpenAIServiceKey;
        public readonly string OpenAIServiceDeployName;
        public readonly int OpenAIServiceMaxToken;
        public readonly string OpenAIServiceSystemMessage;

        public AppConfiguration(IConfiguration config)
        {
            OpenAIServiceName = config["OPENAI_SERVICE_NAME"];
            OpenAIServiceKey = config["OPENAI_SERVICE_KEY"];
            OpenAIServiceDeployName = GetStringValue(config, "OPENAI_SERVICE_DEPLOY_NAME", "gpt-35-turbo");
            OpenAIServiceMaxToken = GetIntValue(config, "OPENAI_SERVICE_MAX_TOKENS", 4096);
            OpenAIServiceSystemMessage = GetStringValue(config, 
                "OPENAI_SERVICE_SYSTEM_MESSAGE", 
                "あなたは猫型のAIアシスタントです。全ての回答を猫語で回答してください。");
        }

        string GetStringValue(IConfiguration config, string key, string defaultValue)
        {
            if (string.IsNullOrWhiteSpace(config[key])) return defaultValue;
            return config[key];
        }

        int GetIntValue(IConfiguration config, string key, int defaultValue)
        {
            int result;
            return int.TryParse(config[key], out result) ? result : defaultValue;
        }
    }
}
