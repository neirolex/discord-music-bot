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

        public Program()
        {
            // Config init
            var _builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory);
                //.AddJsonFile(path: "config.json");  
      
            _config = _builder.Build();
        }

        public static Task Main(string[] args) => new Program().MainAsync();

        public async Task MainAsync()
        {
            using (var services = ConfigureServices())
            {
                await services.GetRequiredService<DiscordClient>().Init();

                await Task.Delay(Timeout.Infinite);
            }
        }

        //Dependency Injection - load services
        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(_config)
                .AddSingleton<FilePlayer>()
                .AddSingleton<DiscordAudioService>()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
                .AddSingleton<CommandHandler>()
                .AddSingleton(x => new DiscordClient(
                    x.GetRequiredService<DiscordAudioService>(), 
                    x.GetRequiredService<DiscordSocketClient>(), 
                    x.GetRequiredService<InteractionService>(), 
                    x.GetRequiredService<CommandHandler>()
                    ))
                .BuildServiceProvider();
        }
    }
}