using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace discord_music_bot
{
class Program
    {
        // setup our fields we assign later
        private readonly IConfiguration _config;
        private DiscordSocketClient _client;
        private InteractionService _commands;
        private ulong _guildId;

        public Program()
        {
            // Config init
            var _builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory);
                //.AddJsonFile(path: "config.json");  
      
            _config = _builder.Build();
            _guildId = ulong.Parse("706622645105328168");
        }

        public static Task Main(string[] args) => new Program().MainAsync();

        public async Task MainAsync()
        {
             using (var services = ConfigureServices())
            {
                //Client and command module initialization
                var client = services.GetRequiredService<DiscordSocketClient>();
                var commands = services.GetRequiredService<InteractionService>();
                _client = client;
                _commands = commands;

                //Logging
                client.Log += LogAsync;
                commands.Log += LogAsync;

                //Do when ready
                client.Ready += ReadyAsync;

                //Login and start bot
                await client.LoginAsync(TokenType.Bot, "MTA2NDUyNzMwNDE0NDk4NjEzNA.G2elQs._r-AtI5BCRcUpph4LPZ8GS2qXTDDxfFhIUyP3w");
                await client.StartAsync();

                // Initialize the CommandHandler service
                await services.GetRequiredService<CommandHandler>().InitializeAsync();

                await Task.Delay(Timeout.Infinite);
            }
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        private async Task ReadyAsync()
        {
            // if (IsDebug())
            // {
            //     System.Console.WriteLine($"In debug mode, adding commands to {_guildId}...");
            //     await _commands.RegisterCommandsToGuildAsync(_guildId);
            // }
            // else
            // {
            //     // this method will add commands globally, but can take around an hour
            //     await _commands.RegisterCommandsGloballyAsync(true);
            // }

            //Loading commands to the server (localy)
            System.Console.WriteLine($"Adding commands to {_guildId}...");
            await _commands.RegisterCommandsToGuildAsync(_guildId);
            Console.WriteLine($"Bot successfully connected to the server!");
        }

        //Dependency Injection - load services
        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(_config)
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
                .AddSingleton<CommandHandler>()
                .AddSingleton<DiscordClient>()
                .BuildServiceProvider();
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