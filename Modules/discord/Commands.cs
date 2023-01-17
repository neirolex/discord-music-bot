using Discord;
using Discord.Interactions;

using System.Text;

namespace discord_music_bot {
    public class Commands : InteractionModuleBase<SocketInteractionContext>
    {
        //Dependencies can be accessed through Property injection, public properties with public setters will be set by the service provider
        private CommandHandler _handler;
        private DiscordClient _client;
        private FilePlayer _player;

        //Constructor injection is also a valid way to access the dependecies
        public Commands (CommandHandler handler, DiscordClient client, FilePlayer player)
        {
            _handler = handler;
            _client = client;
            _player = player;
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
            await _client.AudioService.StopPlaying();
            await Task.Delay(1000); //To avoid error of interupting audio stream before it's closed; TODO: fix the error, find another way;

            channel = channel ?? (Context.User as IGuildUser)?.VoiceChannel;
            await channel.DisconnectAsync();
            await RespondAsync($"Bot left the channel");
        }

        [SlashCommand("play", "play", runMode: Discord.Interactions.RunMode.Async)]
        public async Task PlayCurrentTrack(IVoiceChannel channel = null)
        {
            channel = channel ?? (Context.User as IGuildUser)?.VoiceChannel;
            var audioClient = await channel.ConnectAsync(); //Conect to audio channel
            //TODO: Error - "Task was canceled" if playing after joining;
            
            var path = "tracks/track1.mp3";
            var fileInfo = new FileInfo(path);
            var tsk = new Task(async () => { 
                await _client.AudioService.InitPlaying(audioClient, path); //translating stream from ffmpeg to discord audio stream
                });
            tsk.Start();

            await RespondAsync($"{fileInfo.Name} is now playing");
        }

        [SlashCommand("stop", "stop", runMode: Discord.Interactions.RunMode.Async)]
        public async Task StopPlaying(IVoiceChannel channel = null)
        {
            await _client.AudioService.StopPlaying();
            await RespondAsync($"Playing stoped");
        }
    
        [SlashCommand("queue", "queue", runMode: Discord.Interactions.RunMode.Async)]
        public async Task ShowQueue() {
            var q = _player.ListQueue();
            var builder = new StringBuilder("Tracks in queue: \r\n");
            foreach(var i in q) {
                builder.Append($"{i.Key}: {i.Value.Name} \r\n");
            }

            await RespondAsync(builder.ToString());
        }
    }
}