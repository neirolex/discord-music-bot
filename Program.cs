using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace discord_music_bot
{
public class Program
    {
        // setup our fields we assign later
        private readonly IConfiguration _config;

        public Program()
        {
            // Config init
            var _builder = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(path: "appsettings.json");  
      
            _config = _builder.Build();
        }

        public static Task Main(string[] args) => new Program().MainAsync(args);

        public async Task MainAsync(string[] args)
        {
            using (var services = ConfigureServices())
            {
                await services.GetRequiredService<ICommandHandler>().InitializeAsync(); //Inializind Discord slash commands handler
                await services.GetRequiredService<DiscordClient>().Init(); //Initializing Discord client

                var builder = WebApplication.CreateBuilder(args);

                var app = builder.Build();
                app.Run();
            }
        }

        //Dependency Injection - load services
        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddOptions().Configure<Options>(_config)
                .AddSingleton<IFilePlayer, FilePlayer>()
                .AddSingleton<IDiscordAudioService, DiscordAudioService>()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<InteractionService>()
                .AddSingleton<InteractionModuleBase<SocketInteractionContext>, Commands>()
                .AddSingleton<ICommandHandler, CommandHandler>()
                .AddSingleton<DiscordClient>()
                .BuildServiceProvider();
        }
    }
}