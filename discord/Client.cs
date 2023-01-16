using Discord;
using Discord.WebSocket;
using Discord.Commands;

namespace discord_music_bot.discord
{
    public class DiscordClient
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;

        string _token = "MTA2NDUyNzMwNDE0NDk4NjEzNA.GLBAc2.28OyhM6NNTSqcJQ2Ef1SCG4X9w7iuvF7a8BO4E";

        public DiscordClient() { 
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info,
            });
            
            _commands = new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Info,
                CaseSensitiveCommands = false,
            });
            
            // Subscribe the logging handler to both the client and the CommandService.
            _client.Log += Log;
            _commands.Log += Log;

            //Start bot
            Init();
        }

        private async void Init() {
            await _client.LoginAsync(TokenType.Bot, _token);
            await _client.StartAsync();
        }
        
        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}