using Discord;
using Discord.Audio;
using Discord.Interactions;

using System.Text;

namespace discord_music_bot {
    public class Commands : InteractionModuleBase<SocketInteractionContext>
    {
        //Dependencies can be accessed through Property injection, public properties with public setters will be set by the service provider
        private CommandHandler _handler;
        private DiscordClient _client;
        private IAudioClient _audioClient;
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
            _audioClient = await channel.ConnectAsync();
            await RespondAsync($"Bot joined to the channel {channel.Name}");
        }

        [SlashCommand("leave", "leave", runMode: Discord.Interactions.RunMode.Async)]
        public async Task LeaveChannel(IVoiceChannel channel = null)
        {
            //Get the audio channel
            _client.AudioService.StopPlaying();
            await Task.Delay(1000); //To avoid error of interupting audio stream before it's closed; TODO: fix the error, find another way;

            channel = channel ?? (Context.User as IGuildUser)?.VoiceChannel;
            await channel.DisconnectAsync();
            _audioClient.Dispose();
            await RespondAsync($"Bot left the channel");
        }

        [SlashCommand("play", "play", runMode: Discord.Interactions.RunMode.Async)]
        public async Task PlaySelectedTrack(int trackid, IVoiceChannel channel = null)
        {
            channel = channel ?? (Context.User as IGuildUser)?.VoiceChannel;
            if(_audioClient == null) {
                _audioClient = await channel.ConnectAsync();
            }
            
            //if(_client.AudioService.IsPlaying()) { _client.AudioService.StopPlaying(); } //Stop playing current track (to avoid interrupt stream)
            var fileInfo = _player.GetTrackById(trackid).GetFileInfo();
            var tsk = new Task(async () => {
                await _client.AudioService.InitPlaying(_audioClient, fileInfo.FullName); //translating stream from ffmpeg to discord audio stream
                });
            tsk.Start();

            await RespondAsync($"{fileInfo.Name} is now playing");
        }

        [SlashCommand("next", "next", runMode: Discord.Interactions.RunMode.Async)]
        public async Task PlayNextTrack(IVoiceChannel channel = null)
        {          
            channel = channel ?? (Context.User as IGuildUser)?.VoiceChannel;
            if(_audioClient == null) {
                _audioClient = await channel.ConnectAsync();
            }

            //if(_client.AudioService.IsPlaying()) { _client.AudioService.StopPlaying(); } //Stop playing current track (to avoid interrupt stream)
            _player.Next(); //Increase current playing track index
            var fileInfo = _player.GetCurrentTrack().GetFileInfo();
            var tsk = new Task(async () => {
                //cts.Cancel();
                await _client.AudioService.InitPlaying(_audioClient, fileInfo.FullName); //translating stream from ffmpeg to discord audio stream
                });
            tsk.Start();

            await RespondAsync($"{fileInfo.Name} is now playing");
        }

        [SlashCommand("stop", "stop", runMode: Discord.Interactions.RunMode.Async)]
        public async Task StopPlaying(IVoiceChannel channel = null)
        {
            _client.AudioService.StopPlaying();
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