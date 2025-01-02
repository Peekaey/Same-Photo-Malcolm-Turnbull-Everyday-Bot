using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;
using Same_Photo_Malcolm_Turnbull_Everyday_Discord_Bot.Helpers;

namespace Same_Photo_Malcolm_Turnbull_Everyday_Discord_Bot.Commands;

public class Ping: ApplicationCommandModule<SlashCommandContext>
{
    private readonly ILogger<Ping> _logger;
    private readonly ApplicationCommandService<SlashCommandContext> _commandService;
    
    public Ping(ILogger<Ping> logger,  ApplicationCommandService<SlashCommandContext> commandService)
    {
        _logger = logger;
        _commandService = commandService;
    }

    [SlashCommand("ping", "Returns latency")]
    public async Task ReturnPing()
    {
        try
        {
            await Context.Interaction.SendResponseAsync(InteractionCallback.DeferredMessage());
            
            _logger.LogActionTraceStart(Context, "ReturnPing");

            var latency = Context.Client.Latency;
            await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
            {
                Content = $"Pong! Latency: {latency}ms"
            });

            _logger.LogActionTraceFinish(Context, "ReturnPing");
        } catch (Exception error)
        {
            _logger.LogExceptionError(Context, "ReturnPing", error);
            await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
            {
                Content = $"Error occured when running ping command"
            });
        }
    }
}