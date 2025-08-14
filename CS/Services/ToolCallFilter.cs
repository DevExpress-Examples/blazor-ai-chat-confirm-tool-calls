using Microsoft.Extensions.AI;

namespace DxBlazorChatToolConfirmation.Services;

public class MyToolCallFilter : IToolCallFilter
{
    public event Action<FunctionInvocationContext, TaskCompletionSource<bool>>? ToolCalled;

    /// <summary>
    /// Invokes the FunctionInvoked event if handlers are attached
    /// </summary>
    public Task<bool> InvokeFunctionFilter(FunctionInvocationContext context)
    {
        if (ToolCalled is null)
            return Task.FromResult(true);

        var tcs = new TaskCompletionSource<bool>();
        ToolCalled.Invoke(context, tcs);
        return tcs.Task;
    }
}
