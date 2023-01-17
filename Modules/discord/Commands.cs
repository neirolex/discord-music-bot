using Discord;
using Discord.Interactions;
using Discord.Audio;

namespace discord_music_bot {
    public class Commands : InteractionModuleBase<SocketInteractionContext>
    {
        //Dependencies can be accessed through Property injection, public properties with public setters will be set by the service provider
        public InteractionService InteractionService { get; set; }
        private CommandHandler _handler;
        private DiscordClient _client;

        //Constructor injection is also a valid way to access the dependecies
        public Commands (CommandHandler handler, DiscordClient client)
        {
            _handler = handler;
            _client = client;
        }

        [SlashCommand("join", "join", runMode: Discord.Interactions.RunMode.Async)]
        public async Task JoinChannel(IVoiceChannel channel = null)
        {
            //Get the audio channel
            channel = channel ?? (Context.User as IGuildUser)?.VoiceChannel;
            if (channel == null) { await Context.Channel.SendMessageAsync("You must be in a voice channel."); return; }
            var audioClient = await channel.ConnectAsync();
            await RespondAsync($"Bot joined to the channel {channel.Name}");
        }

        [SlashCommand("leave", "leave", runMode: Discord.Interactions.RunMode.Async)]
        public async Task LeaveChannel(IVoiceChannel channel = null)
        {
            //Get the audio channel
            channel = channel ?? (Context.User as IGuildUser)?.VoiceChannel;
            await channel.DisconnectAsync();
            await RespondAsync($"Bot left the channel");
        }

        [SlashCommand("play", "play", runMode: Discord.Interactions.RunMode.Async)]
        public async Task PlayCurrentTrack(IVoiceChannel channel = null)
        {
            var path = "tracks/track1.mp3";
            _client.CreateStream(path);

            channel = channel ?? (Context.User as IGuildUser)?.VoiceChannel;
            var audioClient = await channel.ConnectAsync();
            await _client.SendAsync(audioClient, path);

            await RespondAsync($"___ is now playing");
        }
    }
}