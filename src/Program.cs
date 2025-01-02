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
using Same_Photo_Malcolm_Turnbull_Everyday_Discord_Bot.CommandModules.Backend;
using Same_Photo_Malcolm_Turnbull_Everyday_Discord_Bot.Commands;
using Same_Photo_Malcolm_Turnbull_Everyday_Discord_Bot.Helpers;
using Same_Photo_Malcolm_Turnbull_Everyday_Discord_Bot.Interfaces;
using Same_Photo_Malcolm_Turnbull_Everyday_Discord_Bot.Models;

namespace Same_Photo_Malcolm_Turnbull_Everyday_Discord_Bot;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("🚀 Starting Application...");

        // Validate Token & Channel ID
        var token = Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN");
        if (string.IsNullOrEmpty(token))
        {
            await Console.Error.WriteLineAsync(" ❌ DISCORD_BOT_TOKEN environment variable is not set.");
            throw new InvalidOperationException("DISCORD_BOT_TOKEN environment variable is not set.");
        }
        
        ulong channelId;
        var channelIdString = Environment.GetEnvironmentVariable("DISCORD_CHANNEL_ID");
        if (string.IsNullOrEmpty(channelIdString) || !ulong.TryParse(channelIdString, out channelId))
        {
            await Console.Error.WriteLineAsync(" ❌ DISCORD_CHANNEL_ID environment variable is not set or invalid.");
            throw new InvalidOperationException("DISCORD_CHANNEL_ID environment variable is not set or invalid.");
        }
        
        // Build Host
        var builder = Host.CreateApplicationBuilder(args);
        ConfigureServices(builder.Services, token, channelId);
        var host = builder.Build();
        ConfigureApp(host);
        
        await host.RunAsync();
    }
    
    // Services for the .NET Generic Host
    private static void ConfigureServices(IServiceCollection services, string token, ulong channelId)
    {
        // Configure App Configuration
        services.Configure<AppConfiguration>(options =>
        {
            options.Token = token;
            options.ChannelId = channelId;
        });

        // Logging
        services.AddLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
            logging.AddDebug();
            logging.SetMinimumLevel(LogLevel.Trace);
        });

        // Discord Gateway Service
        services.AddDiscordGateway(options =>
        {
            options.Intents = GatewayIntents.GuildMessages 
                            | GatewayIntents.DirectMessages 
                            | GatewayIntents.MessageContent 
                            | GatewayIntents.Guilds;
            options.Token = token;
        });
        
        // Background Task Scheduler
        services.AddHostedService<TaskSchedulerService>();

        // Slash Command Service
        services.AddApplicationCommands<SlashCommandInteraction, SlashCommandContext, AutocompleteInteractionContext>();
        
        // Rest Client to support API interaction without responding to an interaction first.
        services.AddSingleton<RestClient>(serviceProvider =>
        {
            var environmentOptions = serviceProvider.GetRequiredService<IOptions<AppConfiguration>>().Value;
            return new RestClient(environmentOptions.EntityToken);
        });
        
        // Additional Services
        services.AddSingleton(serviceProvider => serviceProvider.GetRequiredService<IOptions<AppConfiguration>>().Value);
        services.AddSingleton<IBackendTurnbullImage, BackendTurnbullImage>();
        services.AddSingleton<IPermissionHelper, PermissionHelpers>();
        services.AddTransient<BackendTrigger>();
    }
    
    
    private static void ConfigureApp(IHost host)
    {
        // Additional NetCord Configuration - AddModules to automatically register command modules
        // UseGatewayEventHandlers to automatically register gateway event handlers (Respond to Interactions, etc)
        host.AddModules(typeof(Program).Assembly);
        host.UseGatewayEventHandlers();
        
        // Additional Functionality that will be run right after the bot starts
        var lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();
        lifetime.ApplicationStarted.Register(async void () =>
        {
            try
            {
                StoreBotCommands(host);
                await CheckChannelAccess(host,lifetime);
            }
            catch (Exception e)
            {
                Console.WriteLine("❌ Error occurred while running post startup utilities: " + e.Message);
                lifetime.StopApplication();
            }
        });
        Console.WriteLine("✅ Application is configured and ready to run.");
    }

    // Not really needed
    private static void StoreBotCommands(IHost host)
    {
        Console.WriteLine("Preparing to obtain and store bot commands...");
        var command = host.Services.GetRequiredService<ApplicationCommandService<SlashCommandContext>>();
        var commandList = command.GetCommands();
        var appConfiguration = host.Services.GetRequiredService<AppConfiguration>();
        appConfiguration.Commands = commandList;
        Console.WriteLine("Amount of commands stored: " + commandList.Count);
    }

    
    private static async Task CheckChannelAccess(IHost host, IHostApplicationLifetime lifetime)
    {
        var permissionHelper = host.Services.GetRequiredService<IPermissionHelper>();
        var appConfiguration = host.Services.GetRequiredService<AppConfiguration>();
        var logger = host.Services.GetRequiredService<ILogger<Program>>();
        
        var channelAccess = await permissionHelper.CanAccessChannel(appConfiguration.ChannelId);
        if (channelAccess == null)
        {
            logger.LogError(" ❌ Could not confirm access to channel. Exiting application...");
            lifetime.StopApplication();
        }
    }
}