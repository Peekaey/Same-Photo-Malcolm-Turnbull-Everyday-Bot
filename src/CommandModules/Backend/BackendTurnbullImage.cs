using Microsoft.Extensions.Logging;
using NetCord;
using NetCord.Gateway;
using NetCord.Rest;
using Same_Photo_Malcolm_Turnbull_Everyday_Discord_Bot.Helpers;
using Same_Photo_Malcolm_Turnbull_Everyday_Discord_Bot.Interfaces;

namespace Same_Photo_Malcolm_Turnbull_Everyday_Discord_Bot.CommandModules.Backend;

public class BackendTurnbullImage : IBackendTurnbullImage
{
    private readonly ILogger<BackendTurnbullImage> _logger;
    private readonly RestClient _restClient;
    private readonly IPermissionHelper _permissionHelper;
    
    public BackendTurnbullImage(ILogger<BackendTurnbullImage> logger, RestClient restClient, IPermissionHelper permissionHelper)
    {
        _logger = logger;
        _restClient = restClient;
        _permissionHelper = permissionHelper;
    }

    
    public async Task SendMalcolmTurnbullPhoto(ulong channelId)
    {
        try
        {
            var channel = await _permissionHelper.CanAccessChannel(channelId);
            if (channel == null)
            {
                throw new InvalidOperationException("Unable to confirm access to channel when executing SendMalcolmTurnbullPhoto");
            }
            
            var imagePath = Path.Combine(AppContext.BaseDirectory, "Images", "MalcolmTurnbull.jpg");
            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException("Malcolm Turnbull Image Not Found", imagePath);
            }

            using var stream = File.OpenRead(imagePath);
            var attachment = new AttachmentProperties("MalcolmTurnbull.jpg", stream);
            
            await _restClient.SendMessageAsync(channelId, new MessageProperties()
            {
                Attachments = new List<AttachmentProperties> {attachment}
            });
            

        } catch (Exception error) {
            _logger.LogUnhandledError("SendMalcolmTurnbullPhoto", error);
        }

    }
}