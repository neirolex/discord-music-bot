using Discord.Audio;
using Discord;

using System.Text;
using System.Diagnostics;

namespace discord_music_bot
{    
    public class DiscordAudioService {
        private string _startTime = "00:00:00";
        private string _playngTime;
        private bool _isplaying = false;

        private CancellationTokenSource cts;

        public IAudioClient AudioClient;
        private FilePlayer _player;

        public DiscordAudioService(FilePlayer player) {
            _player = player;
            cts = new CancellationTokenSource();
        }

        public async Task Init(IVoiceChannel channel) {
            if(AudioClient == null) {
                AudioClient = await channel.ConnectAsync();
            }
        }

        public bool IsPlaying() => _isplaying;

        public async Task<string> StartPlaying(int trackid) {
            //if(IsPlaying()) { StopPlaying(); } //Stop playing current track (to avoid interrupt stream)
            _isplaying = true;
            var fileInfo = _player.GetTrackById(trackid).GetFileInfo();
            await TranslateAudio(fileInfo.FullName);

            return fileInfo.Name;
        }

        public void StopPlaying() {
            _isplaying = false;
        }

        private async Task TranslateAudio(string path)
        {
            using (var ffmpeg = CreateStream(path)) //Create process with stream
            using (var ffstream = ffmpeg.StandardOutput.BaseStream)
            using (var discord = AudioClient.CreatePCMStream(AudioApplication.Mixed)) //Create stream to transmit audio to discord AudioClient
            {
                try { 
                    byte[] buffer = new byte[100];
                    int read;
                    int bytesReaded = 0;
                    var task = new Task(async () => {
                        while ((read = ffstream.Read(buffer, 0, buffer.Length)) > 0 && _isplaying)
                        {
                            bytesReaded += read;
                            Console.WriteLine(path);
                            try {
                                Console.WriteLine(discord.ToString());
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

        private Process CreateStream(string path)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ss {_startTime} -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
            });
        }
    }
}