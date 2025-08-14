using Microsoft.Extensions.AI;

namespace DxBlazorChatToolConfirmation.Services;

public static class CustomFunctionInvokingChatClientExtensions 
{
    public static ChatClientBuilder UseMyToolCallConfirmation(this ChatClientBuilder builder, ILoggerFactory? loggerFactory = null) 
    {
        return builder.Use((innerClient, services) => {
            loggerFactory ??= services.GetService<ILoggerFactory>();
            var chatClient = new CustomFunctionInvokingChatClient(innerClient, loggerFactory, services);
            return chatClient;
        });
    }
}