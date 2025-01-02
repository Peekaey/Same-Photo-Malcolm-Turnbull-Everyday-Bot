using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using NetCord;
using NetCord.Gateway;
using NetCord.JsonModels;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;
using NetCord.Services.Commands;
using Same_Photo_Malcolm_Turnbull_Everyday_Discord_Bot.Helpers;
using Same_Photo_Malcolm_Turnbull_Everyday_Discord_Bot.Interfaces;

namespace Same_Photo_Malcolm_Turnbull_Everyday_Discord_Bot.Commands;

public class InteractionTurnbullImage : ApplicationCommandModule<SlashCommandContext>
{
    private readonly ILogger<InteractionTurnbullImage> _logger;

    public InteractionTurnbullImage(ILogger<InteractionTurnbullImage> logger)
    {
        _logger = logger;
    }
    
    [SlashCommand("malcolmturnbull", "Returns the same photo of malcolm turnbull")]
    public async Task ReturnMalcolmTurnbullPhoto()
    {
        try
        {
            await Context.Interaction.SendResponseAsync(InteractionCallback.DeferredMessage());
            
            _logger.LogActionTraceStart(Context, "ReturnMalcolmTurnbullPhoto");
            
            var imagePath = Path.Combine(AppContext.BaseDirectory, "Images", "MalcolmTurnbull.jpg");
            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException("Malcolm Turnbull Image Not Found", imagePath);
            }

            using var stream = File.OpenRead(imagePath);
            var attachment = new AttachmentProperties("MalcolmTurnbull.jpg", stream);
            
            await Context.Interaction.SendFollowupMessageAsync(
                (new InteractionMessageProperties
                {
                    Attachments = new List<AttachmentProperties> {attachment}
                }));
            
            _logger.LogActionTraceFinish(Context, "ReturnMalcolmTurnbullPhoto");
        } catch (Exception error) {
            _logger.LogExceptionError(Context, "ReturnMalcolmTurnbullPhoto", error);
            await Context.Interaction.SendFollowupMessageAsync(new InteractionMessageProperties
            {
                Content = $"Error occured when running malcolmturnbull command"
            });
        }
    }


}