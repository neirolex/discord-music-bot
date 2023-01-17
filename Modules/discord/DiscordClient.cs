using System.Diagnostics;

using Discord.Audio;

namespace discord_music_bot
{
    public class DiscordClient
    {
        public DiscordAudioService AudioService; //TODO: Incapslate, make private
        public DiscordClient(DiscordAudioService audioService) {
            AudioService = audioService;
        }
    }

    public class DiscordAudioService {
        public bool Play = true;

        public Process CreateStream(string path)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
            });
        }

        public async Task StartTranslateAudio(IAudioClient client, string path)
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
                            while ((read = ffstream.Read(buffer, 0, buffer.Length)) > 0 && Play)
                            {
                                await discord.WriteAsync(buffer); //Writing bytes to discord audio stream in realtime
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