using Microsoft.Extensions.Logging;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;
using Same_Photo_Malcolm_Turnbull_Everyday_Discord_Bot.Helpers;

namespace Same_Photo_Malcolm_Turnbull_Everyday_Discord_Bot.Commands;

public class Greeting : ApplicationCommandModule<SlashCommandContext>
{
    private readonly ILogger<Greeting> _logger;
    
    public Greeting(ILogger<Greeting> logger)
    {
        _logger = logger;
    }
    // Expression-bodied method example with a command
    // [SlashCommand("greet", "Greet someone!")]
    // public string ReturnGreeting(User user) => $"{Context.User} greets {user}!";

    [SlashCommand("greet", "Greet someone!")]
    public async Task ReturnGreeting(User user)
    {
        try {
            await Context.Interaction.SendResponseAsync(InteractionCallback.DeferredMessage());
            _logger.LogActionTraceStart(Context, "ReturnGreeting");
        
            await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
            {
                Content = $"{Context.User} greets {user}!"
            });
        
            _logger.LogActionTraceFinish(Context, "ReturnGreeting");
        } catch (Exception error) {
            _logger.LogExceptionError(Context, "ReturnGreeting", error);
            await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
            {
                Content = $"Error occured when running running greet command"
            });
        }
    }
}