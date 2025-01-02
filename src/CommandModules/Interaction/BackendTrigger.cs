using Microsoft.Extensions.Logging;
using NetCord.Services.ApplicationCommands;
using Same_Photo_Malcolm_Turnbull_Everyday_Discord_Bot.Helpers;
using Same_Photo_Malcolm_Turnbull_Everyday_Discord_Bot.Interfaces;

namespace Same_Photo_Malcolm_Turnbull_Everyday_Discord_Bot.Commands;

public class BackendTrigger : ApplicationCommandModule<SlashCommandContext>
{
    private readonly ILogger<BackendTrigger> _logger;
    private readonly IBackendTurnbullImage _backendTurnbullImage;

    public BackendTrigger(ILogger<BackendTrigger> logger, IBackendTurnbullImage backendTurnbullImage)
    {
        _logger = logger;
        _backendTurnbullImage = backendTurnbullImage;
    }
    
    // Triggers the backend function to send a Malcolm Turnbull photo without initially being triggered by an interaction
    // Remove if actually using bot in production
    [SlashCommand("trigger", "Testing Purposes - Triggers back end function")]
    public async Task TriggerBackend()
    {
        try
        {
            await _backendTurnbullImage.SendMalcolmTurnbullPhoto(1289852264953151594);
        } catch (Exception error)
        {
            _logger.LogUnhandledError("TriggerBackend", error);
        }
    }
}