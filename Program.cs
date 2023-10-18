using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.SemanticKernel;

// Add Azure OpenAI package

// Build a config object and retrieve user settings.
IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();
string aoaiEndpoint = config["AzureOAIEndpoint"] ?? "";
string aoaiApiKey = config["AzureOAIKey"] ?? ""; 
string aoaiModel = config["AzureOAIModelName"] ?? "gpt-3.5-turbo";


// See https://aka.ms/new-console-template for more information
Console.WriteLine(aoaiModel);

// Initialize the kernel
IKernel kernel = Kernel.Builder
    .WithAzureChatCompletionService(aoaiModel, aoaiEndpoint, aoaiApiKey)
    .Build();

kernel.RegisterCustomFunction(SKFunction.FromNativeFunction(
    () => $"{DateTime.UtcNow:r}","DateTime", "Now", "Gets the current date and time"));

ISKFunction qa = kernel.CreateSemanticFunction("""
    The current date and time is {{ datetime.now }}.
    {{ $input }}
    """);

// Q&A loop
while (true)
{
    // Console.Write("Question: ");
    // Console.WriteLine((await kernel.InvokeSemanticFunctionAsync(Console.ReadLine()!)).GetValue<string>());
    // Console.WriteLine();

    Console.Write("Question: ");
    Console.WriteLine((await qa.InvokeAsync(Console.ReadLine()!, kernel, functions: kernel.Functions)).GetValue<string>());
    Console.WriteLine();
}