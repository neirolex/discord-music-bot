using Discord.Audio;
using Discord;

using System.Text;
using System.Diagnostics;

namespace discord_music_bot
{    
    public interface IDiscordAudioService {
        public Task Init(IVoiceChannel channel);

        public IAudioClient GetAudioClinet();
        public Task<string> StartPlaying(int trackid);
        public void StopPlaying();
    }

    public class DiscordAudioService : IDiscordAudioService {
        private bool _isplaying = false;
        private IFilePlayer _player;

        private IAudioClient _audioClient;

        public DiscordAudioService(IFilePlayer player) {
            _player = player;
        }

        public IAudioClient GetAudioClinet() => _audioClient;

        public async Task Init(IVoiceChannel channel) {
            if(_audioClient == null) {
                _audioClient = await channel.ConnectAsync();
            }
        }

        public bool IsPlaying() => _isplaying;

        public async Task<string> StartPlaying(int trackid) {
            TerminateStream();

            _isplaying = true;
            var fileInfo = _player.GetTrackById(trackid).GetFileInfo();
            await TranslateAudio(fileInfo.FullName);

            return fileInfo.Name;
        }

        public void StopPlaying() {
            _isplaying = false;
            TerminateStream();
        }

        private async Task TranslateAudio(string path)
        {
            using (var ffmpeg = _player.CreateStream(path)) //Create process with stream
            using (var ffstream = ffmpeg.StandardOutput.BaseStream)
            using (var discord = _audioClient.CreatePCMStream(AudioApplication.Mixed)) //Create stream to transmit audio to discord AudioClient
            {
                try { 
                    byte[] buffer = new byte[100];
                    int read;
                    //int bytesReaded = 0;
                    var task = new Task(async () => {
                        while ((read = ffstream.Read(buffer, 0, buffer.Length)) > 0 && _isplaying)
                        {
                            //bytesReaded += read;
                            //Console.WriteLine(path);
                            try {
                                //Console.WriteLine(discord.ToString());
                                await discord.WriteAsync(buffer); //Writing bytes to discord audio stream in realtime
                            } catch(OperationCanceledException e) {
                                throw e;
                            }
                        }
                    });
                    task.Start();
                    await Task.Delay(1000); //To avoid "Cannot access to a closed file" error; TODO: fix the error, find another way;
                }
                finally { 
                    await discord.FlushAsync();
                }
            }
        }

        private void TerminateStream()
        {
            _player.TerminateStream();
        }
    }
}