using Azure;
using Azure.AI.OpenAI;
using DxBlazorChatToolConfirmation.Components;
using DxBlazorChatToolConfirmation.Services;
using Microsoft.Extensions.AI;

var builder = WebApplication.CreateBuilder(args);

//Replace with your endpoint, API key, and deployed AI model name.
string azureOpenAIEndpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT");
string azureOpenAIKey = Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY");
string deploymentName = string.Empty;

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDevExpressBlazor();
builder.Services.AddMvc();

var azureChatClient = new AzureOpenAIClient(
    new Uri(azureOpenAIEndpoint),
    new AzureKeyCredential(azureOpenAIKey)).GetChatClient(deploymentName).AsIChatClient();

builder.Services.AddScoped<IToolCallFilter, MyToolCallFilter>();

builder.Services.AddScoped(x => {
    return new ChatClientBuilder(azureChatClient)
        .ConfigureOptions(x =>
        {
            x.Tools = [CustomAIFunctions.GetWeatherTool];
        })
        .UseMyToolCallConfirmation()
        .Build(x);
});

builder.Services.AddDevExpressAI();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AllowAnonymous();

app.Run();