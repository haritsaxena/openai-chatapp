using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.ChatCompletion;

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

// Create a new chat
IChatCompletion ai = kernel.GetService<IChatCompletion>();
ChatHistory chat = ai.CreateNewChat("You are an AI assistant that helps people find information.");

while (true)
{
    Console.Write("Question: ");
    chat.AddUserMessage(Console.ReadLine()!);

    string answer = await ai.GenerateMessageAsync(chat);
    chat.AddAssistantMessage(answer);
    Console.WriteLine(answer);

    Console.WriteLine();
}