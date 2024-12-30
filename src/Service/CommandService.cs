using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetCord;
using NetCord.Gateway;

namespace Same_Photo_Malcolm_Turnbull_Everyday_Discord_Bot.Service;
//
// public class DiscordBotService : BackgroundService
// {
//     private readonly GatewayClient _client;
//     private readonly ILogger<DiscordBotService> _logger;
//     
//     public DiscordBotService(GatewayClient client, ILogger<DiscordBotService> logger)
//     {
//         _client = client;
//         _logger = logger;
//     }
//
//     protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//     {
//         // Subscribe to the Ready Event
//         _client.Ready += () =>
//         {
//             _logger.LogInformation("✅ Discord Bot is successfully logged in and ready!");
//             return Task.
//         };
//
//         try
//         {
//             _logger.LogInformation("🟢 Starting Discord Bot...");
//             await _client.StartAsync();
//             _logger.LogInformation("🚀 Discord Bot started successfully.");
//
//             // Keep the service alive
//             await Task.Delay(-1, stoppingToken);
//         }
//         catch (OperationCanceledException)
//         {
//             _logger.LogInformation("🛑 Discord Bot is shutting down gracefully.");
//         }
//         catch (Exception ex)
//         {
//             _logger.LogError(ex, "❌ An error occurred while running the Discord bot.");
//         }
//         finally
//         {
//             await _client.StopAsync();
//         }
//     }
// }