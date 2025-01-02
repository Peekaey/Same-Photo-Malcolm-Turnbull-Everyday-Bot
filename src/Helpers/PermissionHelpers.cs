
using Microsoft.Extensions.Logging;
using NetCord;
using NetCord.Rest;
using Same_Photo_Malcolm_Turnbull_Everyday_Discord_Bot.Interfaces;

namespace Same_Photo_Malcolm_Turnbull_Everyday_Discord_Bot.Helpers;

public class PermissionHelpers : IPermissionHelper
{
    private readonly ILogger<PermissionHelpers> _logger;
    private readonly RestClient _restClient;
    
    public PermissionHelpers(ILogger<PermissionHelpers> logger, RestClient restClient)
    {
        _logger = logger;
        _restClient = restClient;
    }
    
    public async Task<Channel>? CanAccessChannel(ulong channelId)
    {
        try
        {
            var channel = await _restClient.GetChannelAsync(channelId);
            return channel;
        }
        catch (RestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Forbidden)
        {
            _logger.LogError($" ❌ Forbidden: Bot lacks access to channel: {channelId}.");
            return null;
        }
        catch (RestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            _logger.LogError($" ❌ Not Found: Channel ID {channelId}: does not exist.");
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"❌ Unexpected error while checking access to channel: {channelId}.");
            return null;
        }
    }
}