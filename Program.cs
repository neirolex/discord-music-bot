using discord_music_bot.discord;
using Microsoft.Extensions.DependencyInjection;

namespace discord_music_bot {
    public class Program
    {
        private readonly IServiceProvider _services;

        public Program()
        {
            _services = CreateProvider();
        }

        static IServiceProvider CreateProvider()
        {
            var collection = new ServiceCollection()
                .AddSingleton<DiscordClient>()
                .AddSingleton<Player>();

            return collection.BuildServiceProvider();
        }

        // Program entry point
        public static Task Main(string[] args) => new Program().MainAsync();

        public async Task MainAsync()
        {
            //Initialize discorde client
            var discordClient = _services.GetService<DiscordClient>();

            //This should only runs once for registering new commands
            //Takes 1 hour to register
            //DO NOT run on every start or connection

            //Initialize music player
            var player = _services.GetService<Player>();

            // Block this task until the program is closed.
            await Task.Delay(Timeout.Infinite);
        }
    }
}