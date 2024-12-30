using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetCord;
using NetCord.Gateway;
using NetCord.Hosting.Gateway;
using NetCord.Hosting.Services;
using NetCord.Hosting.Services.ApplicationCommands;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;
using Same_Photo_Malcolm_Turnbull_Everyday_Discord_Bot.Models;

namespace Same_Photo_Malcolm_Turnbull_Everyday_Discord_Bot;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Starting Application...");
        
        // Retrieve environment options
        var token = Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN");
        
        if (string.IsNullOrEmpty(token))
        {
            await Console.Error.WriteLineAsync("DISCORD_BOT_TOKEN environment variable is not set.");
            throw new InvalidOperationException("DISCORD_BOT_TOKEN environment variable is not set.");
        }
        
        var builder = Host.CreateApplicationBuilder(args);
        
        // Configure the Options class with the retrieved token value
        builder.Services.Configure<EnvironmentOptions>(environmentOptions =>
        {
            environmentOptions.Token = token;
        });
        
        // Register ILogger
        builder.Services.AddLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
            logging.AddDebug();
            logging.SetMinimumLevel(LogLevel.Trace);

        });
        
        // Register the DiscordBotService
        builder.Services.AddDiscordGateway(o =>
        {
            o.Intents = GatewayIntents.GuildMessages | GatewayIntents.DirectMessages | GatewayIntents.MessageContent |
                        GatewayIntents.Guilds;
            o.Token = token;
        });
        
        // Register Slash Application Commands
        builder.Services.AddApplicationCommands<SlashCommandInteraction, SlashCommandContext, AutocompleteInteractionContext>();
        
        var host = builder.Build();
        host.AddModules(typeof(Program).Assembly);
        host.UseGatewayEventHandlers();
        
        // Add commands using minimal APIs
        // host.AddSlashCommand("ping", "Ping!", () => "Pong!")
        // host.AddSlashCommand("square", "Square!", (int a) => $"{a}² = {a * a}")
        // .UseGatewayEventHandlers();
        
        await host.RunAsync();
    }
}