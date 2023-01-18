using System.Diagnostics;

using Discord.Audio;

namespace discord_music_bot
{    
    public class DiscordAudioService {
        private bool _isplaying = false;
        private CancellationTokenSource _cts;

        public DiscordAudioService() {
            _cts = new CancellationTokenSource();
        }

        public bool IsPlaying() => _isplaying;

        public async Task InitPlaying(IAudioClient client, string path) {
            CreateStream(path);
            _isplaying = true;
            await TranslateAudio(client, path);
        }

        public void StopPlaying() {
            _isplaying = false;
            //_cts.Cancel();
        }

        private Process CreateStream(string path)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
            });
        }

        private async Task TranslateAudio(IAudioClient client, string path)
        {
            using (var ffmpeg = CreateStream(path)) //Create stream based on ffmpeg binary
            using (var ffstream = ffmpeg.StandardOutput.BaseStream)
            using (var discord = client.CreatePCMStream(AudioApplication.Mixed)) //Create stream to transmit audio to discord AudioClient
            {
                try { 
                    byte[] buffer = new byte[100];
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int read;
                        var task = new Task(async () => {
                            while ((read = ffstream.Read(buffer, 0, buffer.Length)) > 0 && _isplaying)
                            {
                                try {
                                    await discord.WriteAsync(buffer, _cts.Token); //Writing bytes to discord audio stream in realtime
                                } catch(OperationCanceledException e) {
                                    break;  //Fixes "task/operation was canceled", but seems like workaround. Think about better approach.
                                }
                            }
                        });
                        task.Start();
                        await Task.Delay(1000); //To avoid "Cannot access to a closed file" error; TODO: fix the error, find another way;
                    }
                }
                finally { await discord.FlushAsync(); }
            }
        }
    }
}