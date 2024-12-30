using NetCord.Services.ApplicationCommands;

namespace Same_Photo_Malcolm_Turnbull_Everyday_Discord_Bot.Commands;

public class HelloWorld: ApplicationCommandModule<SlashCommandContext>
{
    [SlashCommand("hello", "Says Hello World")]
    public string SayHelloWorld(ApplicationCommandContext context)
    {
        return "Hello World!";
    }
}