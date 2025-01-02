using NetCord;

namespace Same_Photo_Malcolm_Turnbull_Everyday_Discord_Bot.Interfaces;

public interface IPermissionHelper
{
    Task<Channel>? CanAccessChannel(ulong channelId);
}