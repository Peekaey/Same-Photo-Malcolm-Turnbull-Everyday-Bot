using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace Same_Photo_Malcolm_Turnbull_Everyday_Discord_Bot.Models;

public class AppConfiguration
{
    public string Token { get; set; } = string.Empty;
    public IEntityToken EntityToken => new BotToken(Token);
    public IReadOnlyDictionary<ulong, ApplicationCommandInfo<SlashCommandContext>> Commands { get; set; }
    public ulong ChannelId { get; set; }
}