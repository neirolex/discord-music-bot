using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Options;

namespace discord_music_bot
{
    public class DiscordClient
    {
        private DiscordSocketClient _client;
        private InteractionService _interactionService;
        private ulong _guildId;
        public IDiscordAudioService _audioService; //TODO: Incapslate, make private
        private readonly Options _options;

        public DiscordClient(
                IDiscordAudioService audioService, 
                DiscordSocketClient client, 
                InteractionService interactionService,
                IOptions<Options> options
                ) {
            _options = options.Value;
            _audioService = audioService;
            _client = client;
            _interactionService = interactionService;

            _guildId = _options.DiscordGuildId;

            //Logging
            _client.Log += LogAsync;
            _interactionService.Log += LogAsync;

            //Do when ready
            _client.Ready += ReadyAsync;
        }

        public async Task Init() {
            var token = _options.DiscordApiToken;

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        private async Task ReadyAsync()
        {
            // // this method will add commands globally, but can take around an hour
            // await _commands.RegisterCommandsGloballyAsync(true);

            System.Console.WriteLine($"Adding commands to {_guildId}...");
            await _interactionService.RegisterCommandsToGuildAsync(_guildId);
            Console.WriteLine($"Bot successfully connected to the server!");
        }

        static bool IsDebug ( )
        {
            #if DEBUG
                return true;
            #else
                return false;
            #endif
        }
    }
}