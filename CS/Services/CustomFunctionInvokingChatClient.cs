using Microsoft.Extensions.AI;

namespace DxBlazorChatToolConfirmation.Services;
public class CustomFunctionInvokingChatClient : FunctionInvokingChatClient {
    public CustomFunctionInvokingChatClient(IChatClient innerClient, ILoggerFactory? factory = null,
        IServiceProvider? services = null)
        : base(innerClient, factory, services) {
        if(services == null) {
            throw new ArgumentNullException(nameof(services), "Service provider cannot be null.");
        }
        FunctionInvoker = CustomFunctionInvoker;
    }

    private async ValueTask<object?> CustomFunctionInvoker(FunctionInvocationContext context, CancellationToken cancellationToken) {
        IToolCallFilter? filter = FunctionInvocationServices!.GetService<IToolCallFilter>();

        // Check if the filter exists and has FunctionInvoked event handlers attached
        if(await (filter?.InvokeFunctionFilter(context) ?? Task.FromResult(true))) {
            // Proceed with function invocation
            return await context.Function.InvokeAsync(context.Arguments, cancellationToken);
        }

        return null;
    }
}
