using Discord;
using Discord.WebSocket;

namespace discord_music_bot.discord
{
    public class Client
    {
        private DiscordSocketClient _client;
        string _token = "MTA2NDUyNzMwNDE0NDk4NjEzNA.GLBAc2.28OyhM6NNTSqcJQ2Ef1SCG4X9w7iuvF7a8BO4E";

        public Client() { 
            _client = new DiscordSocketClient();
            _client.Log += Log;
        }

        public async void Init() {
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