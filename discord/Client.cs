using System.Reflection;
using Newtonsoft.Json;

using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Discord.Net;

namespace discord_music_bot.discord
{
    public class DiscordClient
    {
        private readonly IServiceProvider _services;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;

        string _token = "MTA2NDUyNzMwNDE0NDk4NjEzNA.GLBAc2.28OyhM6NNTSqcJQ2Ef1SCG4X9w7iuvF7a8BO4E";

        public DiscordClient(IServiceProvider services) {
            _services = services;

            //Configure client
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info,
            });
            _client.Log += Log; //Logging added
            _client.SlashCommandExecuted += SlashCommandHandler; //Handle slash commands from bot

            //_client.Ready += Client_RegisterGuildCommands;
            //_client.Ready += Client_DeleteGuildCommands;
            
            //Configure module for recieving commands from bot
            _commands = new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Info,
                CaseSensitiveCommands = false,
            });
            _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: _services);
            _commands.Log += Log; //Logging added

            //Start bot
            Start();
        }

        private async void Start() {
            await _client.LoginAsync(TokenType.Bot, _token);
            await _client.StartAsync();
        }
        
        private async Task SlashCommandHandler(SocketSlashCommand command)
        {
            await command.RespondAsync($"You executed {command.Data.Name}");
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    
        //This should only runs once for registering new commands!
        //Takes 1 hour to register!
        //DO NOT run on every start or connection!
        public async Task Client_RegisterGlobalCommands()
        {
            var globalCommand = new SlashCommandBuilder();

            // Note: Names have to be all lowercase and match the regular expression ^[\w-]{3,32}$
            globalCommand.WithName("say");
            globalCommand.WithDescription("Echoes a message");

            try
            {
                await _client.CreateGlobalApplicationCommandAsync(globalCommand.Build());
                // It is recommended to only run the CreateGlobalApplicationCommandAsync() once for each command.
            }
            catch(HttpException exception)
            {
                var json = JsonConvert.SerializeObject(exception.Message, Formatting.Indented);
                Console.WriteLine(json);
            }
        }
    
        //Guild commands works only on specified "guild" (server in Discord), defined with guildId param
        public async Task Client_RegisterGuildCommands() {
            ulong guildId = 706622645105328168; //Discord server ID
            var guild = _client.GetGuild(guildId);

            var guildCommand = new SlashCommandBuilder();

            // Note: Names have to be all lowercase and match the regular expression ^[\w-]{3,32}$
            guildCommand.WithName("play");
            guildCommand.WithDescription("Play current track");

            try
            {
                //await guild.DeleteApplicationCommandsAsync(); //Cleanup all commands
                await guild.CreateApplicationCommandAsync(guildCommand.Build());
            }
            catch(HttpException exception)
            {
                var json = JsonConvert.SerializeObject(exception.Message, Formatting.Indented);
                Console.WriteLine(json);
            }
        }
    
        public async Task Client_DeleteGuildCommands() {
            ulong guildId = 706622645105328168; //Discord server ID
            var guild = _client.GetGuild(guildId);

            try
            {
                await guild.DeleteApplicationCommandsAsync();
            }
            catch(HttpException exception)
            {
                var json = JsonConvert.SerializeObject(exception.Message, Formatting.Indented);
                Console.WriteLine(json);
            }
        }
    }
}