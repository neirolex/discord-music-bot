using Discord;
using Discord.Audio;
using Discord.Interactions;

using System.Text;

namespace discord_music_bot {
    public class Commands : InteractionModuleBase<SocketInteractionContext>
    {
        //Dependencies can be accessed through Property injection, public properties with public setters will be set by the service provide
        private DiscordClient _client;
        private IFilePlayer _player;

        //Constructor injection is also a valid way to access the dependecies
        public Commands (DiscordClient client, IFilePlayer player)
        {
            _client = client;
            _player = player;
        }

        [SlashCommand("join", "join", runMode: Discord.Interactions.RunMode.Async)]
        public async Task JoinChannel(IVoiceChannel channel = null)
        {
            //Get the audio channel
            channel = channel ?? (Context.User as IGuildUser)?.VoiceChannel;
            if (channel == null) { await Context.Channel.SendMessageAsync("You must be in a voice channel."); return; }

            await _client._audioService.Init(channel);
            await RespondAsync($"Bot joined to the channel {channel.Name}");
        }

        [SlashCommand("leave", "leave", runMode: Discord.Interactions.RunMode.Async)]
        public async Task LeaveChannel(IVoiceChannel channel = null)
        {
            //Get the audio channel
            _client._audioService.StopPlaying();
            await Task.Delay(1000); //To avoid error of interupting audio stream before it's closed; TODO: fix the error, find another way;

            channel = channel ?? (Context.User as IGuildUser)?.VoiceChannel;
            await channel.DisconnectAsync();
            _client._audioService.GetAudioClinet().Dispose();
            await RespondAsync($"Bot left the channel");
        }

        [SlashCommand("play", "play", runMode: Discord.Interactions.RunMode.Async)]
        public async Task PlaySelectedTrack(int trackid, IVoiceChannel channel = null)
        {
            //Get the audio channel
            channel = channel ?? (Context.User as IGuildUser)?.VoiceChannel;
            if (channel == null) { await Context.Channel.SendMessageAsync("You must be in a voice channel."); return; }

            await _client._audioService.Init(channel); //TODO: It's a bad idea to init service in slashcommand. Should do it somewhere else.
            var fileName = await _client._audioService.StartPlaying(trackid);
            await RespondAsync($"{fileName} is now playing");
        }

        [SlashCommand("next", "next", runMode: Discord.Interactions.RunMode.Async)]
        public async Task PlayNextTrack(IVoiceChannel channel = null)
        {          
            _player.Next(); //Increase current playing track index
            var fileName = await _client._audioService.StartPlaying(_player.GetCurrentTrackId());
            await RespondAsync($"{fileName} is now playing");
        }

        [SlashCommand("stop", "stop", runMode: Discord.Interactions.RunMode.Async)]
        public async Task StopPlaying(IVoiceChannel channel = null)
        {
            _client._audioService.StopPlaying();
            await RespondAsync($"Playing stoped");
        }
    
        [SlashCommand("queue", "queue", runMode: Discord.Interactions.RunMode.Async)]
        public async Task ShowQueue() {
            var q = _player.GetQueue();
            var builder = new StringBuilder("Tracks in queue: \r\n");
            foreach(var i in q) {
                builder.Append($"{i.Key}: {i.Value.Name} \r\n");
            }

            await RespondAsync(builder.ToString());
        }
    }
}