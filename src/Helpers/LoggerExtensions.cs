using Microsoft.Extensions.Logging;
using NetCord.Services.ApplicationCommands;

namespace Same_Photo_Malcolm_Turnbull_Everyday_Discord_Bot.Helpers;

public static class ILoggerExtensions
{
    public static async void LogActionTraceStart(this ILogger logger, SlashCommandContext context, string commandName)
    {
        logger.LogTrace($"Start of command {commandName} with interactionId {context.Interaction} initiated by: {context.User} with username: {context.User.GlobalName} " +
                        $"in guild: {context.Guild.Id} with guildname: {context.Guild.Name} " + $"at: {context.Interaction.CreatedAt} ");
    }
    
    public static void LogActionTraceFinish(this ILogger logger, SlashCommandContext context, string commandName)
    {
        logger.LogTrace($"End of command {commandName} with interactionId {context.Interaction.Id} initiated by: {context.User} with username: {context.User.GlobalName} " +
                        $"in guild: {context.Guild.Id} with guildname: {context.Guild.Name} " + $"at: {context.Interaction.CreatedAt} ");
    }

    public static void LogError(this ILogger logger, SlashCommandContext context, string commandName,
        Exception exception)
    {
        logger.LogError(exception, $"Error in command {commandName} with interactionId {context.Interaction.Id} initiated by: {context.User} with username: {context.User.GlobalName} " +
                                   $"in guild: {context.Guild.Id} with guildname: {context.Guild.Name} " + $"at: {context.Interaction.CreatedAt}. Reason: {exception.Message} StackTrace: {exception.StackTrace}");   
    }
}