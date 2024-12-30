using NetCord;

namespace Same_Photo_Malcolm_Turnbull_Everyday_Discord_Bot.Models;

public class EnvironmentOptions
{
    public string Token { get; set; } = string.Empty;
    public IEntityToken EntityToken => new BotToken(Token);
}