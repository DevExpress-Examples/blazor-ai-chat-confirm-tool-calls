using Microsoft.Extensions.AI;

namespace DxBlazorChatToolConfirmation.Services;

public interface IToolCallFilter {
    public event Action<FunctionInvocationContext, TaskCompletionSource<bool>> ToolCalled;
    
    /// <summary>
    /// Invokes the FunctionInvoked event if handlers are attached
    /// </summary>
    Task<bool> InvokeFunctionFilter(FunctionInvocationContext context);
}