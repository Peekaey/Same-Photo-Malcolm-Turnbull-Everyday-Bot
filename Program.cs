using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;


namespace Malcolm_Turnbull_Bot
{
    public class Program
    {
        private static DiscordSocketClient _client;
        public static async Task Main()
        {
            string starting = "Starting Bot...";
            Console.WriteLine(starting);

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("secrets.json");

            IConfiguration configuration = builder.Build();

            _client = new DiscordSocketClient();

            _client.Log += Log;
            
            string discordToken = configuration["DISCORD_TOKEN"];

            //  You can assign your bot token to a string, and pass that in to connect.
            //  This is, however, insecure, particularly if you plan to have your code hosted in a public repository.

            // Some alternative options would be to keep your token in an Environment Variable or a standalone file.
            // var token = Environment.GetEnvironmentVariable("NameOfYourEnvironmentVariable");
            // var token = File.ReadAllText("token.txt");
            // var token = JsonConvert.DeserializeObject<AConfigurationClass>(File.ReadAllText("config.json")).Token;

            await _client.LoginAsync(TokenType.Bot, discordToken);
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
            
        }
        
        private static Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
        
        
    }
}